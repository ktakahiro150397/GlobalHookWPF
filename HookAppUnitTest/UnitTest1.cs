using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HookAppUnitTest
{
    [TestClass]
    public class ModelTests
    {
        /// <summary>
        /// �o�[�W�����v���p�e�B��񂪐������擾�ł��邱�Ƃ��m�F���܂��B
        /// </summary>
        [TestMethod]
        public void VersionInfoPropertyCheck()
        {
            Assert.IsTrue(HookApp.Models.VersionInfoProperty.IsNoError);
        }
    }
}
