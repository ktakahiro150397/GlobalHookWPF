using System;
using System.Collections.Generic;
using System.Text;

namespace HookApp
{
   /// <summary>
   /// このプログラムのエントリポイント。
   /// </summary>
    class StartUpProgram
    {
        [STAThread]
        static void Main()
        {
            App application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
