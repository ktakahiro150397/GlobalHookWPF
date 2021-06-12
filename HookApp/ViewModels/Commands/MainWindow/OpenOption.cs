using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HookApp.Views;
using HookApp.Models;

namespace HookApp.ViewModels.Commands.MainWindow
{
    /// <summary>
    /// オプション画面を開くコマンドクラス。
    /// </summary>
    internal class OpenOption : ICommand
    {
        private MainWindowViewModel _vm;
        public OpenOption(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {

            }

            remove
            {

            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        void ICommand.Execute(object parameter)
        {
            Views.Option option = new Views.Option(new AppSettingsModel());
            option.Show();
        }
    }
}
