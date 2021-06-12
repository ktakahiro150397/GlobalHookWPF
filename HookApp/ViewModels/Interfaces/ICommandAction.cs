using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace HookApp.ViewModels.Interfaces
{
    public interface ICommandAction : ICommand
    {
        public void CommandProcess();
    }
}
