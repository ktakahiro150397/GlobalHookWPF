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
        /// 正しい形式の設定ファイル読み込みテスト
        /// </summary>
        [TestMethod]
        public void YAMLDeserializeTest_Correct()
        {

            var yamlFilePath = ".\\Resources\\yamlReadTest\\KeyDisplaySetting.yaml";
            var baseFilePath = ".\\Resources\\yamlReadTest";

            //YAMLデシリアライズ
            SettingFileDeserializer deserializer = new SettingFileDeserializer(yamlFilePath);
            List<LoadedPicSettings> infoData = deserializer.YAMLDeserialize<List<LoadedPicSettings>>();


            List<IKeyDisplayInfo> infoDataInterface = new List<IKeyDisplayInfo>();

            //期待する読み込みデータ
            List<IKeyDisplayInfo> ansData = new List<IKeyDisplayInfo>();
            ansData.Add(new SingleKeyDisplayInfoData(KeyboardUtilConstants.VirtualKeyCode.Escape,
                                                    Path.Combine(baseFilePath,"Esc.png"),
                                                    32D,
                                                    16D,
                                                    3D,
                                                    3D));
            ansData.Add(new SingleKeyDisplayInfoData(KeyboardUtilConstants.VirtualKeyCode.F1,
                                                   Path.Combine(baseFilePath, "F1.png"),
                                                   32D,
                                                   16D,
                                                   3D,
                                                   3D));
            ansData.Add(new SingleKeyDisplayInfoData(KeyboardUtilConstants.VirtualKeyCode.F2,
                                                   Path.Combine(baseFilePath, "F2.png"),
                                                   32D,
                                                   16D,
                                                   3D,
                                                   3D));


            ansData.AssertIs(infoDataInterface);
        }

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
