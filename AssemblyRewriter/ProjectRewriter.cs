using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using System.IO;
using System.Reflection;
using Mono.Cecil.Cil;

namespace AssemblyRewriter
{
	class ProjectRewriter
	{
		private readonly CSProject Proj;
		private readonly String outDirectory;
		private readonly AssemblyDefinition Assembly;
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
		public ProjectRewriter(CSProject proj, String outDir)
		{
			Proj = proj;
			outDirectory = outDir;
			if(!proj.Configuration.Equals("Debug"))
			{
				throw new Exception("Target Project must be built in debug mode for proper rewriting");
			}
			try
			{
				Assembly = AssemblyDefinition.ReadAssembly(proj.AssemblyLocation, GetReaderParameters(proj));
			}
			catch(Exception e)
			{
				throw new Exception("Unable to open target assembly... has it been built? : " + proj.AssemblyLocation, e);
			}
			MethodInfo instrumentMethodInfo = typeof(ValueInjector.ValueInjector).GetMethod("Instrument", new Type[] { typeof(object), typeof(String), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
			instrumentMethod = Assembly.MainModule.Import(instrumentMethodInfo);

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

		public void Rewrite()
		{
			var methods = Assembly.Modules
				.SelectMany(m => m.Types)
				.SelectMany(t => t.Methods)
				.Where(m => m.HasBody && m.IsIL);
			foreach(var method in methods)
			{
				RewriteMethod(method);
			}

			Assembly.Write(Path.Combine(outDirectory, Path.GetFileName(Proj.AssemblyLocation)));
		}

		private void RewriteMethod(MethodDefinition method)
		{
			int id = 0;
			var proc = method.Body.GetILProcessor();
			//we do a ToList here in order to get a copy of the list, that way we don't accidentally 
			//hook the same instructions twice
			foreach(var inst in method.Body.Instructions.ToList())
			{
				bool should_box;
				TypeReference type;
				if(inst.SequencePoint != null && CanHookInstruction(method, inst, out should_box, out type))
				{
					List<Instruction> sequence = new List<Instruction>();
					var location = inst.SequencePoint;
					var varId = id;
					id++;
					//conditionally box the targeted value
					if(should_box)
					{
						sequence.Add(proc.Create(OpCodes.Box, type));
					}
					//add all the remaining operands
					sequence.Add(proc.Create(OpCodes.Ldstr, location.Document.Url));
					sequence.Add(proc.Create(OpCodes.Ldc_I4, location.StartLine));
					sequence.Add(proc.Create(OpCodes.Ldc_I4, location.EndLine));
					sequence.Add(proc.Create(OpCodes.Ldc_I4, location.StartColumn));
					sequence.Add(proc.Create(OpCodes.Ldc_I4, location.EndColumn));
					sequence.Add(proc.Create(OpCodes.Ldc_I4, varId));
					//call our method
					sequence.Add(proc.Create(OpCodes.Call, instrumentMethod));
					//if we boxed before we should unbox now
					if(should_box)
					{
						sequence.Add(proc.Create(OpCodes.Unbox, type));
					}

					//now insert the instructions at the appropriate location
					var pi = inst;
					foreach(var ni in sequence)
					{
						proc.InsertAfter(pi, ni);
						pi = ni;
					}
				}
			}
		}

		private bool CanHookInstruction(MethodDefinition method, Instruction i, out bool should_box, out TypeReference ValueType)
		{
			should_box = false;
			ValueType = null;
			switch(i.OpCode.Code)
			{
				#region Load Commands
				case Code.Ldarg:
					throw new NotImplementedException();
				case Code.Ldarg_0:
					ValueType = method.Parameters[0].ParameterType;
					should_box = method.Parameters[0].ParameterType.IsValueType;
					return true;
				case Code.Ldarg_1:
					ValueType = method.Parameters[1].ParameterType;
					should_box = method.Parameters[1].ParameterType.IsValueType;
					return true;
				case Code.Ldarg_2:
					ValueType = method.Parameters[2].ParameterType;
					should_box = method.Parameters[2].ParameterType.IsValueType;
					return true;
				case Code.Ldarg_3:
					ValueType = method.Parameters[3].ParameterType;
					should_box = method.Parameters[3].ParameterType.IsValueType;
					return true;
				case Code.Ldarg_S:
					throw new NotImplementedException();
				case Code.Ldarga:
					throw new NotImplementedException();
				case Code.Ldarga_S:
					throw new NotImplementedException();
				case Code.Ldelem_Any:
					throw new NotImplementedException();
				case Code.Ldelem_I:
					should_box = true;
					ValueType = GetTypeReference(typeof(int));
					return true;
				case Code.Ldelem_I1:
				case Code.Ldelem_I2:
				case Code.Ldelem_I4:
					should_box = true;
					ValueType = GetTypeReference(typeof(Int32));
					return true;
				case Code.Ldelem_I8:
					should_box = true;
					ValueType = GetTypeReference(typeof(Int64));
					return true;
				case Code.Ldelem_R4:
					should_box = true;
					ValueType = GetTypeReference(typeof(float));
					return true;
				case Code.Ldelem_R8:
					should_box = true;
					ValueType = GetTypeReference(typeof(double));
					return true;
				case Code.Ldelem_Ref:
					should_box = false;
					return true;
				case Code.Ldelem_U1:
				case Code.Ldelem_U2:
				case Code.Ldelem_U4:
				case Code.Ldelema:
				case Code.Ldfld:
				case Code.Ldflda:
				case Code.Ldftn:
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
				case Code.Ldloc:
				case Code.Ldloc_0:
				case Code.Ldloc_1:
				case Code.Ldloc_2:
				case Code.Ldloc_3:
				case Code.Ldloc_S:
				case Code.Ldloca:
				case Code.Ldloca_S:
				case Code.Ldobj:
				case Code.Ldsfld:
				case Code.Ldsflda:
					return true;
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
	}
}
