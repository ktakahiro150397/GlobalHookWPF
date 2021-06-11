using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using HookApp.Models;
using HookApp.Lib;
using System.Windows.Input;
using System.Windows.Controls;

namespace HookApp.ViewModels
{
    /// <summary>
    /// MainWindowのViewModel
    /// </summary>
    class MainWindowViewModel : INotifyPropertyChanged
    {

        #region "privateフィールド"

        /// <summary>
        /// 画面に表示するキー入力履歴文字列。
        /// </summary>
        private string _inputHistory;

        #endregion

        #region "IPropertyChanged"

        /// <summary>
        /// プロパティ変更をUI側へ通知するイベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティ変更の通知メソッド。
        /// </summary>
        /// <param name="info"></param>
        protected void OnPropertyChanged(string info)
        {
            //イベントはnullを許容

            if(info == "ElapsedTimeString")
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ElapsedTime"));
            }
            else
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        #region "PropertyChangedProxy"

        //Modelの変更を型安全にViewに通知する便利クラス
        private PropertyChangedProxy<Models.KeyInputStatistics, string> _keyInputPropertyChanged;
        private PropertyChangedProxy<Models.KeyInputStatistics, int> _keyInputPropertyChangedKeyDownSum;
        private PropertyChangedProxy<Models.KeyInputStatistics, double> _keyInputPropertyChangedCurrentKPM;
        private PropertyChangedProxy<Models.KeyInputStatistics, double> _keyInputPropertyChangedMaxKPM;



        #endregion

        #region "通知プロパティ"

        /// <summary>
        /// キー入力の履歴文字列プロパティ。変更時、Viewへ通知されます。
        /// </summary>
        public string inputHistory
        {
            get
            {
                return this._inputHistory;
            }
            set
            {
                this._inputHistory = value;
                this.OnPropertyChanged(nameof(inputHistory));
            }
        }

        /// <summary>
        /// 現在のKPM値
        /// </summary>
        public double CurrentKPM
        {
            get
            {
                return this.KeyInputStatistics.CurrentKPM;
            }
            set
            {
                this.KeyInputStatistics.CurrentKPM = value;
                this.OnPropertyChanged(nameof(CurrentKPM));
            }
        }

        /// <summary>
        /// 最高KPM値
        /// </summary>
        public double MaxKPM
        {
            get
            {
                return this.KeyInputStatistics.MaxKPM;
            }
            set
            {
                this.KeyInputStatistics.MaxKPM = value;
                this.OnPropertyChanged(nameof(MaxKPM));
            }
        }

        /// <summary>
        /// キー合計打鍵数
        /// </summary>
        public int KeyDownSum
        {
            get
            {
                return this.KeyInputStatistics.KeyDownSum;
            }
            set
            {
                this.KeyInputStatistics.KeyDownSum = value;
                this.OnPropertyChanged(nameof(KeyDownSum));
            }
        }

        /// <summary>
        /// アプリケーションが開始した時刻。
        /// </summary>
        public DateTime StartUpTime { get; }

        /// <summary>
        /// アプリケーションが開始してから経過した時間。
        /// </summary>
        public string ElapsedTime
        {
            get
            {
                //フォーマットして返却する
                return this.KeyInputStatistics.ElapsedTimeString;
            }
        }


        public ObservableCollection<KeyBoardDisplay.KeyDisplayInfo> KeyDisplayInfoCollection { get; set; }

        #endregion

        #region "非通知プロパティ"

        /// <summary>
        /// タイトル文字列プロパティ。
        /// </summary>
        public string TitleString { get; set; }

        /// <summary>
        /// Shiftキーが現在押下されているかどうかを表します。
        /// </summary>
        [Obsolete("KeyboardDisplayから取得するように変更する")]
        private bool IsShiftPressed { get; set; }

        /// <summary>
        /// キー入力取得時、この文字にセパレータを挿入するかどうかのフラグ。
        /// </summary>
        public bool IsInsertSeparatorSymbol;

        #endregion

        #region "コマンドプロパティ"

            public ICommand ClearInput { get; private set; }

        #endregion

        #region "Modelsインスタンス"

