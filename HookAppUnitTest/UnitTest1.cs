using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HookAppUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void VersionInfoPropertyCheck()
        {
            Assert.IsTrue(HookApp.Models.VersionInfoProperty.IsNoError);
        }
    }
}
