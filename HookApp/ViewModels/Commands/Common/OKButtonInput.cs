using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using HookApp.ViewModels.Consts;
using HookApp.ViewModels.Commands.Base;

namespace HookApp.ViewModels.Commands.Common
{
    internal class OKButtonInput : ButtonInputCommand
    {

        public OKButtonInput(BaseViewModel vm) : base(vm)
        {
            CommandActionType = CommandActionType.OK_Input;
        }
    }
}
