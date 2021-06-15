using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HookApp.Models.KeyBoardDisplay;
using Inasync;

namespace HookAppUnitTest.Models
{
    [TestClass]
    public class KeyDisplayInfoTest : TestBase
    {

        [TestMethod]
        public void GetPicDataTest()
        {
            var basePath = @"C:\Users\Takahiro\source\repos\ktakahiro150397\GlobalHookWPF\HookAppUnitTest\bin\Debug\netcoreapp3.1\Resources\Default";

            //この設定を読み込んだものとする
            var settingsEsc = new LoadedPicSettings()
            {
                KeyName = "Escape",
                KeyPicName = "Esc.png",
                keyPosInfo = new LoadedPicSettings.KeyPos(3D, 3D),
            };

            var testDataEsc = new SingleKeyDisplayInfoData(basePath, settingsEsc);

            var ansDataEsc = new SingleKeyDisplayInfoData(
                KeyboardUtilConstants.VirtualKeyCode.Escape,
                System.IO.Path.Combine(basePath, settingsEsc.KeyPicName),
                32D,
                16D,
                3D,
                3D);

            //この設定を読み込んだものとする
            var settingsF1 = new LoadedPicSettings()
            {
                KeyName = "F2",
                KeyPicName = "NormalKey.png",
                keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
            };

            var testDataF1 = new SingleKeyDisplayInfoData(basePath, settingsF1);

            var ansDataF1 = new SingleKeyDisplayInfoData(
                KeyboardUtilConstants.VirtualKeyCode.F2,
                System.IO.Path.Combine(basePath, settingsF1.KeyPicName),
                32D,
                32D,
                99D,
                999D);


            testDataEsc.AssertIs(ansDataEsc);
            testDataF1.AssertIs(ansDataF1);

        }


    }
}
