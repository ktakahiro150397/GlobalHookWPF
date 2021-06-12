using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HookApp.ViewModels.Consts;

namespace HookApp.ViewModels.Commands
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


    internal class OKButtonInput : ButtonInputCommand
    {

        public OKButtonInput(BaseViewModel vm) : base(vm)
        {
            CommandActionType = CommandActionType.OK_Input;
        }
    }

    internal class CancelButtonInput : ButtonInputCommand
    {

        public CancelButtonInput(BaseViewModel vm) : base(vm)
        {
            CommandActionType = CommandActionType.Cancel_Input;
        }
    }

    internal class MenuCloseButtonInput : ButtonInputCommand
    {

        public MenuCloseButtonInput(BaseViewModel vm) : base(vm)
        {
            CommandActionType = CommandActionType.Menu_Close;
        }

        public override void Execute(object parameter)
        {
            Window window = (Window)parameter;
            window.Close();
        }

    }



}
