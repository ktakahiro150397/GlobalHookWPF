using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HookApp.Views;

namespace HookApp.ViewModels.Commands
{
    class CloseWindow : ICommand
    {
        private MainWindowViewModel _vm;
        public CloseWindow(MainWindowViewModel vm)
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
            
        }
    }
}
