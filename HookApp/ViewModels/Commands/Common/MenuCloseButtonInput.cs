using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HookApp.ViewModels.Consts;
using HookApp.ViewModels.Commands.Base;

namespace HookApp.ViewModels.Commands.Common
{

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
