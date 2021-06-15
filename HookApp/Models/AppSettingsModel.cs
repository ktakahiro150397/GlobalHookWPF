using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HookApp.Models
{
    public class AppSettingsModel : INotifyPropertyChanged
    {
        private string _SelectedSkinFolderName;
        private string _SelectedSkinFolderNameWhenOpened;

        public string SelectedSkinFolderName {
            get
            {
                return _SelectedSkinFolderName;
            }
            set
            {
                _SelectedSkinFolderName = value;
                Setting.Default.SKIN_SELECTED_FOLDER_NAME = SelectedSkinFolderName;
                Setting.Default.Save();

                //変更をViewModelに通知
                NotifyPropertyChanged();

            }
        }

        public List<string> SkinFolderNameList { get; set; }


        public string SelectedSkinSettingFilePath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(SelectedFolderPath, Setting.Default.SYSTEM_SKIN_SETTING_FILE_NAME));
            }
        }

        public string SelectedSkinBaseKeyboardPicFilePath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(SelectedFolderPath,KeyboardBasePicName));
            }
        }

        public string SelectedFolderPath
        {
            get
            {
                return Path.GetFullPath(Path.Combine(Setting.Default.SYSTEM_RESOURCE_FOLDER_NAME, SelectedSkinFolderName));
            }
        }

        public string KeyboardBasePicName
        {
            get
            {
                return Setting.Default.SYSTEM_SKIN_KEYBOARD_BASE_FILE_NAME;
            }
        }

        public AppSettingsModel()
        {
            string selectedFolderName;

            //ローカルフォルダ名称一覧リストを取得する
            SkinFolderNameList = Directory.GetDirectories(Setting.Default.SYSTEM_RESOURCE_FOLDER_NAME).Select((folderPath) => { return Path.GetFileName(folderPath); }).ToList();
            
            if (SkinFolderNameList.Any((folderName) => { return folderName == Setting.Default.SKIN_SELECTED_FOLDER_NAME; }))
            {

                selectedFolderName = Setting.Default.SKIN_SELECTED_FOLDER_NAME;

            }
            else
            {
                selectedFolderName = Setting.Default.SKIN_DEFAULT_FOLDER_NAME;
            }
            
            SelectedSkinFolderName = selectedFolderName;
            _SelectedSkinFolderNameWhenOpened = selectedFolderName;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 現在選択されているフォルダを保存します。
        /// </summary>
        public void SaveFolderSelection()
        {
            //SelectedSkinFolderName

        }
    }
}
