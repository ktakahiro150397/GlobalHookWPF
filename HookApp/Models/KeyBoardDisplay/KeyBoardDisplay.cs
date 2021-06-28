using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using YamlDotNet.Serialization;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;

namespace HookApp.Models.KeyBoardDisplay
{
    /// <summary>
    /// キーボード表示についてのロジックを持ちます。
    /// </summary>
    public class KeyBoardDisplay 
    {
        
        public Uri KeyboardBasePicUri { get; set; }
        public ObservableCollection<KeyDisplayInfo> KeyDisplayInfoCollection { get; set; }

        /// <summary>
        /// 設定ファイルのファイルパスを指定し、キー表示情報を初期します。
        /// </summary>
        /// <param name="settingFilePath"></param>
        public KeyBoardDisplay(string settingFilePath,double baseKeyboardPicScale)
        {

            KeyDisplayInfoCollection = new ObservableCollection<KeyDisplayInfo>();

            KeyDisplayInfoData data = new KeyDisplayInfoData(settingFilePath);

            foreach(IKeyDisplayInfo item in data.KeyDisplayInfos)
            {
                KeyDisplayInfoCollection.Add(new KeyDisplayInfo(item, baseKeyboardPicScale));
            }

        }

        /// <summary>
        /// YAMLからロードされた画像情報を保持します
        /// </summary>
        public class LoadedPicSettings
        {
            [YamlMember(Alias = "KeyName")]
            public string KeyName { get; set; }

            [YamlMember(Alias = "KeyPic")]
            public KeyPic KeyPics { get; set; }

            [YamlMember(Alias = "KeyPos")]
            public KeyPos keyPoses { get; set; }
        }

        public class KeyPic
        {
            public string PicName { get; set; }
            public double PicWidth { get; set; }
            public double PicHeight { get; set; }
        }

        public class KeyPos
        {
            public double PosLeft { get; set; }
            public double PosTop { get; set; }
        }

        public class KeyDisplayInfo : INotifyPropertyChanged
        {
            private double _height;
            private double _width;
            private double _keyPicScalecoefficient; 

            /// <summary>
            /// プロパティ変更をUI側へ通知するイベント
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            public KeyboardUtilConstants.VirtualKeyCode Key { get; }

            public string PicUri { get; }
            public double Height { get
                {
                    return _height * _keyPicScalecoefficient;
                }

            }
            public double Width { get
                {
                    return _width * _keyPicScalecoefficient;
                }
            }
            public double Top { get; }
            public double Left { get; }

            private Visibility _visible;

            /// <summary>
            /// キーの表示を取得、または変更して通知します。
            /// </summary>
            public Visibility Visible
            {
                get
                {
                    return _visible;
                }
                set
                {
                    _visible = value;
                    OnPropertyChanged(nameof(Visible));
                }
            }

            /// <summary>
            /// キーの画像情報から、表示情報を初期化します。
            /// </summary>
            /// <param name="initializeData"></param>
            public KeyDisplayInfo(IKeyDisplayInfo initializeData,double keyPicScale)
            {
                Key = initializeData.Key;
                PicUri = initializeData.PicUri;
                _height = initializeData.Height;
                _width = initializeData.Width;
                Top = initializeData.Top;
                Left = initializeData.Left;
                _keyPicScalecoefficient = keyPicScale;

                //初期状態
                _visible = Visibility.Hidden;

            }

            /// <summary>
            /// プロパティ変更の通知メソッド。
            /// </summary>
            /// <param name="info"></param>
            protected void OnPropertyChanged(string info)
            {
                //イベントはnullを許容
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
            }
        }

    }



}
