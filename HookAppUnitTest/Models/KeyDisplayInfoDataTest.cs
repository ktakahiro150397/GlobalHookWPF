using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HookApp.Models.KeyBoardDisplay;
using System.IO;
using Inasync;
using System;

namespace HookAppUnitTest.Models
{

    [TestClass]
    public class KeyDisplayInfoDataTest
    {

        /// <summary>
        /// データ読み込みテスト
        /// </summary>
        [TestMethod]
        public void LoadKeyDisplayInfos_Correct()
        {
            var yamlFilePath = ".\\Resources\\yamlReadTest\\KeyDisplaySetting.yaml";

            //実際の処理
            KeyDisplayInfoData data = new KeyDisplayInfoData(yamlFilePath);

            //期待する出力
            List<SingleKeyDisplayInfoData> expected = new List<SingleKeyDisplayInfoData>();
            expected.Add(new SingleKeyDisplayInfoData(
                KeyboardUtilConstants.VirtualKeyCode.Escape,
               Path.GetFullPath(".\\Resources\\yamlReadTest\\Esc.png"),
                32D,
                16D,
                1,
                2));
            expected.Add(new SingleKeyDisplayInfoData(
                KeyboardUtilConstants.VirtualKeyCode.F1,
                Path.GetFullPath(".\\Resources\\yamlReadTest\\Esc.png"),
                32D,
                16D,
                3,
                4));
            expected.Add(new SingleKeyDisplayInfoData(
                KeyboardUtilConstants.VirtualKeyCode.F2,
                Path.GetFullPath(".\\Resources\\yamlReadTest\\Esc.png"),
                32D,
                16D,
                5,
                6));

            data.KeyDisplayInfos.AssertIs(expected);


        }





    }
}
