using HookApp.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HookApp.ViewModels.Consts;
using HookApp.Models;
using HookApp.ViewModels.Interfaces;

namespace HookApp.ViewModels
{
    internal class OptionViewModel : BaseViewModel
    {

        private AppSettingsModel AppSettings { get; }
        
        public List<String> FolderList { get; set; }
        public int FolderListSelectedIndex { get; set; }

        public ICommand OKInput { get; private set; }
        public ICommand CancelInput { get; private set; }

        public OptionViewModel(AppSettingsModel appSettings)
        {
            //アプリ設定情報Model
            AppSettings = appSettings;

            //TODO : 初期表示項目の設定
            //コンボボックスバインド
            //初期選択処理

            //ボタンアクションの割当
            //AddCommandAction(CommandActionType.OK_Input, OnOkPressAction);
            //AddCommandAction(CommandActionType.Cancel_Input, OnCancelPressAction);

            //コマンドクラスのインスタンス化
            OKInput = new MenuCloseButtonInput(this);

            //画面を閉じる
            CancelInput = new MenuCloseButtonInput(this);

        }

        public void OnOkPressAction()
        {
            //選択されたフォルダ名称を設定
            AppSettings.SelectedSkinFolderName = FolderList[FolderListSelectedIndex];

        }

        ///// <summary>
        ///// キャンセルボタン押下
        ///// </summary>
        //public void OnCancelPressAction()
        //{
        //    //画面を閉じる

        //}

    }
}