        /// <summary>
        /// キーボード入力取得を行うクラスインスタンス。
        /// </summary>
        private Models.KeyboardUtil KeyboardUtil { get; }

        /// <summary>
        /// キーボード表示を行うクラスインスタンス。
        /// </summary>
        private Models.KeyBoardDisplay KeyboardDisplay { get; }

        private Models.KeyInputStatistics KeyInputStatistics { get; }

        #endregion

        /// <summary>
        /// MainWindowのViewModelを初期化します。
        /// </summary>
        public MainWindowViewModel()
        {
            //キーボード入力取得クラスのインスタンスを生成・イベントハンドラーの設定
            this.KeyboardUtil = new Models.KeyboardUtil(this);
            this.KeyboardUtil.KeyHookKeyUp += this.KeyHookKeyUp_Handler;
            this.KeyboardUtil.KeyHookKeyDown += this.KeyHookKeyDown_Handler;
            this.KeyboardUtil.KeyHookShiftKeyUp += this.KeyHookShiftKeyUp_Handler;
            this.KeyboardUtil.KeyHookShiftKeyDown += this.KeyHookShiftKeyDown_Handler;
            this.KeyboardUtil.KeyHookAltKeyUp += this.KeyHookAltKeyUp_Handler;
            this.KeyboardUtil.KeyHookAltKeyDown += this.KeyHookAltKeyDown_Handler;

            //キーボード入力表示クラスのインスタンスを生成・プロパティの割当
            this.KeyboardDisplay = new KeyBoardDisplay();
            KeyDisplayInfoCollection = KeyboardDisplay.KeyDisplayInfoCollection;

            //キー入力統計情報
            KeyInputStatistics = new KeyInputStatistics();
            this.KeyboardUtil.KeyHookKeyDown += KeyInputStatistics.KeyDownCount;
            this.KeyboardUtil.KeyHookShiftKeyDown += KeyInputStatistics.KeyDownCount;
            this.KeyboardUtil.KeyHookAltKeyDown += KeyInputStatistics.KeyDownCount;
            var vm = this;
            _keyInputPropertyChanged = new PropertyChangedProxy<KeyInputStatistics, string>(
                KeyInputStatistics, 
                keyStatic => keyStatic.ElapsedTimeString, 
                elapsedTime => vm.OnPropertyChanged(nameof(elapsedTime))
            );
            _keyInputPropertyChangedKeyDownSum = new PropertyChangedProxy<KeyInputStatistics, int>(
                KeyInputStatistics,
                keyStatic => keyStatic.KeyDownSum,
                KeyDownSum => vm.OnPropertyChanged(nameof(KeyDownSum))
            );
            _keyInputPropertyChangedCurrentKPM = new PropertyChangedProxy<KeyInputStatistics, double>(
               KeyInputStatistics,
               keyStatic => keyStatic.CurrentKPM,
               CurrentKPM => vm.OnPropertyChanged(nameof(CurrentKPM))
           );
            _keyInputPropertyChangedMaxKPM = new PropertyChangedProxy<KeyInputStatistics, double>(
               KeyInputStatistics,
               keyStatic => keyStatic.MaxKPM,
               MaxKPM => vm.OnPropertyChanged(nameof(MaxKPM))
           );

            //最初の入力にセパレータは不要
            this.IsInsertSeparatorSymbol = false;

            //Shiftキー初期化
            this.IsShiftPressed = false;

            //バージョン情報を取得し、タイトルへ反映する
            this.TitleString = Models.General.GetTitleString();

            //コマンド割当
            ClearInput = new ClearInput_Imp(this);
        }

        

        /// <summary>
        /// キーアップイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookKeyUp_Handler(object sender, KeyboardUtil.HookKeyEventArgs e)
        {
            //このキーコードをアンプッシュ状態にする
            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //このキーのオーバーレイを非表示にする
                keyDisp.Visible = Visibility.Hidden;
            }
        }

