using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;
using FaultLocalization.Util;
using Mono.Cecil.Rocks;
namespace ValueReplacement
{
	class ProjectRewriter
	{
		private const int HiddenLineNumber = 0xfeefee;
		private const String BackupExtension = ".prw_bak";
		private static byte[] _id;
		private static byte[] RewriterID
		{
			get
			{
				if(_id == null)
				{
					String loc = typeof(ProjectRewriter).Assembly.Location;
					var hasher = System.Security.Cryptography.SHA512.Create();
					using(var stream = File.OpenRead(loc))
					{
						_id = hasher.ComputeHash(stream);
					}
				}
				return _id;
			}
		}


		private readonly CSProject _proj;
		public CSProject Proj { get { return _proj; } }

		private AssemblyDefinition Assembly;
		private readonly MethodReference instrumentMethod;
		private static ReaderParameters GetReaderParameters(CSProject proj)
		{
			ReaderParameters _params = new ReaderParameters(ReadingMode.Immediate);
			_params.ReadSymbols = true;
			_params.SymbolReaderProvider = new PdbReaderProvider();
			var resolver = new DefaultAssemblyResolver();
			resolver.AddSearchDirectory(Path.GetDirectoryName(proj.AssemblyLocation));
			_params.AssemblyResolver = resolver;
			return _params;
		}
		private static WriterParameters GetWriterParameters(CSProject proj)
		{
			WriterParameters _params = new WriterParameters()
			{
				WriteSymbols = true,
				SymbolWriterProvider = new PdbWriterProvider(),
			};
			return _params;
		}
		public ProjectRewriter(CSProject proj)
		{
			_proj = proj;
			if(!proj.Configuration.Equals("Debug"))
			{
				throw new Exception("Target Project must be built in debug mode for proper rewriting");
			}
			try
			{
				try
				{
					Assembly = AssemblyDefinition.ReadAssembly(proj.AssemblyLocation, GetReaderParameters(proj));
				}
				catch(InvalidOperationException e)//this gets 
				{
					RestoreBackup();
				}
			}
			catch(Exception e)
			{
				throw new Exception("Unable to open target assembly... try rebuilding : " + proj.AssemblyLocation, e);
			}

			MethodInfo instrumentMethodInfo = typeof(ValueInjector.Instrumenter).GetMethod("Instrument", new Type[] { typeof(object), typeof(String), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
			instrumentMethod = Assembly.MainModule.Import(instrumentMethodInfo);
		}

		private static bool ArraysEqual<T>(T[] ar1, T[] ar2)
		{
			if(ar1.Length == ar2.Length)
			{
				for(int i = 0; i < ar1.Length; i++)
				{
					if(!ar1[i].Equals(ar2[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private Dictionary<Type, TypeReference> TypeLookup = new Dictionary<Type, TypeReference>();
		private TypeReference GetTypeReference(Type t)
		{
			TypeReference r;
			if(!TypeLookup.TryGetValue(t, out r))
			{
				r = Assembly.MainModule.Import(t);
				TypeLookup[t] = r;
			}
			return r;
		}

		private bool RewritePrepare()
		{
			var attribs = Assembly.CustomAttributes
							.Where(ca => ca.AttributeType.FullName.Equals(typeof(ValueInjector.ValueInjectedAttribute).FullName));
			if(attribs.Any())
			{
				//we have already instrumented this assembly
				var injected = attribs.First();
				var arg = injected.ConstructorArguments.First();
				if(arg.Type.Equals(typeof(byte[])) && ArraysEqual((byte[]) arg.Value, RewriterID))
				{
					return false;//we have used the same version
				}
				else
				{
					//we already rewrote it but we used an old version so we should update it
					//so we will restore the backup and then proceed as normal
					RestoreBackup();
				}
			}
			return true;
		}

		private void MarkAssembly()
		{
			var constructorInfo = typeof(ValueInjector.ValueInjectedAttribute).GetConstructor(new Type[] { typeof(byte[]) });
			var constructor = Assembly.MainModule.Import(constructorInfo);
			CustomAttribute ca = new CustomAttribute(constructor);
			ca.ConstructorArguments.Add(new CustomAttributeArgument(GetTypeReference(typeof(byte[])), RewriterID));
			Assembly.CustomAttributes.Add(ca);
		}

		public void Rewrite()
		{
			if(!RewritePrepare())
				return;
			MarkAssembly();

			var methods = Assembly.Modules
				.SelectMany(m => m.Types)
				.SelectMany(t => t.Methods)
				.Where(m => m.HasBody && m.IsIL);

			foreach(var method in methods)
			{
				RewriteMethod(method);
			}
			SaveModifications();
		}

		private void BackupFile(String fn)
		{
			String backup = fn + BackupExtension;
			File.Copy(fn, backup, true);
		}

		private void SaveModifications()
		{
			BackupFile(Proj.AssemblyLocation);
			BackupFile(Path.ChangeExtension(Proj.AssemblyLocation, "pdb"));
			Assembly.Write(Proj.AssemblyLocation, GetWriterParameters(Proj));
		}
		private void UnbackupFile(String fn)
		{
			String backup = fn + BackupExtension;
			if(File.Exists(backup))
			{
				File.Copy(backup, fn, true);
				File.Delete(backup);
			}
			throw new Exception("Cannot find backup for " + fn);
		}
		private void RestoreBackup()
		{
			UnbackupFile(Proj.AssemblyLocation);
			UnbackupFile(Path.ChangeExtension(Proj.AssemblyLocation, "pdb"));
			Assembly = AssemblyDefinition.ReadAssembly(Proj.AssemblyLocation, GetReaderParameters(Proj));
		}
		private IEnumerable<Instruction> PreviousIterator(Instruction i)
		{
			while(i.Previous != null)
			{
				i = i.Previous;
				yield return i;
			}
		}
		private void RewriteMethod(MethodDefinition method)
		{
			int id = 0;
			method.Body.SimplifyMacros();
			var proc = method.Body.GetILProcessor();
			
			//we do a ToList here in order to get a copy of the list, that way we don't accidentally 
			//hook the same instructions twice
			var toHook = method.Body.Instructions.Where(i => !IsHiddenLine(i.SequencePoint))
				.Select(i =>
					{
						TypeReference tr;
						var b = CanHookInstruction(method, i, out tr);
						return new { b, tr, i };
					})
				.Where(a => a.b)
				.ToList();

			foreach(var a in toHook)
			{
				bool should_box = a.tr.IsValueType;
				List<Instruction> sequence = new List<Instruction>();
				//some instructions randomly don't have
				var location = a.i.SequencePoint ?? PreviousIterator(a.i).Where(i => i.SequencePoint != null && !IsHiddenLine(i.SequencePoint)).First().SequencePoint;
				var varId = id;
				id++;
				//conditionally box the targeted value
				if(should_box)
				{
					sequence.Add(proc.Create(OpCodes.Box, a.tr));
				}
				//add all the remaining operands

				//sequence.Add(proc.Create(OpCodes.Castclass, GetTypeReference(typeof(object))));
				sequence.Add(proc.Create(OpCodes.Ldstr, location.Document.Url));
				sequence.Add(proc.Create(OpCodes.Ldc_I4, location.StartLine));
				sequence.Add(proc.Create(OpCodes.Ldc_I4, location.EndLine));
				sequence.Add(proc.Create(OpCodes.Ldc_I4, location.StartColumn));
				sequence.Add(proc.Create(OpCodes.Ldc_I4, location.EndColumn));
				sequence.Add(proc.Create(OpCodes.Ldc_I4, varId));
				//call our method
				sequence.Add(proc.Create(OpCodes.Call, instrumentMethod));
				//sequence.Add(proc.Create(OpCodes.Castclass, a.tr));

				//if we boxed before we should unbox now
				if(should_box)
				{
					sequence.Add(proc.Create(OpCodes.Unbox_Any, a.tr));
				}

				//now insert the instructions at the appropriate location
				var pi = a.i;
				foreach(var ni in sequence)
				{
					ni.SequencePoint = MakeHiddenLine(location);
					proc.InsertAfter(pi, ni);
					pi = ni;
				}

			}
			//method.Body.OptimizeMacros();
		}

		private static bool IsHiddenLine(SequencePoint pt)
		{
			return pt!= null && pt.StartLine == HiddenLineNumber && pt.EndLine == HiddenLineNumber;
		}

		private static SequencePoint MakeHiddenLine(SequencePoint pt)
		{
			return new SequencePoint(pt.Document) { EndColumn = 0, StartColumn = 0, EndLine = HiddenLineNumber, StartLine = HiddenLineNumber };
		}

		private bool CanHookInstruction(MethodDefinition method, Instruction i, out TypeReference ValueType)
		{
			ValueType = null;
			switch(i.OpCode.Code)
			{
				#region Load Commands
				#region locals
				case Code.Ldloc:
					var vd = (VariableDefinition) i.Operand;
					ValueType = vd.VariableType;
					return true;
				case Code.Ldloc_0:
				case Code.Ldloc_1:
				case Code.Ldloc_2:
				case Code.Ldloc_3:
				case Code.Ldloc_S:
				case Code.Ldloca:
				case Code.Ldloca_S:
					throw new NotSupportedException();//we shouldnt see this
				#endregion
				#region fields
				case Code.Ldflda:
				case Code.Ldsflda:
					throw new NotImplementedException();
				case Code.Ldfld:
				case Code.Ldsfld:
					var fr = (FieldReference) i.Operand;
					ValueType = fr.FieldType;
					return true;
				#endregion
				#region arguments
				case Code.Ldarg:
					var pr = (ParameterReference) i.Operand;
					if(method.HasThis && pr.Equals(method.Body.ThisParameter))
					{
						ValueType = method.Body.ThisParameter.ParameterType;
						return false;//doing value replacement on 'this' is (based on a small amount of observation) a bad idea
					}
					ValueType = method.Parameters[pr.Index].ParameterType;
					return true;
				case Code.Ldarg_0:
				case Code.Ldarg_1:
				case Code.Ldarg_2:
				case Code.Ldarg_3:
				case Code.Ldarg_S:
					throw new NotSupportedException();//we shouldnt see this
				case Code.Ldarga:
					throw new NotImplementedException();//not sure how to do this yet
				case Code.Ldarga_S:
					throw new NotSupportedException();//we shouldnt see this
				#endregion
				#region elements
				case Code.Ldelem_Any:
				case Code.Ldelem_I:
				case Code.Ldelem_I1:
				case Code.Ldelem_I2:
				case Code.Ldelem_I4:
				case Code.Ldelem_I8:
				case Code.Ldelem_R4:
				case Code.Ldelem_R8:
				case Code.Ldelem_Ref:
				case Code.Ldelem_U1:
				case Code.Ldelem_U2:
				case Code.Ldelem_U4:
				case Code.Ldelema:
					throw new NotImplementedException();
				#endregion

				case Code.Ldftn:
					return false;//we don't handle method pointers
				case Code.Ldind_I:
				case Code.Ldind_I1:
				case Code.Ldind_I2:
				case Code.Ldind_I4:
				case Code.Ldind_I8:
				case Code.Ldind_R4:
				case Code.Ldind_R8:
				case Code.Ldind_Ref:
				case Code.Ldind_U1:
				case Code.Ldind_U2:
				case Code.Ldind_U4:
				case Code.Ldlen:
					return false;

				case Code.Ldobj://loads an object
					throw new NotImplementedException();

				case Code.Ldtoken:
				case Code.Ldvirtftn:
					return false;//not sure how to handle these yet
				#region Load Constants,  no point in intercepting these because they never change
				case Code.Ldstr:
				case Code.Ldnull:
				case Code.Ldc_I4:
				case Code.Ldc_I4_0:
				case Code.Ldc_I4_1:
				case Code.Ldc_I4_2:
				case Code.Ldc_I4_3:
				case Code.Ldc_I4_4:
				case Code.Ldc_I4_5:
				case Code.Ldc_I4_6:
				case Code.Ldc_I4_7:
				case Code.Ldc_I4_8:
				case Code.Ldc_I4_M1:
				case Code.Ldc_I4_S:
				case Code.Ldc_I8:
				case Code.Ldc_R4:
				case Code.Ldc_R8:
					return false;
				#endregion
				#endregion
				#region Non load instructions
				case Code.Add:
				case Code.Add_Ovf:
				case Code.Add_Ovf_Un:
				case Code.And:
				case Code.Arglist:
				case Code.Beq:
				case Code.Beq_S:
				case Code.Bge:
				case Code.Bge_S:
				case Code.Bge_Un:
				case Code.Bge_Un_S:
				case Code.Bgt:
				case Code.Bgt_S:
				case Code.Bgt_Un:
				case Code.Bgt_Un_S:
				case Code.Ble:
				case Code.Ble_S:
				case Code.Ble_Un:
				case Code.Ble_Un_S:
				case Code.Blt:
				case Code.Blt_S:
				case Code.Blt_Un:
				case Code.Blt_Un_S:
				case Code.Bne_Un:
				case Code.Bne_Un_S:
				case Code.Box:
				case Code.Br:
				case Code.Br_S:
				case Code.Break:
				case Code.Brfalse:
				case Code.Brfalse_S:
				case Code.Brtrue:
				case Code.Brtrue_S:
				case Code.Call:
				case Code.Calli:
				case Code.Callvirt:
				case Code.Castclass:
				case Code.Ceq:
				case Code.Cgt:
				case Code.Cgt_Un:
				case Code.Ckfinite:
				case Code.Clt:
				case Code.Clt_Un:
				case Code.Constrained:
				case Code.Conv_I:
				case Code.Conv_I1:
				case Code.Conv_I2:
				case Code.Conv_I4:
				case Code.Conv_I8:
				case Code.Conv_Ovf_I:
				case Code.Conv_Ovf_I1:
				case Code.Conv_Ovf_I1_Un:
				case Code.Conv_Ovf_I2:
				case Code.Conv_Ovf_I2_Un:
				case Code.Conv_Ovf_I4:
				case Code.Conv_Ovf_I4_Un:
				case Code.Conv_Ovf_I8:
				case Code.Conv_Ovf_I8_Un:
				case Code.Conv_Ovf_I_Un:
				case Code.Conv_Ovf_U:
				case Code.Conv_Ovf_U1:
				case Code.Conv_Ovf_U1_Un:
				case Code.Conv_Ovf_U2:
				case Code.Conv_Ovf_U2_Un:
				case Code.Conv_Ovf_U4:
				case Code.Conv_Ovf_U4_Un:
				case Code.Conv_Ovf_U8:
				case Code.Conv_Ovf_U8_Un:
				case Code.Conv_Ovf_U_Un:
				case Code.Conv_R4:
				case Code.Conv_R8:
				case Code.Conv_R_Un:
				case Code.Conv_U:
				case Code.Conv_U1:
				case Code.Conv_U2:
				case Code.Conv_U4:
				case Code.Conv_U8:
				case Code.Cpblk:
				case Code.Cpobj:
				case Code.Div:
				case Code.Div_Un:
				case Code.Dup:
				case Code.Endfilter:
				case Code.Endfinally:
				case Code.Initblk:
				case Code.Initobj:
				case Code.Isinst:
				case Code.Jmp:
				case Code.Leave:
				case Code.Leave_S:
				case Code.Localloc:
				case Code.Mkrefany:
				case Code.Mul:
				case Code.Mul_Ovf:
				case Code.Mul_Ovf_Un:
				case Code.Neg:
				case Code.Newarr:
				case Code.Newobj:
				case Code.No:
				case Code.Nop:
				case Code.Not:
				case Code.Or:
				case Code.Pop:
				case Code.Readonly:
				case Code.Refanytype:
				case Code.Refanyval:
				case Code.Rem:
				case Code.Rem_Un:
				case Code.Ret:
				case Code.Rethrow:
				case Code.Shl:
				case Code.Shr:
				case Code.Shr_Un:
				case Code.Sizeof:
				case Code.Starg:
				case Code.Starg_S:
				case Code.Stelem_Any:
				case Code.Stelem_I:
				case Code.Stelem_I1:
				case Code.Stelem_I2:
				case Code.Stelem_I4:
				case Code.Stelem_I8:
				case Code.Stelem_R4:
				case Code.Stelem_R8:
				case Code.Stelem_Ref:
				case Code.Stfld:
				case Code.Stind_I:
				case Code.Stind_I1:
				case Code.Stind_I2:
				case Code.Stind_I4:
				case Code.Stind_I8:
				case Code.Stind_R4:
				case Code.Stind_R8:
				case Code.Stind_Ref:
				case Code.Stloc:
				case Code.Stloc_0:
				case Code.Stloc_1:
				case Code.Stloc_2:
				case Code.Stloc_3:
				case Code.Stloc_S:
				case Code.Stobj:
				case Code.Stsfld:
				case Code.Sub:
				case Code.Sub_Ovf:
				case Code.Sub_Ovf_Un:
				case Code.Switch:
				case Code.Tail:
				case Code.Throw:
				case Code.Unaligned:
				case Code.Unbox:
				case Code.Unbox_Any:
				case Code.Volatile:
				case Code.Xor:
				default:
					return false;
				#endregion
			}
		}

		private VariableDefinition GetLocal(MethodDefinition method, int index)
		{
			var scope = method.Body.Scope;
			return scope.Variables[index];
		}
	}
}
