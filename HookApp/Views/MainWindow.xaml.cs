using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HookApp.ViewModels;
using HookApp.Views;

namespace HookApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _vm = null;

        public MainWindow()
        {
            InitializeComponent();

            // ウィンドウをマウスのドラッグで移動できるようにする
            this.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };

            //ViewModelのインスタンスを生成し、DataContextに代入
            if (this._vm == null)
            {
                this._vm = new MainWindowViewModel();
                this.DataContext = _vm;
            }

        }

        /// <summary>
        /// クリアボタン押下のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _vm.inputHistory = "";

            //クリア後にセパレータは挿入しない
            _vm.IsInsertSeparatorSymbol = false;
        }

        /// <summary>
        /// テキスト変更イベント。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputHistory_TextChanged(object sender, TextChangedEventArgs e)
        {
            //テキストが変更されたとき、最下部までスクロールする
            var txtBox = (TextBox)sender;
            txtBox.ScrollToEnd();
        }

        /// <summary>
        /// バージョン情報クリックイベント。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VersionInfo_Click(object sender, RoutedEventArgs e)
        {
            var versionInfo = new VersionInfo();
            versionInfo.Show();
        }
    }
}
