using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using HookApp.ViewModels.Commands.Base;
using HookApp.ViewModels.Consts;

namespace HookApp.ViewModels.Commands.Option
{
    internal class OptionOKInputCommand : ButtonInputCommand
    {
        public OptionOKInputCommand(BaseViewModel vm) : base(vm)
        {
        }

        public override void Execute(object parameter)
        {

            Window _win = (Window)parameter;

            OptionViewModel vm = (OptionViewModel)_vm;
            vm.SaveSelection();

            _win.Close();
        }
    }
}
