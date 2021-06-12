using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HookApp.Models.KeyBoardDisplay;
using System.IO;

namespace HookAppUnitTest.Models
{

    [TestClass]
    public class SettingFileDeserializerTest
    {
        /// <summary>
        /// Resources以下のフォルダ構成が取得できるかどうかテスト
        /// </summary>
        [TestMethod]
        public void YAMLDeserializeTest()
        {

            var yamlFilePath = ".\\Resources\\Default\\KeyDisplaySetting.yaml";
            var baseFilePath = ".\\Resources\\Default";

            //データ読み込み
            var infoData = new KeyDisplayInfoData(yamlFilePath);

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

            

            Assert.IsTrue(infoData.KeyDisplayInfos.SequenceEqual(ansData));
        }




    }

}
