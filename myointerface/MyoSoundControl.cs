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
        //folgende Interfaces werden aus dem Baukasten verwendet (SDK)
        IChannel Channel;
        IHub Hub;
        IMyo myo;
        MainWindow Window; //Forms
        //Bibliothek laden für pose und sequenz
        Dictionary<Pose, SoundPlayer> PoseToSound = new Dictionary<Pose, SoundPlayer>();
        Dictionary<IPoseSequence, SoundPlayer> PoseSequenceToSound = new Dictionary<IPoseSequence, SoundPlayer>();

        public MyoSoundControl(MainWindow Gui)
        {
            this.Window = Gui;
            //Channel und Hub (Schnittstelle) erzeugen
            this.Channel = MyoSharp.Communication.Channel.Create(
                ChannelDriver.Create(ChannelBridge.Create(),
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            this.Hub = MyoSharp.Device.Hub.Create(Channel);
            Hub.MyoConnected += (sender, e) =>
            {
                this.myo = e.Myo;
                e.Myo.Vibrate(VibrationType.Long);
                e.Myo.PoseChanged += Myo_PoseChanged;
                e.Myo.Locked += Myo_Locked;
                e.Myo.Unlocked += Myo_Unlocked;

                this.myo.Unlock(UnlockType.Hold);

                //Pose in Dictionary mit Sound verbinden (Soundplayerklasse extra)
                this.PoseToSound[Pose.Fist] = new SoundPlayer("./Sound/Mario.wav");
                this.PoseToSound[Pose.WaveIn] = new SoundPlayer("./Sound/Glass.wav");
                this.PoseToSound[Pose.WaveOut] = new SoundPlayer("./Sound/Slap.wav");

                //Sequence 

                //IPoseSequence sequence0 = PoseSequence.Create(e.Myo, Pose.Fist, Pose.FingersSpread);
                //sequence0.PoseSequenceCompleted += PoseSequenceCompleted;
                //this.PoseSequenceToSound[sequence0] = new SoundPlayer("./Sound/Mario.wav");

                //IPoseSequence sequence1 = PoseSequence.Create(e.Myo, Pose.WaveIn, Pose.WaveOut);
                //sequence1.PoseSequenceCompleted += PoseSequenceCompleted;
                //this.PoseSequenceToSound[sequence1] = new SoundPlayer("./Sound/Slap.wav");

                //IPoseSequence sequence2 = PoseSequence.Create(e.Myo, Pose.WaveOut, Pose.WaveIn);
                //sequence2.PoseSequenceCompleted += PoseSequenceCompleted;
                //this.PoseSequenceToSound[sequence2] = new SoundPlayer("./Sound/Glass.wav");
            };
         
            Hub.MyoDisconnected += (sender, e) =>
            {
                e.Myo.PoseChanged -= Myo_PoseChanged;
                e.Myo.Locked -= Myo_Locked;
                e.Myo.Unlocked -= Myo_Unlocked;
            };            
        }
        public void Quit(object sender, EventArgs e)
        {
            this.myo.Lock();
        }
        ~MyoSoundControl()
        {

        }
        private void PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            var sequence = (IPoseSequence)sender;

            if (PoseSequenceToSound.ContainsKey(sequence))
                PoseSequenceToSound[sequence].Play();
        }

        public void Run() // beginnt Daten vom Myoarmband einzulesen
        {
            Channel.StartListening();
        }
        #region Event Handlers

        private void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            if (PoseToSound.ContainsKey(e.Pose))
                PoseToSound[e.Pose].Play(); 
        }

        private void Myo_Unlocked(object sender, MyoEventArgs e)
        {
        }

        private void Myo_Locked(object sender, MyoEventArgs e)
        {
        }
        #endregion
    }

}

