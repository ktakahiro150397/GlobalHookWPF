using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace HookAppUnitTest.Models
{
    [TestClass]
    public class AppSettingsModelTests
    {
        /// <summary>
        /// Resources以下のフォルダ構成が取得できるかどうかテスト
        /// </summary>
        [TestMethod]
        public void ObtainFolderListTest()
        {
            AppSettingsModel settingVm = new AppSettingsModel();
            List<string> ansFolderList = new List<string> { "Default","Test2","Test3" };

            Assert.IsTrue(settingVm.SkinFolderNameList.SequenceEqual(ansFolderList));
            Assert.IsTrue(settingVm.SelectedSkinFolderName == "Default");
        }




    }
}
