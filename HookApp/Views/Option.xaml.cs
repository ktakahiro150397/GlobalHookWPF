using HookApp.Models;
using HookApp.ViewModels;
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
    /// Option.xaml の相互作用ロジック
    /// </summary>
    public partial class Option : Window
    {

        private OptionViewModel _vm;
        public Option(AppSettingsModel optionModel)
        {
            InitializeComponent();

            //ViewModelのインスタンスを生成し、DataContextに代入
            if (this._vm == null)
            {
                this._vm = new OptionViewModel(optionModel);
                this.DataContext = _vm;
            }
        }
    }
}
