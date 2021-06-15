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

        /// <summary>
        /// すべてのキーが含まれているかどうかのチェック
        /// </summary>
        [TestMethod]
        public void NoExistKeyCodeList_Correct()
        {

            var testsettings = new List<LoadedPicSettings>() {
                new LoadedPicSettings()
                {
                    KeyName = "Escape",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F1",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                }
            };

            //実際の処理
            SettingFileValidator actualValidator = new SettingFileValidator(testsettings);
            SettingFileValidatorResult actualResult = actualValidator.GetValidationResult();

            //期待する出力 : 存在しないキーなし
            SettingFileValidatorResult expectedResult = new SettingFileValidatorResult();

            //比較
            actualResult.NoExistKeyCodeList.AssertIs(expectedResult.NoExistKeyCodeList);

        }

        /// <summary>
        /// すべてのキーが含まれないチェック
        /// </summary>
        [TestMethod]
        public void NoExistKeyCodeList_NoExists()
        {

            var testsettings = new List<LoadedPicSettings>() {
                new LoadedPicSettings()
                {
                    KeyName = "Escape",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                }
            };

            //実際の処理
            SettingFileValidator actualValidator = new SettingFileValidator(testsettings);
            SettingFileValidatorResult actualResult = actualValidator.GetValidationResult();

            //期待する出力
            //F1が存在しない
            SettingFileValidatorResult expectedResult = new SettingFileValidatorResult();
            expectedResult.NoExistKeyCodeList.AddRange(new List<KeyboardUtilConstants.VirtualKeyCode>() { KeyboardUtilConstants.VirtualKeyCode.F1 });

            //比較
            actualResult.NoExistKeyCodeList.AssertIs(expectedResult.NoExistKeyCodeList);

        }


        /// <summary>
        /// 設定ファイル中の重複キーチェック
        /// </summary>
        [TestMethod]
        public void NoExistKeyCodeList_DuplicateKeyList()
        {
            var testsettings = new List<LoadedPicSettings>() {
                new LoadedPicSettings()
                {
                    KeyName = "Escape",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F1",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F1",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                },
                new LoadedPicSettings()
                {
                    KeyName = "F2",
                    KeyPicName = "NormalKey.png",
                    keyPosInfo = new LoadedPicSettings.KeyPos(99D, 999D),
                }
            };

            //実際の処理
            SettingFileValidator actualValidator = new SettingFileValidator(testsettings);
            SettingFileValidatorResult actualResult = actualValidator.GetValidationResult();

            //期待する出力
            //F1が存在しない
            SettingFileValidatorResult expectedResult = new SettingFileValidatorResult();
            expectedResult.DuplicateKeyList.AddRange(new List<SettingFileValidatorResult.SettingFileValidatorDuplicateResult>() { 
                new SettingFileValidatorResult.SettingFileValidatorDuplicateResult(KeyboardUtilConstants.VirtualKeyCode.F1,2),
                new SettingFileValidatorResult.SettingFileValidatorDuplicateResult(KeyboardUtilConstants.VirtualKeyCode.F2,3)
            });

            //比較
            actualResult.NoExistKeyCodeList.AssertIs(expectedResult.NoExistKeyCodeList);

        }

    }
}
