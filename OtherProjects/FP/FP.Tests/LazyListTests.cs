using System.Collections.Generic;
using FP.Collections.Persistent;
using FP.Core;
using Microsoft.Pex.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPTests {
    [PexClass(typeof(LazyList<>))]
	[TestClass]
    public partial class LazyListTests {
        [PexMethod]
        [PexGenericArguments(typeof(int), typeof(object))]
        public void Test_Enumeration<T>([PexAssumeNotNull] T[] array) {
            PexAssert.AreElementsEqual(array.ToLazyList(), array, (x, y) => EqualityComparer<T>.Default.Equals(x, y));
        }
    }
}
