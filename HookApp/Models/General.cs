using System;
using System.Text.Json;
using System.IO;
using System.Net;

namespace HookApp.Models
{
    public static class General
    {
        /// <summary>
        /// タイトル文字列を返します。
        /// </summary>
        /// <returns></returns>
        public static string GetTitleString()
        {
            return Setting.Default.APP_NAME + "    " + Setting.Default.TITLE_VERSION_PREFIX + VersionInfoProperty.GetVersionString();
        }

       

    }
}
