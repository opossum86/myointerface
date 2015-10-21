using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using System.Windows.Forms;

namespace myointerface
{
    class Programm
    {
        static void Main(string[] args)
        {
            MainWindow Window = new MainWindow(); //Forms intialisieren
            MyoSoundControl Controller = new MyoSoundControl(Window); //objekt der Klasse Myosoundcontrol
            Controller.Run();
            Application.EnableVisualStyles(); //zugriff auf steuerelemente, farben etc. aus der Forms (Windows)
            Application.Run(Window);
            Application.ApplicationExit += Controller.Quit;
        }

        
        
    }

}