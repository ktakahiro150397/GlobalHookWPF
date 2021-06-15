using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using HookApp.Models;
using YamlDotNet.Serialization;
using System.Drawing;

namespace HookApp.Models.KeyBoardDisplay
{

    public class KeyDisplayInfoData
    {
        private List<LoadedPicSettings> loadedPicSettings { get; set; }
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
            loadedPicSettings = deserializer.YAMLDeserialize<List<LoadedPicSettings>>();

            //デシリアライズ結果のバリデーション
            SettingFileValidator validator = new SettingFileValidator(loadedPicSettings);



            //データのバリデーション
            //キー種類・キー数・

            List<IKeyDisplayInfo> ret = new List<IKeyDisplayInfo>();

            foreach (var item in loadedPicSettings)
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

        /// <summary>
        /// 読み込んだ設定ファイルから、キー位置情報を初期化します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="basePath"></param>
        /// <param name="settings"></param>
        public SingleKeyDisplayInfoData(string basePath, LoadedPicSettings settings)
        {

            ActualPicFileData picFileData = GetPicFileData(basePath, settings);


            if (!KeyboardUtilConstants.keyNameKeyCodeDictionary.ContainsKey(settings.KeyName))
            {
                throw new ApplicationException($"設定ファイル読み込み：KeyName「{settings.KeyName}」は存在しません。");
            }

            Key = KeyboardUtilConstants.keyNameKeyCodeDictionary[settings.KeyName];
            PicUri = picFileData.PicUri.LocalPath;
            Height = picFileData.Height;
            Width = picFileData.Width;
            Top = settings.keyPosInfo.PosTop;
            Left = settings.keyPosInfo.PosLeft;
        }

        /// <summary>
        /// 設定ファイルに指定された画像ファイルの情報を取得します。
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private ActualPicFileData GetPicFileData(string basePath, LoadedPicSettings settings)
        {


            //画像ファイルのパス
            string PicPath = Path.Combine(Path.GetFullPath(basePath), settings.KeyPicName);

            //ファイルの存在チェック
            if (!File.Exists(PicPath))
            {
                throw new ApplicationException($"設定ファイル読み込み：指定された画像ファイル「{PicPath}」が存在しませんでした。");
            }

            //画像ファイルのImageオブジェクト取得
            Image image = Image.FromFile(PicPath);
            return new ActualPicFileData(image, new Uri(PicPath));
        }

        private class ActualPicFileData
        {
            public Image PicImage { get; private set; }
            public Uri PicUri { get; private set; }

            public double Width
            {
                get
                {
                    return PicImage.Width;
                }
            }
            public double Height
            {
                get
                {
                    return PicImage.Height;
                }
            }

            public ActualPicFileData(Image image, Uri picUri)
            {
                PicImage = image;
                PicUri = picUri;
            }
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

            public KeyPos(double posLeft, double posTop)
            {
                PosLeft = posLeft;
                PosTop = posTop;

            }
        }


    }


}
