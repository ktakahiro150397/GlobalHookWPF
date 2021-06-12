using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using HookApp.ViewModels.Consts;

namespace HookApp.ViewModels.Commands
{
    internal abstract class ButtonInputCommand : ICommand
    {
        protected BaseViewModel _vm;

        public ButtonInputCommand(BaseViewModel vm)
        {
            _vm = vm;
        }

        public virtual event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);
    }


    internal class OKButtonInput : ButtonInputCommand
    {

        public OKButtonInput(BaseViewModel vm) : base(vm)
        {
        }

        public override void Execute(object parameter)
        {
            _vm.GetCommandAction(CommandActionType.OK_Input).Invoke();
        }

    }

    internal class CancelButtonInput : ButtonInputCommand
    {

        public CancelButtonInput(BaseViewModel vm) : base(vm)
        {
        }

        public override void Execute(object parameter)
        {
            _vm.GetCommandAction(CommandActionType.Cancel_Input).Invoke();
        }

    }


}
