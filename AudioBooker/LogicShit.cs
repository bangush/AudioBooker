using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AudioBooker.classes;
using System.Windows.Forms;
using Audiobooker.controls;
using Miktemk;
using Miktemk.Mp3;

namespace AudioBooker
{
    public class LogicShit : IAudioBookerLogicForRecControls
    {
        private const string DumpFolder = "____temp";

        private XmlAudiobook xml;
        private XmlWavEvent curSegment;
        private AudioRecManager recMan;
        private TimeSpan curTime;
        private Timer ttSex;
        private DateTime whenLastSegmentStarted;

        public LogicShit(AudioRecManager recMan) {
            this.recMan = recMan;
            FilenameFormat = DumpFolder + "/segment_{0}.wav";
            ttSex = new Timer();
            ttSex.Interval = 500;
            ttSex.Tick += ttSex_tick;
        }

        private void ttSex_tick(object sender, EventArgs e)
        {
            FireSegmentTimeUpdatedBasedOnCurrentSituation();
        }

        #region events
        public event VoidHandler CurSegmentUpdated;
        public event VoidHandler XmlUpdated;
        public event VoidHandler RecStopped;
        public event TimeSpanHandler SegmentTimeUpdated;
        private AudioPlaya playa;
        private void FireHandler(VoidHandler x)
        {
            if (x != null)
                x();
        }
        private void FireHandler(TimeSpanHandler x, TimeSpan ts)
        {
            if (x != null)
                x(ts);
        }
        private void FireSegmentTimeUpdatedBasedOnCurrentSituation()
        {
            if (!recMan.IsRecording)
                FireHandler(SegmentTimeUpdated, TimeSpan.Zero);
            var tsDiff = DateTime.Now.Subtract(whenLastSegmentStarted);
            FireHandler(SegmentTimeUpdated, tsDiff);
        }
        #endregion

        #region state machine

        private void commitSegment() {
            if (curSegment == null)
                return;
            recMan.StopRecording();
            recMan.SaveLastRecording(curSegment.Filename);
            curTime += recMan.LastRecordingLength;
            curSegment.TimeOut = curTime;
            xml.Segments.Add(curSegment);
            startSegment();
        }

        private void rollbackSegment()
        {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            startSegment();
        }

        private void deleteLastCommitedSegmentAndRead()
        {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            xml.Segments.RemoveAt(xml.Segments.Count-1);
            startSegment();
        }

        public void addSoundEffect(string filaname) {
            //xml.SoundEffects.Add(new XmlSoundEffect {
            //    TimeIn = DateTime.Now.Subtract(recorder.StartTime),
            //    Filename = filaname
            //});
        }

        public void startRecording() {
            xml = new XmlAudiobook();
            curTime = TimeSpan.Zero;
            ttSex.Start();
            startSegment();
        }
        //public void pauseRecording() {
        //    commitSegment();
        //    curSegment = null;
        //}
        public void stopRecording() {
            //commitSegment();
            ttSex.Stop();
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            curSegment = null;
            FireHandler(RecStopped);
            FireSegmentTimeUpdatedBasedOnCurrentSituation();
        }

        private void startSegment() {
            whenLastSegmentStarted = DateTime.Now;
            recMan.StartRecording();
            curSegment = new XmlWavEvent() {
                Type = WavEventType.WavRecording1,
                Filename = string.Format(FilenameFormat, xml.Segments.Count),
                TimeIn = curTime,
            };
            FireHandler(CurSegmentUpdated);
            FireSegmentTimeUpdatedBasedOnCurrentSituation();
        }

        public void cleanUpFilenames(string filenameBig) {
            var newFolderName = UtilsCore.GetFullPathWithoutExtension(filenameBig);
            foreach (var seg in AudiobookXml.Segments) {
                seg.Filename = seg.Filename.Replace(DumpFolder + "/", "");
            }
            if (Directory.Exists(newFolderName))
                Directory.Delete(newFolderName, true);
            Directory.Move(DumpFolder, newFolderName);
        }

        public void keyPressed(System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                commitSegment();
            else if (e.Shift && e.KeyCode == Keys.Back)
                deleteLastCommitedSegmentAndRead();
            else if (e.KeyCode == Keys.Back)
                rollbackSegment();
            else if (e.KeyCode == Keys.R)
                playbackLastSegment();
            if (XmlUpdated != null)
                XmlUpdated();
        }

        private void playbackLastSegment()
        {
            var lastSeg = xml.Segments.LastOrDefault();
            if (lastSeg == null)
                return;

            if (playa != null)
            {
                playa.Stop();
                return;
            }

            playa = new AudioPlaya(lastSeg.Filename);
            playa.Play();
            playa.Finished += (playaDone) =>
            {
                playaDone.Dispose();
                playaDone = null;
            };
        }

        #endregion

        #region props

        public string AudiobookMp3 { get; set; }
        public XmlAudiobook AudiobookXml {
            get {
                return xml;
            }
            set {
                xml = value;
            }
        }
        private string FilenameFormat { get; set; }
        public XmlWavEvent CurSegment { get { return curSegment; } }

        #endregion


    }
}
