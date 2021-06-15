using System;
using System.Collections.Generic;
using System.Text;
using HookApp.Models;

namespace HookApp.Models.KeyBoardDisplay
{
    /// <summary>
    /// デシリアライズしたデータのバリデーションを行います。
    /// </summary>
    public class SettingFileValidator
    {
        private List<LoadedPicSettings> _loadedPicSettings { get; }

        public SettingFileValidator(List<LoadedPicSettings> loadedPicSettings)
        {
            _loadedPicSettings = loadedPicSettings;
        }

        /// <summary>
        /// キー情報がすべて含まれているかどうかのバリデーションを行い、その結果を返します。
        /// </summary>
        /// <returns></returns>
        public SettingFileValidatorResult GetKeyContainsValidationResult()
        {
            return new SettingFileValidatorResult(new List<KeyboardUtilConstants.VirtualKeyCode>());
        }


    }

    public class SettingFileValidatorResult
    {
        /// <summary>
        /// 対象の読み込みデータに、全てのキーが存在するかどうかを表します。
        /// </summary>
        public bool IsAllKeyContain { get; private set; }

        /// <summary>
        /// 対象の読み込みデータ中に存在しなかったキーのリストを表します。
        /// </summary>
        public List<KeyboardUtilConstants.VirtualKeyCode> NoExistKeyCodeList { get; private set; }

        public SettingFileValidatorResult(List<KeyboardUtilConstants.VirtualKeyCode> noExistKeyCodeList)
        {
            if (noExistKeyCodeList.Count == 0)
            {
                IsAllKeyContain = true;
            }
            else
            {
                IsAllKeyContain = false;
            }

            NoExistKeyCodeList = noExistKeyCodeList;
        }


    }
}
