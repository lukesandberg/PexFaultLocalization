using FP.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XunitExtensions;

namespace FPTests {
	[TestClass]
    public class RefTests {
        private readonly IRef<int> _refInt = Ref.New(0);
        private readonly IRef<string> _refString = Ref.New("");
        private readonly IRef<bool> _refBool = Ref.New(true);
        private readonly IRef<long> _refLong = Ref.New(0L);

		[TestMethod]
        public void Test_RefNewPicksCorrectType() {
            Assert2.IsType<RefInt>(_refInt);
            Assert2.IsType<RefObject<string>>(_refString);
            Assert2.IsType<RefBool>(_refBool);
            Assert2.IsType<RefLong>(_refLong);
        }
    }
}