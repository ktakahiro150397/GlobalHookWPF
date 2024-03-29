﻿using System;
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
using HookApp.ViewModels.Commands.Common;
using HookApp.ViewModels.Commands.MainWindow;
using HookApp.Models.KeyBoardDisplay;

namespace HookApp.ViewModels
{
    /// <summary>
    /// MainWindowのViewModel
    /// </summary>
    internal class MainWindowViewModel : BaseViewModel
    {

        #region "privateフィールド"

        /// <summary>
        /// 画面に表示するキー入力履歴文字列。
        /// </summary>
        private string _inputHistory;

        #endregion

        #region "PropertyChangedProxy"

        //Modelの変更を型安全にViewに通知する便利クラス
        private PropertyChangedProxy<Models.KeyInputStatistics, string> _keyInputPropertyChanged;
        private PropertyChangedProxy<Models.KeyInputStatistics, int> _keyInputPropertyChangedKeyDownSum;
        private PropertyChangedProxy<Models.KeyInputStatistics, double> _keyInputPropertyChangedCurrentKPM;
        private PropertyChangedProxy<Models.KeyInputStatistics, double> _keyInputPropertyChangedMaxKPM;
        private PropertyChangedProxy<AppSettingsModel, string> _baseKeyBoardPicChanged;


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
                return "";
            }
        }


        public ObservableCollection<KeyBoardDisplay.KeyDisplayInfo> KeyDisplayInfoCollection { get; set; }

        /// <summary>
        /// キーボードのベース画像URIを取得します。
        /// </summary>
        public string SelectedSkinBaseKeyboardPicUriSource
        {
            get
            {
                return SettingsModel.SelectedSkinBaseKeyboardPicFilePath;
            }
            set
            {
                OnPropertyChanged(nameof(SelectedSkinBaseKeyboardPicUriSource));
            }
           
        }

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
        /// Ctrlキーが現在押下されているかどうかを表します。
        /// </summary>
        private bool IsCtrlPressed { get; set; }

        /// <summary>
        /// キー入力取得時、この文字にセパレータを挿入するかどうかのフラグ。
        /// </summary>
        public bool IsInsertSeparatorSymbol;

        #endregion

        #region "コマンドプロパティ"

        public ICommand ClearInput { get; private set; }

        public ICommand OpenOption { get; private set; }

        public ICommand WindowClose { get; private set; }

        #endregion

        #region "Modelsインスタンス"

        /// <summary>
        /// キーボード入力取得を行うクラスインスタンス。
        /// </summary>
        private Models.KeyboardUtil KeyboardUtil { get; }

        /// <summary>
        /// キーボード表示を行うクラスインスタンス。
        /// </summary>
        private KeyBoardDisplay KeyboardDisplay { get; }

        private KeyInputStatistics KeyInputStatistics { get; }

        /// <summary>
        /// Appの設定情報を持つクラスインスタンス。
        /// </summary>
        public AppSettingsModel SettingsModel { get; }

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

            //設定ファイル
            SettingsModel = new AppSettingsModel();

            //キーボード入力表示クラスのインスタンスを生成・プロパティの割当
            KeyboardDisplay = new KeyBoardDisplay(SettingsModel.SelectedSkinSettingFilePath, 1D); //設定ファイルのパスを与えて初期化
            KeyDisplayInfoCollection = KeyboardDisplay.KeyDisplayInfoCollection;

            //キー入力統計情報
            KeyInputStatistics = new KeyInputStatistics();
            this.KeyboardUtil.KeyHookKeyDown += KeyInputStatistics.KeyDownCount;
            this.KeyboardUtil.KeyHookShiftKeyDown += KeyInputStatistics.KeyDownCount;
            this.KeyboardUtil.KeyHookAltKeyDown += KeyInputStatistics.KeyDownCount;
            MainWindowViewModel vm = this;
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
            //_baseKeyBoardPicChanged = new PropertyChangedProxy<AppSettingsModel, string>(
            //    SettingsModel,
            //    setting => setting.SelectedSkinBaseKeyboardPicFilePath,
            //    SelectedSkinBaseKeyboardPicFilePath => vm.OnPropertyChanged(nameof(SelectedSkinBaseKeyboardPicFilePath))
            //);

            //最初の入力にセパレータは不要
            this.IsInsertSeparatorSymbol = false;

            //Shiftキー初期化
            this.IsShiftPressed = false;

            //バージョン情報を取得し、タイトルへ反映する
            this.TitleString = Models.General.GetTitleString();

            ClearInput = new ClearInput_Imp(this);
            OpenOption = new OpenOption(this);
            WindowClose = new MenuCloseButtonInput(this);


            SettingsModel.PropertyChanged += SettingsModel_PropertyChanged;
        }

        private void SettingsModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedSkinBaseKeyboardPicUriSource = SettingsModel.SelectedSkinBaseKeyboardPicFilePath;


        }

        /// <summary>
        /// キーアップイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookKeyUp_Handler(object sender, KeyboardUtil.HookKeyEventArgs e)
        {
            //Ctrlキーの押下チェック
            if (e.vkCode == KeyboardUtilConstants.VirtualKeyCode.LeftControl)
            {
                IsCtrlPressed = false;
            }

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
            //Ctrlキーの押下チェック
            if(e.vkCode == KeyboardUtilConstants.VirtualKeyCode.LeftControl && !IsShiftPressed)
            {
                IsCtrlPressed = true;
            }

            string inputChar = null;
            //入力文字を取得する
            if (this.IsShiftPressed)
            {

                if (KeyboardUtilConstants.bigKeyNameDictionary.ContainsKey(e.vkCode))
                {
                    //シフトが押されている場合、大文字を取得
                    inputChar = KeyboardUtilConstants.bigKeyNameDictionary.GetKeyString(e.vkCode);
                }
            }
            else
            {

                if (KeyboardUtilConstants.bigKeyNameDictionary.ContainsKey(e.vkCode))
                {
                    //シフトが押されている場合、大文字を取得
                    inputChar = KeyboardUtilConstants.smallKeyNameDictionary.GetKeyString(e.vkCode);
                }
            }

            if (inputChar == null)
            {
                //入力文字が取得できなかった場合
                return;
            }

            //このキーコードをプッシュ状態にする

            var keyDisp = KeyboardDisplay.KeyDisplayInfoCollection.Where(info => info.Key == e.vkCode).FirstOrDefault();
            if (keyDisp == null)
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


            //クリアショートカットのチェック
            if (e.vkCode == KeyboardUtilConstants.VirtualKeyCode.Escape)
            {
                //入力クリア
                inputHistory = "";
                return;
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
    }
}
