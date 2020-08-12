using HookApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HookApp.Views
{
    /// <summary>
    /// VersionInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionInfo : Window
    {
        public VersionInfo()
        {
            InitializeComponent();

            //バージョン情報テキストの表示
            if (VersionInfoProperty.IsNoError)
            {
                txtVersionInfo.Text = Setting.Default.APP_NAME + "    " + VersionInfoProperty.GetVersionString();

            }
            else
            {
                txtVersionInfo.Text = "バージョンの取得に失敗しました。";
            }


        }
    }
}
