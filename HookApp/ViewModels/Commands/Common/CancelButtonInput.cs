using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HookApp.ViewModels.Consts;
using HookApp.ViewModels.Commands.Base;

namespace HookApp.ViewModels.Commands.Common
{
    internal class CancelButtonInput : ButtonInputCommand
    {

        public CancelButtonInput(BaseViewModel vm) : base(vm)
        {
            CommandActionType = CommandActionType.Cancel_Input;
        }
    }
}
