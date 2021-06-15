using System;
using System.Collections.Generic;
using System.Text;
using HookApp.Models;
using System.Linq;

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
        /// 対象の設定ファイルのバリデーションを行い、その結果を返します。
        /// </summary>
        /// <returns></returns>
        public SettingFileValidatorResult GetValidationResult()
        {
            SettingFileValidatorResult ret = new SettingFileValidatorResult();

            ret = GetKeyContainsValidationResult(ret);
            ret = GetKeyDuplicateValidationResult(ret);

            return ret;
        }

        /// <summary>
        /// キー情報がすべて含まれているかどうかのバリデーションを行い、その結果を返します。
        /// </summary>
        /// <returns></returns>
        private SettingFileValidatorResult GetKeyContainsValidationResult(SettingFileValidatorResult result)
        {
            SettingFileValidatorResult ret = new SettingFileValidatorResult(result);

            //対応ディクショナリとの差集合リストを取得し、設定
            List<KeyboardUtilConstants.VirtualKeyCode> exceptKeys = 
                KeyboardUtilConstants.keyNameKeyCodeDictionary.Values.Except(_loadedPicSettings.Select((item) => KeyboardUtilConstants.keyNameKeyCodeDictionary[item.KeyName])).ToList();
            ret.NoExistKeyCodeList.AddRange(exceptKeys);

            return ret;
        }

        /// <summary>
        /// キー情報が重複しているかどうかのバリデーションを行い、その結果を返します。
        /// </summary>
        /// <returns></returns>
        private SettingFileValidatorResult GetKeyDuplicateValidationResult(SettingFileValidatorResult result)
        {
            SettingFileValidatorResult ret = new SettingFileValidatorResult(result);

            //キー名称の重複を検出
            var duplicates = _loadedPicSettings
                .GroupBy((item) => item.KeyName)
                .Where((item) => item.Count() > 1)
                .Select((item) => new { Key = item.Key, Count = item.Count() })
                .ToList();

            //キー重複情報の設定
            foreach(var item in duplicates)
            {
                ret.DuplicateKeyList.Add(new SettingFileValidatorResult.SettingFileValidatorDuplicateResult(
                    KeyboardUtilConstants.keyNameKeyCodeDictionary[item.Key],
                    item.Count));
            }

            return ret;
        }


    }

    /// <summary>
    /// 設定ファイルのバリデーション結果を返します。
    /// </summary>
    public class SettingFileValidatorResult
    {
        /// <summary>
        /// 対象の読み込みデータに、全てのキーが存在する場合True。
        /// </summary>
        public bool IsAllKeyContain
        {
            get { return NoExistKeyCodeList.Count == 0; }
        }

        /// <summary>
        /// 対象の読み込みデータに、キー重複が存在しない場合True。
        /// </summary>
        public bool IsNoKeyDuplicate
        {
            get { return DuplicateKeyList.Count == 0; }
        }

        /// <summary>
        /// 対象の読み込みデータ中に存在しなかったキーのリストを表します。
        /// </summary>
        public List<KeyboardUtilConstants.VirtualKeyCode> NoExistKeyCodeList { get; set; }

        /// <summary>
        /// 対象の読み込みデータ中の重複しているキーのデータリストを表します。
        /// </summary>
        public List<SettingFileValidatorDuplicateResult> DuplicateKeyList { get; set; }

        public SettingFileValidatorResult()
        {
            NoExistKeyCodeList = new List<KeyboardUtilConstants.VirtualKeyCode>();
            DuplicateKeyList = new List<SettingFileValidatorDuplicateResult>();
        }

        public SettingFileValidatorResult(SettingFileValidatorResult result)
        {
            NoExistKeyCodeList = result.NoExistKeyCodeList;
            DuplicateKeyList = result.DuplicateKeyList;
        }

        public SettingFileValidatorResult(List<KeyboardUtilConstants.VirtualKeyCode> noExistKeyCodeList, List<SettingFileValidatorDuplicateResult> duplicateKeyList)
        {
            NoExistKeyCodeList = noExistKeyCodeList;
            DuplicateKeyList = duplicateKeyList;
        }


        public class SettingFileValidatorDuplicateResult
        {
            public KeyboardUtilConstants.VirtualKeyCode DuplicateKey { get; }
            public int DuplicateKeyCount { get; }

            public SettingFileValidatorDuplicateResult(KeyboardUtilConstants.VirtualKeyCode duplicateKey, int duplicateCount)
            {
                DuplicateKey = duplicateKey;
                DuplicateKeyCount = duplicateCount;
            }
        }
    }



}
