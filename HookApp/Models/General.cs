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
            return Setting.Default.TITLE_STRING + "    " + Setting.Default.TITLE_VERSION_PREFIX + GetVersionString();
        }

        /// <summary>
        /// バージョン情報の文字列を返します。
        /// </summary>
        /// <returns></returns>
        private static string GetVersionString()
        {
            //APIから最新バージョンを取得し、プロパティへ格納する
            //最新バージョンを返すAPIのURI
            string requestUrl = Setting.Default.API_ENDPOINT + Setting.Default.API_GET_LATEST_VER;

            //リクエストを投げてレスポンスのStreamReaderを得る
            var request = WebRequest.Create(requestUrl);
            request.Method = "GET";
            var responseStream = request.GetResponse().GetResponseStream();
            var reader = new StreamReader(responseStream);

            //取得したデータをパース
            var obtainedJsonStr = reader.ReadToEnd();
            var versionInfo = JsonSerializer.Deserialize<VersionInfo>(obtainedJsonStr);
            if (versionInfo == null)
            {
                return "<バージョン情報が取得できませんでした>";
            }

            string versionStr = null;
            //バージョンチェック
            if (Setting.Default.APP_VERSION_ID == versionInfo.id)
            {
                //実行中のアプリは最新
                //取得したバージョン情報をそのまま使用する
                versionStr = versionInfo.major.ToString() + "." + versionInfo.minor.ToString();
                if (!String.IsNullOrEmpty(versionInfo.revision))
                {
                    //リビジョン文字列が存在する場合は追加する
                    versionStr += " " + versionInfo.revision;
                }

                //最新バージョンであることを表示する
                versionStr += "  " + Setting.Default.TITLE_UPDATE_NONE;
            }
            else
            {
                //実行中のアプリからのアップデートが存在する
                //APP_VERSION_IDを指定してデータを再度取得し、バージョン情報を使用する
                string requestUrl2 = Setting.Default.API_ENDPOINT + Setting.Default.API_GET_VERSION_BY_ID + Setting.Default.APP_VERSION_ID;
                var request2 = WebRequest.Create(requestUrl2);
                request2.Method = "GET";
                var responseStream2 = request2.GetResponse().GetResponseStream();
                var reader2 = new StreamReader(responseStream2);
                var obtainedJsonStr2 = reader2.ReadToEnd();
                var versionInfo2 = JsonSerializer.Deserialize<VersionInfo>(obtainedJsonStr2);
                if (versionInfo2 == null)
                {
                    return "<バージョン情報が取得できませんでした>";
                }
                versionStr = versionInfo2.major.ToString() + "." + versionInfo2.minor.ToString();
                if (!String.IsNullOrEmpty(versionInfo2.revision))
                {
                    //リビジョン文字列が存在する場合は追加する
                    versionStr += " " + versionInfo2.revision;
                }

                //アップデートが存在する可能性を表示する
                versionStr += "  " + Setting.Default.TITLE_UPDATE_EXIST;
            }


            return versionStr;
        }

        /// <summary>
        /// バージョン情報データ型
        /// </summary>
        private class VersionInfo
        {
            public int id { get; set; }
            public int major { get; set; }
            public int minor { get; set; }
            public string revision { get; set; }
        }

    }
}
