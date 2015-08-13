using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AudioBooker.classes;
using Mp3SplitterCommon;

namespace IlyaFranker {

    // old logic shit
    /*public class LogicShit {

        private const string DumpFolder = "____temp";

        //private XmlWavEvent CurSegment, lang1Xml_curSegment;
        private int segmentId = 0;
        private XmlIlyaSentence curSentence;
        private AudioRecManager recMan;
        //private List<XmlWavEvent> pendingRussian;

        public LogicShit(AudioRecManager recMan) {
            this.recMan = recMan;
            PhraseIndex = 0;
            FilenameFormat = DumpFolder + "/segment_{0}.wav";
            //pendingRussian = new List<XmlWavEvent>();
        }

        #region events
        public delegate void VoidHandler();
        public event VoidHandler CurSegmentUpdated;
        //public event VoidHandler CurSentenceUpdated;
        public event VoidHandler CurParagraphUpdated;
        //public event VoidHandler Lang1XmlUpdated;
        public event VoidHandler PrematureAdvancement;
        #endregion

        #region logic

        public void startRecording() {
            if (IlyaXml == null) {
                new_all();
                new_paragraph();
            }
        }

        public void stopRecording() {
            PurgePendingSentences();
            clearSegment();
        }

        public void NextPhrase() {
            // insanity checks
            if (Lang1Xml == null)
                return;
            if (IlyaXml == null)
                return;
            if (CurIlyaParagraph == null)
                return;

            var l1seg = GetLang1Xml_curSegment();
            if (l1seg == null)
                return;

            if (CurIlyaSentence == null) {
                new_sentence(l1seg);
            }
            else if (CurIlyaSentence.Lang2Segments.Any()) {
                // if something was recorded, leave sentence (it is already added)
                CurIlyaSentence = null;
                // advance
                PhraseIndex++;
                l1seg = GetLang1Xml_curSegment();
                if (l1seg != null)
                    new_sentence(l1seg);
            }
            else {
                if (PrematureAdvancement != null)
                    PrematureAdvancement();
            }
            var fnameWav = GetLang1Xml_curSegmentFilename();
            if (fnameWav != null)
                WavUtils.PlayAllOfFile(fnameWav);
            clearSegment();
        }

        public void RereadLastPhrase() {
            if (Lang1Xml == null)
                return;
            if (IlyaXml == null)
                return;
            if (PhraseIndex >= Lang1Xml.Segments.Count)
                return;
            //if (lang1Xml_curSegment == null)
            //    return;
            WavUtils.PlayAllOfFile(GetLang1Xml_curSegmentFilename());
        }

        public void CommitSegment() {
            if (IlyaXml == null)
                return;
            if (CurIlyaSentence == null)
                return;
            if (CurSegment == null) {
                startSegment();
                return;
            }
            recMan.StopRecording();
            recMan.SaveLastRecording(CurSegment.Filename);
            segmentId++;
            // TODO: add to either one depending on mode (if + was pressed)
            CurIlyaSentence.Lang2Segments.Add(CurSegment);
            if (CurParagraphUpdated != null)
                CurParagraphUpdated();
            startSegment();
        }

        public void RollbackSegment() {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            startSegment();
        }

        public void PurgePendingSentences() {
            if (IlyaXml == null)
                return;
            if (CurIlyaParagraph == null)
                return;
            if (!CurIlyaParagraph.Sentences.SelectMany(x => x.Lang2Segments).Any()) {
                if (PrematureAdvancement != null)
                    PrematureAdvancement();
                if (CurIlyaSentence == null)
                    NextPhrase();
                return;
            }
            new_paragraph();
            PhraseIndex++;
        }

        #endregion

        #region privates

        private void startSegment() {
            if (IlyaXml == null)
                return;
            recMan.StartRecording();
            CurSegment = new XmlWavEvent() {
                //TimeIn = curTime,
                Filename = string.Format(FilenameFormat, segmentId),
            };
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
        }
        private void clearSegment() {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            CurSegment = null;
            if (CurSegmentUpdated != null)
                CurSegmentUpdated();
        }

        private void new_all() {
            IlyaXml = new XmlIlyaFrankAbook() {
                Lang1Mp3Filename = Lang1Mp3,
            };
        }
        private void new_paragraph() {
            CurIlyaParagraph = new XmlIlyaParagraph();
            IlyaXml.Paragraphs.Add(CurIlyaParagraph);
            CurIlyaSentence = null;
            if (CurParagraphUpdated != null)
                CurParagraphUpdated();
        }
        private void new_sentence(XmlWavEvent lang1) {
            CurIlyaSentence = new XmlIlyaSentence();
            CurIlyaSentence.Lang1Segments.Add(lang1);
            CurIlyaParagraph.Sentences.Add(CurIlyaSentence);
            if (CurParagraphUpdated != null)
                CurParagraphUpdated();
        }

        #endregion

        #region props

        // ----------- writing
        public XmlIlyaFrankAbook IlyaXml { get; private set; }
        public XmlIlyaParagraph CurIlyaParagraph { get; private set; }
        public XmlIlyaSentence CurIlyaSentence { get; private set; }
        public XmlWavEvent CurSegment { get; private set; }
        private string FilenameFormat { get; set; }
        
        // ----------- reading
        public int PhraseIndex { get; set; }
        public string Lang1Mp3 { get; set; }
        
        private XmlAudiobook _italXml;
        public XmlAudiobook Lang1Xml {
            get {
                return _italXml;
            }
            set {
                PhraseIndex = 0;
                _italXml = value;
                //if (Lang1XmlUpdated != null)
                //    Lang1XmlUpdated();
            }
        }
        /// <summary>
        /// Helper to get the lang1Xml.curSegment
        /// </summary>
        public XmlWavEvent GetLang1Xml_curSegment() {
            if (Lang1Xml == null)
                return null;
            if (PhraseIndex >= Lang1Xml.Segments.Count)
                return null;
            return Lang1Xml.Segments[PhraseIndex];
        }
        /// <summary>
        /// Helper to get the full filename of lang1Xml.curSegment field
        /// </summary>
        public string GetLang1Xml_curSegmentFilename() {
            var l1seg = GetLang1Xml_curSegment();
            if (l1seg == null)
                return null;
            return AudioBooker.classes.Utils.GetFullPathWithoutExtension(Lang1Mp3) + "/" + l1seg.Filename;
        }

        #endregion
    }

    public enum LogicShitState {
        Idle = 1,
        Recording = 2,
        Paused = 3,
    }
     */
}
