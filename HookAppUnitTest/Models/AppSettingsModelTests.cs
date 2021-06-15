using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Inasync;

namespace HookAppUnitTest.Models
{
    [TestClass]
    public class AppSettingsModelTests : TestBase
    {
        /// <summary>
        /// Resources以下のフォルダ構成が取得できるかどうかテスト
        /// </summary>
        [TestMethod]
        public void ObtainFolderListTest()
        {
            AppSettingsModel settingVm = new AppSettingsModel();
            List<string> ansFolderList = new List<string> { "Default","Test2","Test3","yamlFormatInvalid","yamlReadTest"};

            ansFolderList.AssertIs(settingVm.SkinFolderNameList);
        }

        /// <summary>
        /// Resourcesまでの絶対パスを取得できるかどうかテスト
        /// </summary>
        [TestMethod]
        public void ObtainLocalPath()
        {
            AppSettingsModel settingVm = new AppSettingsModel();

            var expected = @"C:\Users\Takahiro\source\repos\ktakahiro150397\GlobalHookWPF\HookAppUnitTest\bin\Debug\netcoreapp3.1\Resources\Default\KeyDisplaySetting.yaml";

            settingVm.SelectedSkinSettingFilePath.AssertIs(expected);

        }
    }
}
