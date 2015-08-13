using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioBooker.classes;
using Mp3SplitterCommon;

namespace IlyaFranker {
    public class LogicShit {

        private XmlIlyaFrankAbook xml;
        //private XmlSegment curSegment;
        private XmlAbookEvent_OneSentence curSentence;
        private IRecorder recorder;
        //private List<XmlSegment> pendingRussian;

        public LogicShit(IRecorder recorder) {
            this.recorder = recorder;
            PhraseIndex = 0;
            //pendingRussian = new List<XmlSegment>();
        }

        #region events
        public delegate void VoidHandler();
        public event VoidHandler CurSegmentUpdated;
        public event VoidHandler CurSentenceUpdated;
        public event VoidHandler ItalianXmlUpdated;
        public event VoidHandler PrematureAdvancement;
        #endregion

        #region logic

        public void startRecording() {
            if (recorder.State != IRecorderState.Paused)
                xml = new XmlIlyaFrankAbook();
            //startSegment();
        }
        public void stopRecording() {
            PurgePendingSentences();
            curSegment = null;
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
        }

        private void commitTheLastItalianSegment() {
            // mark last italian segment ass processed
            if (ItalianXml.CurSegment != null) {
                ItalianXml.CurSegment.ProcessedState = XmlSegmentState.Pending;
            }
            // if user forgot to press space having read nothing, commit a segment...
            if (curSentence != null && curSegment != null) {
                if (!curSentence.RussianSegments.Any())
                    commitSegment();
            }
        }

        public void NextPhrase() {
            if (ItalianXml == null)
                return;
            if (PhraseIndex >= ItalianXml.Segments.Count) {
                PurgePendingSentences();
                return;
            }
            if (curSentence != null && curSegment == null) {
                if (PrematureAdvancement != null)
                    PrematureAdvancement();
                return;
            }

            commitTheLastItalianSegment();

            // goto next italian segment
            ItalianXml.CurSegment = ItalianXml.Segments.Skip(PhraseIndex).FirstOrDefault();
            if (ItalianXml.CurSegment == null) {
                curSentence = null;
                if (CurSentenceUpdated != null)
                    CurSentenceUpdated();
                return;
            }

            // ...if it exists, ADVANCE!!!
            curSentence = new XmlAbookEvent_OneSentence() {
                ItalianSegment = ItalianXml.CurSegment,
                ItalianSegments = new List<XmlSegment> { ItalianXml.CurSegment }
            };
            xml.Events.Add(curSentence);
            curSegment = null;
            PhraseIndex++;
            Mp3Utils.PlayPieceOfAFile(ItalianMp3,
                        ItalianXml.CurSegment.TimeIn.TotalSeconds,
                        ItalianXml.CurSegment.TimeOut.TotalSeconds
                        );

            // events;
            if (CurSentenceUpdated != null)
                CurSentenceUpdated();
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
            if (ItalianXmlUpdated != null)
                ItalianXmlUpdated();
        }
        public void RereadLastPhrase() {
            if (ItalianXml == null)
                return;
            if (PhraseIndex >= ItalianXml.Segments.Count)
                return;
            if (ItalianXml.CurSegment == null)
                return;
            Mp3Utils.PlayPieceOfAFile(ItalianMp3,
                        ItalianXml.CurSegment.TimeIn.TotalSeconds,
                        ItalianXml.CurSegment.TimeOut.TotalSeconds
                        );
        }

        public void commitSegment() {
            if (curSentence == null)
                return;
            if (curSegment == null) {
                startSegment();
                return;
            }
            curSegment.TimeOut = DateTime.Now.Subtract(recorder.StartTime).Subtract(TimeSpan.FromSeconds(AudiobookerMp3Utils.CutSecondsAfter));
            curSentence.RussianSegments.Add(curSegment);
            pendingRussian.Add(curSegment);
            startSegment();
        }

        public void rollbackSegment() {
            startSegment();
        }

        private void startSegment() {
            curSegment = new XmlSegment() {
                TimeIn = DateTime.Now.Subtract(recorder.StartTime).Add(TimeSpan.FromSeconds(AudiobookerMp3Utils.CutSecondsBefore)),
            };
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
        }

        public void PurgePendingSentences() {
            if (ItalianXml == null)
                return;

            commitTheLastItalianSegment();
            ItalianXml.CurSegment = null;

            var pending = ItalianXml.Segments.Where(x => x.ProcessedState == XmlSegmentState.Pending);
            if (!pending.Any())
                return;

            curSentence = null;
            curSegment = null;

            // ...if it exists ADVANCE!!!
            var purgeEvent = new XmlAbookEvent_PurgeSentences();
            xml.Events.Add(purgeEvent);
            foreach (var ppp in pending) {
                purgeEvent.ItalianSegments.Add(ppp);
                ppp.ProcessedState = XmlSegmentState.Processed;
            }
            foreach (var ppp in pendingRussian) {
                purgeEvent.RussianSegments.Add(ppp);
            }
            pendingRussian.Clear();

            // events
            if (CurSentenceUpdated != null)
                CurSentenceUpdated();
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
            if (ItalianXmlUpdated != null)
                ItalianXmlUpdated();
        }

        #endregion

        #region props

        public int PhraseIndex { get; set; }
        public string ItalianMp3 { get; set; }

        public XmlSegment CurIlyaSegment {
            get {
                return curSegment;
            }
        }

        public XmlIlyaFrankAbook IlyaXml {
            get {
                return xml;
            }
        }

        private XmlAudiobook _italXml;
        public XmlAudiobook ItalianXml {
            get {
                return _italXml;
            }
            set {
                PhraseIndex = 0;
                _italXml = value;
                if (ItalianXmlUpdated != null)
                    ItalianXmlUpdated();
            }
        }

        #endregion
    }

    public enum LogicShitState {
        Idle = 1,
        Recording = 2,
        Paused = 3,
    }
}
