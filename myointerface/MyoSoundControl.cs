using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;

namespace myointerface
{
    public class MyoSoundControl
    {
        IChannel Channel;
        IHub Hub;
        MainWindow Window;
        Dictionary<Pose, SoundPlayer> PoseToSound = new Dictionary<Pose, SoundPlayer>();

        //testing values
        

        public MyoSoundControl(MainWindow Gui)
        {
            this.Window = Gui;
            this.Channel = MyoSharp.Communication.Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            this.Hub = MyoSharp.Device.Hub.Create(Channel);
            Hub.MyoConnected += (sender, e) =>
            {

                Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                e.Myo.Vibrate(VibrationType.Long);
                e.Myo.PoseChanged += Myo_PoseChanged;
                e.Myo.Locked += Myo_Locked;
                e.Myo.Unlocked += Myo_Unlocked;
                Pose testpose = Pose.Fist;
                IPoseSequence testsequence = PoseSequence.Create(e.Myo, Pose.Fist, Pose.FingersSpread);
                testsequence.PoseSequenceCompleted += Testsequence_PoseSequenceCompleted;
                this.PoseToSound[testpose] = new SoundPlayer("./Sound/Mario.wav");
            };

            // listen for when the Myo disconnects
            Hub.MyoDisconnected += (sender, e) =>
            {
                Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                e.Myo.PoseChanged -= Myo_PoseChanged;
                e.Myo.Locked -= Myo_Locked;
                e.Myo.Unlocked -= Myo_Unlocked;
            };
            
        }

        private void Testsequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            //this.PoseToSound[(IPoseSequence)sender].Play();
        }

        public void Run()
        {
            Channel.StartListening();
        }
        #region Event Handlers
        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            //Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
            //System.Windows.Forms.MessageBox.Show("pose {0}", e.Myo.Pose.ToString());
            if (PoseToSound.ContainsKey(e.Pose))
                PoseToSound[e.Pose].Play(); 
        }

        private void Myo_Unlocked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has unlocked!", e.Myo.Arm);
        }

        private void Myo_Locked(object sender, MyoEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has locked!", e.Myo.Arm);
        }
        #endregion
    }

}

