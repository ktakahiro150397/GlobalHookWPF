using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HookApp.ViewModels.Consts;

namespace HookApp.ViewModels.Commands.Base
{
    internal abstract class ButtonInputCommand : ICommand
    {
        protected BaseViewModel _vm;
        protected CommandActionType CommandActionType;

        public ButtonInputCommand(BaseViewModel vm)
        {
            _vm = vm;
        }

        public virtual event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            _vm.GetCommandAction(CommandActionType).Invoke();
        }
    }



}
