using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AudioBooker.classes;
using Mp3SplitterCommon;
using NAudio.Wave;

namespace IlyaFranker {

    public class LogicState {
        public const string Ready = "ready";
        public const string DeqedLang1 = "DeqedLang1";
        public const string DeqingLang1 = "DeqingLang1";
        public const string Recording1st = "Recording1st";
        public const string RecordingMore = "RecordingMore";
        public const string CommitedParagraph = "CommitedParagraph";
    }

    public enum SegmentRecordingType : int {
        None = 0,
        Session1 = 1,
        Session2 = 2,
    }

    public class LogicShit {
        private const string DumpFolder = "____temp";

        private FSM<Keys> fsmWithXml;
        private FSM<Keys> fsmManualSplit;
        private FSM<Keys> curFsm;

        //private XmlWavEvent CurSegment, lang1Xml_curSegment;
        private int segmentId = 0;
        private XmlIlyaSentence curSentence;
        private AudioRecManager recMan;
        //private List<XmlWavEvent> pendingRussian;

        public LogicShit(AudioRecManager recMan) {
            this.recMan = recMan;
            FilenameFormat = DumpFolder + "/segment_{0}.wav";

            #region FSM with XML

            fsmWithXml = new FSM<Keys>()
                .addNode_0_(LogicState.Ready)
                .addEdge___(Keys.Enter, LogicState.DeqedLang1, (args) => {
                    IlyaXml = new XmlIlyaFrankAbook() {
                        Lang1Mp3Filename = Lang1Mp3,
                        Lang1Prefix = AudioBooker.classes.Utils.GetFullPathWithoutExtension(Lang1Mp3)
                    };
                    new_paragraph();
                    nextLang1Phrase();
                    readCurLang1Phrase();
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.DeqedLang1)
                .addEdge___(Keys.R, LogicState.DeqedLang1, (args) => readCurLang1Phrase())
                .addEdge___(Keys.P, LogicState.DeqedLang1, (args) => {
                    purgeParaAllExceptLast();
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.Space, LogicState.Recording1st, (args) => {
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.Recording1st)
                .addEdge___(Keys.R, LogicState.Recording1st, (args) => readCurLang1Phrase())
                .addEdge___(Keys.P, LogicState.DeqedLang1, (args) => {
                    purgeParaAllExceptLast();
                    stopAndDeleteSegment();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.Back, LogicState.Recording1st, (args) => {
                    stopAndDeleteSegment();
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Space, LogicState.RecordingMore, (args) => {
                    commitSegment();
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addNode_0_(LogicState.RecordingMore)
                .addEdge___(Keys.R, LogicState.RecordingMore, (args) => readCurLang1Phrase())
                .addEdge___(Keys.Back, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Space, LogicState.RecordingMore, (args) => {
                    commitSegment();
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.D1, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    RecordedSegments = CurIlyaSentence.Lang1Segments;
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.D2, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    RecordedSegments = CurIlyaSentence.Lang2Segments;
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Enter, LogicState.DeqedLang1, (args) => {
                    stopAndDeleteSegment();
                    nextLang1Phrase();
                    readCurLang1Phrase();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.P, LogicState.CommitedParagraph, (args) => {
                    stopAndDeleteSegment();
                    new_paragraph();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.CommitedParagraph)
                .addEdge___(Keys.Enter, LogicState.DeqedLang1, (args) => {
                    nextLang1Phrase();
                    readCurLang1Phrase();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
            ;
            fsmWithXml.Reset();

            #endregion

            #region FSM manual split

            fsmManualSplit = new FSM<Keys>()
                .addNode_0_(LogicState.Ready)
                .addEdge___(Keys.Enter, LogicState.DeqingLang1, (args) => {
                    IlyaXml = new XmlIlyaFrankAbook() {
                        Lang1Mp3Filename = Path.GetFileName(Lang1Mp3),
                        Lang1Prefix = Path.GetDirectoryName(Lang1Mp3)
                    };
                    new_paragraph();
                    AudioPlaya = new AudioPlaya(Lang1Mp3);
                    AudioPlaya.Play();
                })
                .addNode_0_(LogicState.DeqingLang1)
                .addEdge___(Keys.OemPeriod, LogicState.DeqingLang1, (args) => {
                    AudioPlaya.Pause();
                    AudioPlaya.Position += TimeSpan.FromSeconds(5);
                    AudioPlaya.Play();
                })
                .addEdge___(Keys.Oemcomma, LogicState.DeqingLang1, (args) => {
                    AudioPlaya.Pause();
                    AudioPlaya.Position -= TimeSpan.FromSeconds(5);
                    AudioPlaya.Play();
                })
                .addEdge___(Keys.Space, LogicState.DeqingLang1, (args) => {
                    ManualSegmentA = AudioPlaya.Position;
                })
                .addEdge___(Keys.Enter, LogicState.DeqedLang1, (args) => {
                    ManualSegmentB = AudioPlaya.Position;
                    AudioPlaya.Pause();
                    nextLang1Phrase_manual();
                    readCurLang1Phrase();
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.DeqedLang1)
                .addEdge___(Keys.R, LogicState.DeqedLang1, (args) => readCurLang1Phrase())
                .addEdge___(Keys.P, LogicState.DeqedLang1, (args) => {
                    purgeParaAllExceptLast();
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.Space, LogicState.Recording1st, (args) => {
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.Recording1st)
                .addEdge___(Keys.R, LogicState.Recording1st, (args) => readCurLang1Phrase())
                .addEdge___(Keys.P, LogicState.DeqedLang1, (args) => {
                    purgeParaAllExceptLast();
                    stopAndDeleteSegment();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.Back, LogicState.Recording1st, (args) => {
                    stopAndDeleteSegment();
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Space, LogicState.RecordingMore, (args) => {
                    commitSegment();
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addNode_0_(LogicState.RecordingMore)
                .addEdge___(Keys.R, LogicState.RecordingMore, (args) => readCurLang1Phrase())
                .addEdge___(Keys.Back, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    startSegment();
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Space, LogicState.RecordingMore, (args) => {
                    commitSegment();
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.D1, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    RecordedSegments = CurIlyaSentence.Lang1Segments;
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.D2, LogicState.RecordingMore, (args) => {
                    stopAndDeleteSegment();
                    RecordedSegments = CurIlyaSentence.Lang2Segments;
                    startSegment();
                    FireHandler(CurParagraphUpdated);
                    FireHandler(CurSegmentUpdated);
                })
                .addEdge___(Keys.Enter, LogicState.DeqingLang1, (args) => {
                    stopAndDeleteSegment();
                    ManualSegmentA = AudioPlaya.Position;
                    AudioPlaya.Play();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addEdge___(Keys.P, LogicState.CommitedParagraph, (args) => {
                    stopAndDeleteSegment();
                    new_paragraph();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
                .addNode_0_(LogicState.CommitedParagraph)
                .addEdge___(Keys.Enter, LogicState.DeqingLang1, (args) => {
                    ManualSegmentA = AudioPlaya.Position;
                    AudioPlaya.Play();
                    FireHandler(CurSegmentUpdated);
                    FireHandler(CurParagraphUpdated);
                })
            ;
            fsmManualSplit.Reset();

            #endregion
        }

        /// <summary>
        /// Calling this puts the FSM into the Ready state
        /// </summary>
        public void SetAudioAndMeta(string lang1Mp3, XmlAudiobook lang1Xml) {
            Lang1Mp3 = lang1Mp3;
            Lang1Xml = lang1Xml;
            curFsm = fsmManualSplit; // TODO: change fsm for w/o XML
            if (Lang1Xml != null) {
                curFsm = fsmWithXml; 
                Lang1XmlQueue = Lang1Xml.Segments;
            }
            curFsm.Goto(LogicState.Ready);
        }

        /// <summary>
        /// Called with each keystroke
        /// </summary>
        public void EatCommand(Keys action) {
            if (curFsm == null)
                return;
            curFsm.Advance(action);
        }

        #region events
        public delegate void VoidHandler();
        public event VoidHandler CurSegmentUpdated;
        //public event VoidHandler CurSentenceUpdated;
        public event VoidHandler CurParagraphUpdated;
        //public event VoidHandler Lang1XmlUpdated;
        public event VoidHandler PrematureAdvancement;
        private void FireHandler(VoidHandler x) {
            if (x != null)
                x();
        }
        #endregion

        #region fsm helpers

        private void deqNextLang1() {
            Lang1CurSegment = Lang1XmlQueue.FirstOrDefault();
            Lang1XmlQueue = Lang1XmlQueue.Skip(1);
        }
        private void readCurLang1Phrase() {
            if (Lang1CurSegment == null)
                return;
            var fname = Lang1CurSegment.Filename;
            if (fname == null)
                return;
            if (Lang1CurSegment.Type == WavEventType.WavRecording1)
                WavUtils.PlayAllOfFile(AudioBooker.classes.Utils.GetFullPathWithoutExtension(Lang1Mp3) + "/" + fname);
            else
                Mp3Utils.PlayPieceOfAFile(
                    Lang1Mp3,
                    Lang1CurSegment.TimeIn.TotalSeconds,
                    Lang1CurSegment.TimeOut.TotalSeconds);
        }
        private void new_paragraph() {
            CurIlyaParagraph = new XmlIlyaParagraph();
            IlyaXml.Paragraphs.Add(CurIlyaParagraph);
        }
        private void nextLang1Phrase() {
            deqNextLang1();
            CurIlyaSentence = null;
            if (Lang1CurSegment != null) {
                CurIlyaSentence = new XmlIlyaSentence();
                CurIlyaSentence.Lang1Segments.Add(Lang1CurSegment);
                CurIlyaParagraph.Sentences.Add(CurIlyaSentence);
                RecordedSegments = CurIlyaSentence.Lang2Segments;
            }
        }
        private void nextLang1Phrase_manual() {
            Lang1CurSegment = new XmlWavEvent {
                Filename = IlyaXml.Lang1Mp3Filename,
                TimeIn = ManualSegmentA,
                TimeOut = ManualSegmentB,
                Type = WavEventType.Mp3Segment
            };
            if (Lang1CurSegment != null) {
                CurIlyaSentence = new XmlIlyaSentence();
                CurIlyaSentence.Lang1Segments.Add(Lang1CurSegment);
                CurIlyaParagraph.Sentences.Add(CurIlyaSentence);
                RecordedSegments = CurIlyaSentence.Lang2Segments;
            }
        }
        private void startSegment() {
            recMan.StartRecording();
            CurSegment = new XmlWavEvent() {
                //TimeIn = curTime,
                Filename = string.Format(FilenameFormat, segmentId),
                Type = WavEventType.WavRecording2,
            };
        }
        private void commitSegment() {
            recMan.StopRecording();
            recMan.SaveLastRecording(CurSegment.Filename);
            RecordedSegments.Add(CurSegment);
            segmentId++;
            CurSegment = null;
        }
        private void stopAndDeleteSegment() {
            recMan.StopRecording();
            recMan.DisposeOfLastRecording();
            CurSegment = null;
        }
        /// <summary>
        /// oops forgot to commit... commit previously recorded sentences, excluding the last one
        /// </summary>
        private void purgeParaAllExceptLast() {
            if (Lang1CurSegment == null)
                return; //EOF: do nada
            readCurLang1Phrase();
            if (CurIlyaParagraph.Sentences.Count <= 1)
                return; //nothing recorded yet... do nada
            var lastSentence = CurIlyaParagraph.Sentences.Last();
            CurIlyaParagraph.Sentences.Remove(lastSentence);
            new_paragraph();
            CurIlyaParagraph.Sentences.Add(lastSentence);
        }

        #endregion

        #region props

        // ----------- writing
        public string FilenameFormat { get; set; }
        public XmlIlyaFrankAbook IlyaXml { get; private set; }
        public XmlIlyaParagraph CurIlyaParagraph { get; private set; }
        private XmlIlyaSentence CurIlyaSentence { get; set; }
        private List<XmlWavEvent> RecordedSegments { get; set; }
        public XmlWavEvent CurSegment { get; private set; }
        public SegmentRecordingType CurSegmentType { get {
            if (CurIlyaSentence == null)
                return SegmentRecordingType.None;
            if (CurSegment == null)
                return SegmentRecordingType.None;
            if (RecordedSegments == null)
                return SegmentRecordingType.None;
            if (RecordedSegments == CurIlyaSentence.Lang1Segments)
                return SegmentRecordingType.Session1;
            return SegmentRecordingType.Session2;
        } }

        // ----------- reading
        public string Lang1Mp3 { get; private set; }
        public XmlAudiobook Lang1Xml { get; private set; }
        private XmlWavEvent Lang1CurSegment { get; set; }
        private IEnumerable<XmlWavEvent> Lang1XmlQueue { get; set; }

        /// <summary>
        /// Helper to get the full filename of lang1Xml.curSegment field
        /// </summary>
        private string GetLang1Xml_curSegmentFilename() {
            
            return AudioBooker.classes.Utils.GetFullPathWithoutExtension(Lang1Mp3) + "/" + Lang1CurSegment.Filename;
        }

        #endregion

        public void cleanUpFilenames(string filenameBig) {
            var newFolderName = AudioBooker.classes.Utils.GetFullPathWithoutExtension(filenameBig);
            var allSeg1 = IlyaXml.Paragraphs
                .SelectMany(x => x.Sentences)
                .SelectMany(x => x.Lang1Segments);
            var allSeg2 = IlyaXml.Paragraphs
                .SelectMany(x => x.Sentences)
                .SelectMany(x => x.Lang2Segments);
            foreach (var seg in allSeg1) {
                seg.Filename = seg.Filename.Replace(DumpFolder + "/", "");
            }
            foreach (var seg in allSeg2) {
                seg.Filename = seg.Filename.Replace(DumpFolder + "/", "");
            }
            IlyaXml.Lang2Prefix = newFolderName;
            Directory.Move(DumpFolder, newFolderName);
        }

        public AudioPlaya AudioPlaya { get; set; }
        public TimeSpan ManualSegmentA { get; set; }
        public TimeSpan ManualSegmentB { get; set; }
    }
    
}
