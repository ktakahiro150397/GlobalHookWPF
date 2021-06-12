using System;
using System.Collections.Generic;
using System.Text;

namespace HookApp.Models.KeyBoardDisplay
{
    public interface IKeyDisplayInfo
    {
        public string PicUri { get; }
        public double Height { get; }
        public double Width { get; }
        public double Top { get; }
        public double Left { get; }
        public KeyboardUtilConstants.VirtualKeyCode Key { get; }
    }
}
