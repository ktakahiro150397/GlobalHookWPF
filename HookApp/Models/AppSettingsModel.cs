﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace HookApp.Models
{
    public class AppSettingsModel
    {
        public string SelectedSkinFolderName { get; set; }
        public List<string> SkinFolderNameList { get; set; }

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
            
        }

        /// <summary>
        /// 現在選択されているフォルダを保存します。
        /// </summary>
        public void SaveFolderSelection()
        {
            Setting.Default.SKIN_SELECTED_FOLDER_NAME = SelectedSkinFolderName;
        }

    }
}
