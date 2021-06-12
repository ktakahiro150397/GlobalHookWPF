using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace HookApp.ViewModels.Commands.MainWindow
{

    /// <summary>
    /// テキストクリア処理を行うコマンドクラス。
    /// </summary>
    internal class ClearInput_Imp : ICommand
    {

        private MainWindowViewModel _vm;
        public ClearInput_Imp(MainWindowViewModel vm)
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
            _vm.inputHistory = "";
        }
    }

}
