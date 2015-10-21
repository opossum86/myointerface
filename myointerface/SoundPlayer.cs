using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myointerface
{
    public class SoundPlayer : System.Windows.Media.MediaPlayer
    {
        Uri SoundLocation;
        public string File
        {
            get { return SoundLocation; }
        }

        public SoundPlayer(string SoundFile)
        {
            this.SoundLocation = new Uri(SoundFile);
            this.Open(SoundLocation);   
        }
    }
}