        /// <summary>
        /// キーダウンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookKeyDown_Handler(object sender, KeyboardUtil.HookKeyEventArgs e)
        {       
            string inputChar = null;
            //入力文字を取得する
            if (this.IsShiftPressed)
            {
                try
                {

                    //シフトが押されている場合、大文字を取得
                    inputChar = KeyboardUtilConstants.bigKeyNameDictionary[e.vkCode];
                }
                catch(KeyNotFoundException keyEx)
                {
                    //割り当てる文字が存在しない
                }

            }
            else
            {
                try
                {
                    //シフトが押されていない場合、小文字を取得
                    inputChar = KeyboardUtilConstants.smallKeyNameDictionary[e.vkCode];
                }
                catch (KeyNotFoundException keyEx)
                {
                    //割り当てる文字が存在しない
                }
            }

            if (inputChar == null)
            {
                //入力文字が取得できなかった場合
                return;
            }

            //このキーコードをプッシュ状態にする
            
            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if(keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //特殊キー
                //全角/半角キー

                //ひらがな/かたかな

                //RightWin検証不可

                //Appキー検証不可

                

                //このキーのオーバーレイを表示する
               keyDisp.Visible = Visibility.Visible;
            }

            //テキストに入力を反映する
            switch (e.vkCode)
            {
                case KeyboardUtilConstants.VirtualKeyCode.Return:
                    //Enterなら改行する
                    this.inputHistory += Setting.Default.KEYHISTORY_RETURN_SYMBOL + Environment.NewLine;
                    //次の文頭にセパレータは不要
                    this.IsInsertSeparatorSymbol = false;
                    break;
                case KeyboardUtilConstants.VirtualKeyCode.Space:
                    //Spaceなら空白を挿入
                    if (this.IsInsertSeparatorSymbol)
                    {
                        this.inputHistory += Setting.Default.KEYHISTORY_SEPARATOR + Setting.Default.KEYHISTORY_SPACE_SYMBOL;
                    }
                    else
                    {
                        //次の入力文字からセパレータを挿入する
                        this.inputHistory += Setting.Default.KEYHISTORY_SPACE_SYMBOL;
                        this.IsInsertSeparatorSymbol = true;
                    }
                    break;
                default:
                    //キー入力を反映
                    if (this.IsInsertSeparatorSymbol)
                    {
                        //セパレータを挿入する
                        this.inputHistory += Setting.Default.KEYHISTORY_SEPARATOR + inputChar;
                    }
                    else
                    {
                        //次の入力文字からセパレータを挿入する
                        this.IsInsertSeparatorSymbol = true;
                        this.inputHistory += inputChar;
                    }
                    break;
            }
        }

        /// <summary>
        /// Shiftキーアップイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookShiftKeyUp_Handler(object sender, Models.KeyboardUtil.HookKeyEventArgs e)
        {
            this.IsShiftPressed = false;

            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //このキーのオーバーレイを非表示にする
                keyDisp.Visible = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Shiftキーダウンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookShiftKeyDown_Handler(object sender, Models.KeyboardUtil.HookKeyEventArgs e)
        {
            this.IsShiftPressed = true;

            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //このキーのオーバーレイを表示する
                keyDisp.Visible = Visibility.Visible;
            }
        }

        private void KeyHookAltKeyUp_Handler(object sender, Models.KeyboardUtil.HookKeyEventArgs e)
        {
            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //このキーのオーバーレイを表示する
                keyDisp.Visible = Visibility.Hidden;
            }

        }

        private void KeyHookAltKeyDown_Handler(object sender, Models.KeyboardUtil.HookKeyEventArgs e)
        {
            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
            {
                //キーコードが存在しない場合何もしない
            }
            else
            {
                //このキーのオーバーレイを表示する
                keyDisp.Visible = Visibility.Visible;
            }

        }

        /// <summary>
        /// テキストクリア処理コマンド
        /// </summary>
        public class ClearInput_Imp : ICommand
        {

            private MainWindowViewModel _vm;
            public ClearInput_Imp(MainWindowViewModel vm)
            {
                _vm = vm;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add
                {
                   
                }

                remove
                {
                    
                }
            }

            bool ICommand.CanExecute(object parameter)
            {
                return true;
            }

            void ICommand.Execute(object parameter)
            {
                _vm.inputHistory = "";
            }
        }

    }
}
