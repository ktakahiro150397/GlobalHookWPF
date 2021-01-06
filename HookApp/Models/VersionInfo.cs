using System;
using System.Text.Json;
using System.IO;
using System.Net;

namespace HookApp.Models
{
    public static class VersionInfoProperty
    {
        public static bool IsNoError { get; }

        /// <summary>
        /// このアプリケーションのバージョンID。
        /// </summary>
        public static int ID { get; }

        /// <summary>
        /// このアプリケーションのメジャーバージョン番号。
        /// </summary>
        public static int major { get; }

        /// <summary>
        /// このアプリケーションのマイナーバージョン番号。
        /// </summary>
        public static int minor { get; }

        /// <summary>
        /// このアプリケーションのリビジョン文字列。
        /// </summary>
        public static string revision { get; }

        /// <summary>
        /// バージョン情報のプロパティを初期化します。
        /// </summary>
        static VersionInfoProperty()
        {
            // APIから最新バージョンを取得し、プロパティへ格納する
            //最新バージョンを返すAPIのURI
            string requestUrl = Setting.Default.API_ENDPOINT + Setting.Default.API_GET_LATEST_VER;

            //リクエストを投げてレスポンスのStreamReaderを得る
            var request = WebRequest.Create(requestUrl);
            request.Method = "GET";

            try
            {
                var responseStream = request.GetResponse().GetResponseStream();
                var reader = new StreamReader(responseStream);

                //取得したデータをパース
                var obtainedJsonStr = reader.ReadToEnd();
                var versionInfo = JsonSerializer.Deserialize<APIVersionInfo>(obtainedJsonStr);

                //自身のプロパティに設定する
                if (versionInfo == null)
                {
                    //取得失敗時
                    VersionInfoProperty.IsNoError = false;
                    VersionInfoProperty.ID = 0;
                    VersionInfoProperty.major = 0;
                    VersionInfoProperty.minor = 0;
                    VersionInfoProperty.revision = null;
                }
                else
                {
                    //取得成功時
                    VersionInfoProperty.IsNoError = true;
                    VersionInfoProperty.ID = versionInfo.id;
                    VersionInfoProperty.major = versionInfo.major;
                    VersionInfoProperty.minor = versionInfo.minor;
                    VersionInfoProperty.revision = versionInfo.revision;
                }

            }
            catch(WebException ex){
                //URLが見つからない場合
                VersionInfoProperty.IsNoError = false;
                VersionInfoProperty.ID = 0;
                VersionInfoProperty.major = 0;
                VersionInfoProperty.minor = 0;
                VersionInfoProperty.revision = null;
            }
        }

        /// <summary>
        /// バージョン情報の文字列を返します。
        /// </summary>
        /// <returns></returns>
        public static string GetVersionString()
        {
            
            if (!IsNoError)
            {
                //失敗しているが呼び出された場合
                return "<バージョン情報が取得できませんでした>";
            }
            else
            {
                string versionStr = null;
                //バージョンチェック
                if (Setting.Default.APP_VERSION_ID == VersionInfoProperty.ID)
                {
                    //実行中のアプリは最新
                    //取得したバージョン情報をそのまま使用する
                    versionStr = VersionInfoProperty.major.ToString() + "." + VersionInfoProperty.minor.ToString();
                    if (!String.IsNullOrEmpty(VersionInfoProperty.revision))
                    {
                        //リビジョン文字列が存在する場合は追加する
                        versionStr += " " + VersionInfoProperty.revision;
                    }

                    //最新バージョンであることを表示する
                    versionStr += "  " + Setting.Default.TITLE_UPDATE_NONE;
                }
                else
                {
                    //実行中のアプリからのアップデートが存在する
                    //APP_VERSION_IDを指定してデータを再度取得し、バージョン情報を使用する
                    string requestUrl = Setting.Default.API_ENDPOINT + Setting.Default.API_GET_VERSION_BY_ID + Setting.Default.APP_VERSION_ID;
                    var request = WebRequest.Create(requestUrl);
                    request.Method = "GET";
                    var responseStream = request.GetResponse().GetResponseStream();
                    var reader = new StreamReader(responseStream);
                    var obtainedJsonStr = reader.ReadToEnd();
                    var versionInfo = JsonSerializer.Deserialize<APIVersionInfo>(obtainedJsonStr);
                    if (versionInfo == null)
                    {
                        return "<最新バージョン情報が取得できませんでした>";
                    }
                    versionStr = versionInfo.major.ToString() + "." + versionInfo.minor.ToString();
                    if (!String.IsNullOrEmpty(versionInfo.revision))
                    {
                        //リビジョン文字列が存在する場合は追加する
                        versionStr += " " + versionInfo.revision;
                    }

                    //アップデートが存在する可能性を表示する
                    versionStr += "  " + Setting.Default.TITLE_UPDATE_EXIST;
                }


            return versionStr;
            }
        }

        /// <summary>
        /// APIから取得するアプリケーションのバージョン情報。
        /// </summary>
        private class APIVersionInfo
        {
            public int id { get; set; }
            public int major { get; set; }
            public int minor { get; set; }
            public string revision { get; set; }
        }

    }
}
