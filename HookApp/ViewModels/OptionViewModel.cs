using HookApp.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HookApp.ViewModels.Consts;
using HookApp.Models;
using HookApp.ViewModels.Interfaces;
using HookApp.ViewModels.Commands.Common;
using HookApp.ViewModels.Commands.Option;

namespace HookApp.ViewModels
{
    internal class OptionViewModel : BaseViewModel
    {

        private AppSettingsModel AppSettings { get; }
        
        public List<String> FolderList
        {
            get
            {
                return AppSettings.SkinFolderNameList;
            }
            set
            {
                AppSettings.SkinFolderNameList = value;
            }
        }

        public string SelectedFolderListName
        {
            get
            {
                return AppSettings.SelectedSkinFolderName;
            }
            set
            {
                AppSettings.SelectedSkinFolderName = value;
            }
        }

        public ICommand OKInput { get; private set; }
        public ICommand CancelInput { get; private set; }

        public OptionViewModel(AppSettingsModel appSettings)
        {
            //アプリ設定情報Model
            AppSettings = appSettings;

            //コマンドクラスのインスタンス化
            OKInput = new OptionOKInputCommand(this);
            CancelInput = new MenuCloseButtonInput(this);

        }

        /// <summary>
        /// 現在のビューモデルの状態をモデルに保存します。
        /// </summary>
        public void SaveSelection()
        {
            SaveFolderSelection();
        }

        /// <summary>
        /// 現在のフォルダ選択状態をモデルに保存します。
        /// </summary>
        public void SaveFolderSelection()
        {
            AppSettings.SaveFolderSelection();
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
