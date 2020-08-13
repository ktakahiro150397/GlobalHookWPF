using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HookAppUnitTest
{
    [TestClass]
    public class ModelTests
    {
        /// <summary>
        /// バージョンプロパティ情報が正しく取得できることを確認します。
        /// </summary>
        [TestMethod]
        public void VersionInfoPropertyCheck()
        {
            Assert.IsTrue(HookApp.Models.VersionInfoProperty.IsNoError);
        }
    }
}
