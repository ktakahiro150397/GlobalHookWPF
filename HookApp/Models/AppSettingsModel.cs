using System;
using System.Collections.Generic;
using System.Text;

namespace HookApp.Models
{
    public class AppSettingsModel
    {
        public string SelectedSkinFolderName { get; set; }

        public AppSettingsModel(string skinFolderName)
        {
            SelectedSkinFolderName = skinFolderName;
        }
    }
}
