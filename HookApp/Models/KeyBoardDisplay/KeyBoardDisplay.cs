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
    class KeyBoardDisplay 
    {
        

        public ObservableCollection<KeyDisplayInfo> KeyDisplayInfoCollection { get; set; }

        /// <summary>
        /// 設定ファイルのファイルパスを指定し、キー表示情報を初期します。
        /// </summary>
        /// <param name="settingFilePath"></param>
        public KeyBoardDisplay(string settingFilePath)
        {

            KeyDisplayInfoCollection = new ObservableCollection<KeyDisplayInfo>();

            KeyDisplayInfoData data = new KeyDisplayInfoData(settingFilePath);

            foreach(IKeyDisplayInfo item in data.KeyDisplayInfos)
            {
                KeyDisplayInfoCollection.Add(new KeyDisplayInfo(item));
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

            /// <summary>
            /// プロパティ変更をUI側へ通知するイベント
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            public KeyboardUtilConstants.VirtualKeyCode Key { get; }

            public string PicUri { get; }
            public double Height { get; }
            public double Width { get; }
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


            [Obsolete("IKeyDisplayInfoから初期化するようにする")]
            public KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode key, string picUri, double width, double height, double left, double top, Visibility visible)
            {
                Key = key;
                PicUri = picUri;
                Height = height;
                Width = width;
                Top = top;
                Left = left;
                _visible = visible;
            }

            /// <summary>
            /// キーの画像情報から、表示情報を初期化します。
            /// </summary>
            /// <param name="initializeData"></param>
            public KeyDisplayInfo(IKeyDisplayInfo initializeData)
            {
                Key = initializeData.Key;
                PicUri = initializeData.PicUri;
                Height = initializeData.Height;
                Width = initializeData.Width;
                Top = initializeData.Top;
                Left = initializeData.Left;

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
