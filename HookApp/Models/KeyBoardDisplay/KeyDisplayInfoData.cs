using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HookApp.Models;
using YamlDotNet.Serialization;

namespace HookApp.Models.KeyBoardDisplay
{

    public class KeyDisplayInfoData
    {
        public List<IKeyDisplayInfo> KeyDisplayInfos { get; }

        /// <summary>
        /// 設定ファイルパスから、画像情報を初期化します。
        /// </summary>
        /// <param name="settingFilePath"></param>
        public KeyDisplayInfoData(string settingFilePath)
        {
            KeyDisplayInfos = LoadKeyDisplayInfos(settingFilePath);
        }

        /// <summary>
        /// 指定されたYAML設定ファイルから、キーの位置情報を取得します。
        /// </summary>
        /// <returns></returns>
        public List<IKeyDisplayInfo> LoadKeyDisplayInfos(string settingFilePath)
        {
            //YAMLデシリアライズ
            SettingFileDeserializer deserializer = new SettingFileDeserializer(settingFilePath);
            List<LoadedPicSettings> data = deserializer.YAMLDeserialize<List<LoadedPicSettings>>();

            //デシリアライズ結果のバリデーション

            //データのバリデーション
            //キー種類・キー数・

            List<IKeyDisplayInfo> ret = new List<IKeyDisplayInfo>();

            foreach (var item in data)
            {
                //TODO 読み込んだデータの変換・情報取得
                //VirtualKeyCode,ファイル名,画像サイズ(w,h)
                ret.Add(new SingleKeyDisplayInfoData(KeyboardUtilConstants.VirtualKeyCode.BackSpace,
                                                    "",
                                                    0D,
                                                    0D,
                                                    item.keyPosInfo.PosLeft,
                                                    item.keyPosInfo.PosTop));
            }

            return ret;

        }

    }

    /// <summary>
    /// Appで実際に使用するキー位置情報を表します。
    /// </summary>
    public class SingleKeyDisplayInfoData : IKeyDisplayInfo
    {
        public KeyboardUtilConstants.VirtualKeyCode Key { get; }
        public string PicUri { get; }
        public double Height { get; }
        public double Width { get; }
        public double Top { get; }
        public double Left { get; }

        public SingleKeyDisplayInfoData(KeyboardUtilConstants.VirtualKeyCode key, string picUri, double width, double height, double left, double top)
        {
            Key = key;
            PicUri = picUri;
            Height = height;
            Width = width;
            Top = top;
            Left = left;
        }

    }

    /// <summary>
    /// YAMLからロードされた情報を保持します
    /// </summary>
    public class LoadedPicSettings
    {
        [YamlMember(Alias = "KeyName")]
        public string KeyName { get; set; }

        [YamlMember(Alias = "KeyPicName")]
        public string KeyPicName { get; set; }

        [YamlMember(Alias = "KeyPos")]
        public KeyPos keyPosInfo { get; set; }

        public class KeyPos
        {
            public double PosLeft { get; set; }
            public double PosTop { get; set; }

            public KeyPos() { }

            public KeyPos(double posLeft,double posTop)
            {
                PosLeft = posLeft;
                PosTop = posTop;

            }
        }
    }


}
