using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HookApp.Models.KeyBoardDisplay;
using System.IO;
using Inasync;

namespace HookAppUnitTest.Models
{

    [TestClass]
    public class SettingFileDeserializerTest : TestBase
    {
        

        /// <summary>
        /// フォーマット違い例外テスト
        /// </summary>
        [TestMethod]
        public void YAMLDeserializeTest_Format_Invalid()
        {

            var yamlFilePath = ".\\Resources\\yamlFormatInvalid\\KeyDisplaySetting.yaml";
            Assert.ThrowsException<YamlDotNet.Core.YamlException>(() => { new KeyDisplayInfoData(yamlFilePath); });

        }


    }

}
