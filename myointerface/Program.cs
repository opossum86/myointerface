using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyoSharp.Communication;
using MyoSharp.Device;
//using MyoSharp.ConsoleSample.Internal;
using MyoSharp.Exceptions;
using System.Windows.Forms;

namespace myointerface
{
    class Programm
    {
        static void Main(string[] args)
        {
            MainWindow Window = new MainWindow();
            MyoSoundControl Controller = new MyoSoundControl(Window);
            Controller.Run();
            Application.EnableVisualStyles();
            Application.Run(Window);
        }
        
    }

}