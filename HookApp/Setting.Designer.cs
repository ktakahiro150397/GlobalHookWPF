﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HookApp {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.7.0.0")]
    internal sealed partial class Setting : global::System.Configuration.ApplicationSettingsBase {
        
        private static Setting defaultInstance = ((Setting)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Setting())));
        
        public static Setting Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("_")]
        public string KEYHISTORY_SEPARATOR {
            get {
                return ((string)(this["KEYHISTORY_SEPARATOR"]));
            }
            set {
                this["KEYHISTORY_SEPARATOR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("⏎")]
        public string KEYHISTORY_RETURN_SYMBOL {
            get {
                return ((string)(this["KEYHISTORY_RETURN_SYMBOL"]));
            }
            set {
                this["KEYHISTORY_RETURN_SYMBOL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[ ]")]
        public string KEYHISTORY_SPACE_SYMBOL {
            get {
                return ((string)(this["KEYHISTORY_SPACE_SYMBOL"]));
            }
            set {
                this["KEYHISTORY_SPACE_SYMBOL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("キー入力履歴取得App")]
        public string TITLE_STRING {
            get {
                return ((string)(this["TITLE_STRING"]));
            }
            set {
                this["TITLE_STRING"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://globalhookapi.azurewebsites.net")]
        public string API_ENDPOINT {
            get {
                return ((string)(this["API_ENDPOINT"]));
            }
            set {
                this["API_ENDPOINT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/api/Hook/LatestVersion")]
        public string API_GET_LATEST_VER {
            get {
                return ((string)(this["API_GET_LATEST_VER"]));
            }
            set {
                this["API_GET_LATEST_VER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ver : ")]
        public string TITLE_VERSION_PREFIX {
            get {
                return ((string)(this["TITLE_VERSION_PREFIX"]));
            }
            set {
                this["TITLE_VERSION_PREFIX"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11")]
        public int APP_VERSION_ID {
            get {
                return ((int)(this["APP_VERSION_ID"]));
            }
            set {
                this["APP_VERSION_ID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("【新バージョンが存在します】")]
        public string TITLE_UPDATE_EXIST {
            get {
                return ((string)(this["TITLE_UPDATE_EXIST"]));
            }
            set {
                this["TITLE_UPDATE_EXIST"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/api/Hook/GetVersion/")]
        public string API_GET_VERSION_BY_ID {
            get {
                return ((string)(this["API_GET_VERSION_BY_ID"]));
            }
            set {
                this["API_GET_VERSION_BY_ID"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("【最新バージョンです】")]
        public string TITLE_UPDATE_NONE {
            get {
                return ((string)(this["TITLE_UPDATE_NONE"]));
            }
            set {
                this["TITLE_UPDATE_NONE"] = value;
            }
        }
    }
}
