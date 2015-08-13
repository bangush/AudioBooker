using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AudioBooker.classes;

namespace AudioBooker {
    public class LogicShit
    {
        private const string DumpFolder = "____temp";

        private XmlAudiobook xml;
        private XmlWavEvent curSegment;
        private AudioRecManager recMan;
        private TimeSpan curTime;

        public LogicShit(AudioRecManager recMan) {
            this.recMan = recMan;
            FilenameFormat = DumpFolder + "/segment_{0}.wav";
        }

        #region events
        public delegate void VoidHandler();
        public event VoidHandler CurSegmentUpdated;
        private void FireHandler(VoidHandler x) {
            if (x != null)
                x();
        }
        #endregion

        #region state machine

        public void commitSegment() {
            if (curSegment == null)
                return;
            recMan.StopRecording();
            recMan.SaveLastRecording(curSegment.Filename);
            curTime += recMan.LastRecordingLength;
            xml.Segments.Add(curSegment);
            startSegment();
        }

        public void rollbackSegment() {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
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
            startSegment();
        }
        //public void pauseRecording() {
        //    commitSegment();
        //    curSegment = null;
        //}
        public void stopRecording() {
            //commitSegment();
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            curSegment = null;
        }

        private void startSegment() {
            recMan.StartRecording();
            curSegment = new XmlWavEvent() {
                Type = WavEventType.WavRecording1,
                Filename = string.Format(FilenameFormat, xml.Segments.Count),
                //TimeIn = curTime,
            };
            FireHandler(CurSegmentUpdated);
        }

        public void cleanUpFilenames(string filenameBig) {
            var newFolderName = Utils.GetFullPathWithoutExtension(filenameBig);
            foreach (var seg in AudiobookXml.Segments) {
                seg.Filename = seg.Filename.Replace(DumpFolder + "/", "");
            }
            Directory.Move(DumpFolder, newFolderName);
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
