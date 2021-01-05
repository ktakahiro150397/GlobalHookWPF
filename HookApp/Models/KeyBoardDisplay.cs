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

namespace HookApp.Models
{
    /// <summary>
    /// キーボード表示についてのロジックを持ちます。
    /// </summary>
    class KeyBoardDisplay 
    {
        

        public ObservableCollection<KeyDisplayInfo> KeyDisplayInfoCollection { get; set; }

        public KeyBoardDisplay()
        {
            KeyDisplayInfoCollection = new ObservableCollection<KeyDisplayInfo>();

            //_KeyDisplayInfoCollectionを初期化する
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Escape, "../Resources/Esc.png", 32D, 16D, 3D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F1, "../Resources/Esc.png", 32D, 16D, 73D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F2, "../Resources/Esc.png", 32D, 16D, 108D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F3, "../Resources/Esc.png", 32D, 16D, 143D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F4, "../Resources/Esc.png", 32D, 16D, 178D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F5, "../Resources/Esc.png", 32D, 16D, 230D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F6, "../Resources/Esc.png", 32D, 16D, 265D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F7, "../Resources/Esc.png", 32D, 16D, 300D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F8, "../Resources/Esc.png", 32D, 16D, 335D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F9, "../Resources/Esc.png", 32D, 16D, 388D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F10, "../Resources/Esc.png", 32D, 16D, 423D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F11, "../Resources/Esc.png", 32D, 16D, 458D, 3D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F12, "../Resources/Esc.png", 32D, 16D, 493D, 3D, Visibility.Hidden));
            //KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.FullToHalf, "../Resources/NormalKey.png", 32D, 32D, 3D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.HalfToFull, "../Resources/NormalKey.png", 32D, 32D, 3D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.One, "../Resources/NormalKey.png", 32D, 32D, 38D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Two, "../Resources/NormalKey.png", 32D, 32D, 73D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Three, "../Resources/NormalKey.png", 32D, 32D, 108D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Four, "../Resources/NormalKey.png", 32D, 32D, 143D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Five, "../Resources/NormalKey.png", 32D, 32D, 178D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Six, "../Resources/NormalKey.png", 32D, 32D, 213D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Seven, "../Resources/NormalKey.png", 32D, 32D, 248D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Eight, "../Resources/NormalKey.png", 32D, 32D, 283D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Nine, "../Resources/NormalKey.png", 32D, 32D, 318D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Zero, "../Resources/NormalKey.png", 32D, 32D, 353D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Hyphen, "../Resources/NormalKey.png", 32D, 32D, 388D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Caret, "../Resources/NormalKey.png", 32D, 32D, 423D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Yen, "../Resources/NormalKey.png", 32D, 32D, 458D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.BackSpace, "../Resources/NormalKey.png", 32D, 32D, 493D, 22D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Tab, "../Resources/Tab.png", 57D, 32D, 3D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Q, "../Resources/NormalKey.png", 32D, 32D, 63D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.W, "../Resources/NormalKey.png", 32D, 32D, 98D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.E, "../Resources/NormalKey.png", 32D, 32D, 133D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.R, "../Resources/NormalKey.png", 32D, 32D, 168D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.T, "../Resources/NormalKey.png", 32D, 32D, 203D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Y, "../Resources/NormalKey.png", 32D, 32D, 238D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.U, "../Resources/NormalKey.png", 32D, 32D, 273D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.I, "../Resources/NormalKey.png", 32D, 32D, 308D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.O, "../Resources/NormalKey.png", 32D, 32D, 343D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.P, "../Resources/NormalKey.png", 32D, 32D, 378D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.AMark, "../Resources/NormalKey.png", 32D, 32D, 413D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.LeftBlacket, "../Resources/NormalKey.png", 32D, 32D, 448D, 57D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.CapsLock, "../Resources/Caps.png", 65D, 32D, 3D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.A, "../Resources/NormalKey.png", 32D, 32D, 71D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.S, "../Resources/NormalKey.png", 32D, 32D, 106D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.D, "../Resources/NormalKey.png", 32D, 32D, 141D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.F, "../Resources/NormalKey.png", 32D, 32D, 176D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.G, "../Resources/NormalKey.png", 32D, 32D, 211D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.H, "../Resources/NormalKey.png", 32D, 32D, 246D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.J, "../Resources/NormalKey.png", 32D, 32D, 281D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.K, "../Resources/NormalKey.png", 32D, 32D, 316D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.L, "../Resources/NormalKey.png", 32D, 32D, 351D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.SemiColon, "../Resources/NormalKey.png", 32D, 32D, 386D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Colon, "../Resources/NormalKey.png", 32D, 32D, 421D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.RightBlacket, "../Resources/NormalKey.png", 32D, 32D, 456D, 92D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.LeftShift, "../Resources/LeftShiftSpace.png", 82D, 32D, 3D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Z, "../Resources/NormalKey.png", 32D, 32D, 88D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.X, "../Resources/NormalKey.png", 32D, 32D, 123D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.C, "../Resources/NormalKey.png", 32D, 32D, 158D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.V, "../Resources/NormalKey.png", 32D, 32D, 193D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.B, "../Resources/NormalKey.png", 32D, 32D, 228D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.N, "../Resources/NormalKey.png", 32D, 32D, 263D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.M, "../Resources/NormalKey.png", 32D, 32D, 298D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Comma, "../Resources/NormalKey.png", 32D, 32D, 333D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Period, "../Resources/NormalKey.png", 32D, 32D, 368D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Slash, "../Resources/NormalKey.png", 32D, 32D, 403D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.YenUnder, "../Resources/NormalKey.png", 32D, 32D, 438D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.RightShift, "../Resources/RightShift.png", 52D, 32D, 473D, 127D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.LeftControl, "../Resources/SystemKey.png", 41D, 32D, 3D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.LeftWin, "../Resources/SystemKey.png", 41D, 32D, 47D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.LeftAlt, "../Resources/SystemKey.png", 41D, 32D, 91D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Muhenkan, "../Resources/SystemKey.png", 41D, 32D, 135D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Space, "../Resources/LeftShiftSpace.png", 82D, 32D, 179D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Henkan, "../Resources/SystemKey.png", 41D, 32D, 264D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.HiraganaKatakana, "../Resources/SystemKey.png", 41D, 32D, 308D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.RightAlt, "../Resources/SystemKey.png", 41D, 32D, 352D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.RightWin, "../Resources/SystemKey.png", 41D, 32D, 396D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.AppKey, "../Resources/SystemKey.png", 41D, 32D, 440D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.RightControl, "../Resources/SystemKey.png", 41D, 32D, 484D, 162D, Visibility.Hidden));
            KeyDisplayInfoCollection.Add(new KeyDisplayInfo(KeyboardUtilConstants.VirtualKeyCode.Return, "../Resources/Enter.png", 42D, 67D, 483D, 57D, Visibility.Hidden));


