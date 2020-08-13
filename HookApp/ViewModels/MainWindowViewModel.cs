using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HookApp.Models;

namespace HookApp.ViewModels
{
    /// <summary>
    /// MainWindowのViewModel
    /// </summary>
    class MainWindowViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// プロパティ変更をUI側へ通知するイベント
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 画面に表示するキー入力履歴文字列。
        /// </summary>
        private string _inputHistory;

        /// <summary>
        /// キー入力取得時、この文字にセパレータを挿入するかどうかのフラグ。
        /// </summary>
        public bool IsInsertSeparatorSymbol;

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
        /// タイトル文字列プロパティ。
        /// </summary>
        public string TitleString { get; set; }

        /// <summary>
        /// キーボード入力取得を行うクラスインスタンス。
        /// </summary>
        private Models.KeyboardUtil KeyboardUtil { get; }

        /// <summary>
        /// キーボード表示を行うクラスインスタンス。
        /// </summary>
        private Models.KeyBoardDisplay KeyboardDisplay { get; }

        private bool IsShiftPressed { get; set; }

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

            //キーボード表示クラスのインスタンスを生成・イベントハンドラーの設定
            this.KeyboardDisplay = new Models.KeyBoardDisplay();

            //最初の入力にセパレータは不要
            this.IsInsertSeparatorSymbol = false;

            //Shiftキー初期化
            this.IsShiftPressed = false;

            //バージョン情報を取得し、タイトルへ反映する
            this.TitleString = Models.General.GetTitleString();
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

        /// <summary>
        /// キーダウンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookKeyUp_Handler(object sender, KeyboardUtil.HookKeyEventArgs e)
        {
            //キーダウン時は何もしない
        }

        /// <summary>
        /// キーアップイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookKeyDown_Handler(object sender, KeyboardUtil.HookKeyEventArgs e)
        {
            string inputChar = null;
            //入力文字を取得する
            if (this.IsShiftPressed)
            {
                //シフトが押されている場合、大文字を取得
                inputChar = KeyboardUtilConstants.bigKeyNameDictionary[e.vkCode];
            }
            else
            {
                //シフトが押されていない場合、小文字を取得
                inputChar = KeyboardUtilConstants.smallKeyNameDictionary[e.vkCode];
            }

            if (inputChar == null)
            {
                //入力文字が取得できなかった場合
                return;
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
        }

        /// <summary>
        /// Shiftキーダウンイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyHookShiftKeyDown_Handler(object sender, Models.KeyboardUtil.HookKeyEventArgs e)
        {
            this.IsShiftPressed = true;
        }

    }
}
