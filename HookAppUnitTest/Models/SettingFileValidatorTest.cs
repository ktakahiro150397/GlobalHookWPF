using Microsoft.VisualStudio.TestTools.UnitTesting;
using HookApp;
using HookApp.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Inasync;
using HookApp.Models.KeyBoardDisplay;

namespace HookAppUnitTest.Models
{
    [TestClass]
    public class SettingFileValidatorTest : TestBase
    {

        [TestMethod]
        public void AllKeyContainsTest()
        {

            var testsettings = new List<LoadedPicSettings>() {
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                }
            };

            //実際の処理
            SettingFileValidator actualValidator = new SettingFileValidator(testsettings);
            SettingFileValidatorResult actualResult = actualValidator.GetKeyContainsValidationResult();

            //期待する出力
            //対応ディクショナリとの差集合リストを取得する
            var expectedList = KeyboardUtilConstants.keyNameKeyCodeDictionary.Values.Except(testsettings.Select((item) => KeyboardUtilConstants.keyNameKeyCodeDictionary[item.KeyName])).ToList();
            SettingFileValidatorResult expectedResult = new SettingFileValidatorResult(expectedList);

            //比較
            expectedResult.AssertIs(actualResult);

        }





    }
}