            //YAMLロードテスト
            //var loadedPic = Deserialize(@"C:\Users\koike.LOGIC\Source\Repos\HookApp2\HookApp\bin\Debug\netcoreapp3.1\Resources\KeyDisplaySetting.yaml");
            var loadedPic = Deserialize(@"C:\Users\Takahiro\source\repos\HookApp\HookApp\bin\Debug\netcoreapp3.1\Resources\KeyDisplaySetting.yaml");


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

        /// <summary>
        /// 指定したYAML形式の画像設定ファイルをデシリアライズします。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private List<LoadedPicSettings> Deserialize(string filePath)
        {
            //ファイルを開き、テキストとして取得
            var fileSr = new StreamReader(filePath);
            string text = fileSr.ReadToEnd();

            //デシリアライザを構成し、結果を返却
            var deserializer = new DeserializerBuilder().Build();

            return deserializer.Deserialize<List<LoadedPicSettings>>(text);
        }

        public class KeyDisplayInfo : INotifyPropertyChanged
        {

            /// <summary>
            /// プロパティ変更をUI側へ通知するイベント
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            private Visibility _visible;

            public string PicUri { get; }
            public double Height { get; }
            public double Width { get; }
            public double Top { get; }
            public double Left { get; }

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

            public KeyboardUtilConstants.VirtualKeyCode Key { get; }

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
