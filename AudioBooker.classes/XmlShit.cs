using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AudioBooker.classes {

    #region audiobook

    /// <summary>
    /// a single wav phrase recorded in its own file. TimeIn used for ordering, nothing more
    /// </summary>
    public class XmlWavEvent
    {
        public WavEventType Type { get; set; }
        public string Filename { get; set; }
        [XmlIgnore]
        public TimeSpan TimeIn { get; set; }
        [XmlIgnore]
        public TimeSpan TimeOut { get; set; }

        #region custom serialization

        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = "TimeIn")]
        public string TimeIn_str {
            get {
                return XmlConvert.ToString(TimeIn);
            }
            set {
                TimeIn = string.IsNullOrEmpty(value)
                    ? TimeSpan.Zero
                    : XmlConvert.ToTimeSpan(value);
            }
        }
        [Browsable(false)]
        [XmlElement(DataType = "duration", ElementName = "TimeOut")]
        public string TimeOut_str {
            get {
                return XmlConvert.ToString(TimeOut);
            }
            set {
                TimeOut = string.IsNullOrEmpty(value)
                    ? TimeSpan.Zero
                    : XmlConvert.ToTimeSpan(value);
            }
        } 
        #endregion
    }

    public enum WavEventType : int {
        WavRecording1 = 1, // from Audiobooker
        WavRecording2 = 2, // from session 2, AKA Ilya-franker
        Mp3Segment = 3,    // used Ilya-franker's mp3 splitter
    }

    public class XmlAudiobook {

        public XmlAudiobook() {
            Segments = new List<XmlWavEvent>();
            SoundEffects = new List<XmlWavEvent>();
        }

        public List<XmlWavEvent> Segments { get; set; }
        public List<XmlWavEvent> SoundEffects { get; set; }

        public override string ToString() {
            var shit = Segments.Union(SoundEffects)
                .OrderBy(x => x.TimeIn)
                .Select(x => String.Format("{0}: {1}", x.Filename, x.TimeIn.ToTsString()));
            return string.Join("\n", shit);
        }
    }

    #endregion

    #region ilya-frank

    public class XmlIlyaFrankAbook {
        public List<XmlIlyaParagraph> Paragraphs { get; set; }
        public string Lang1Mp3Filename { get; set; }
        public string Lang1Prefix { get; set; }
        public string Lang2Prefix { get; set; }
        public XmlIlyaFrankAbook() {
            Paragraphs = new List<XmlIlyaParagraph>();
        }
    }
    public class XmlIlyaParagraph {
        public List<XmlIlyaSentence> Sentences { get; set; }
        public XmlIlyaParagraph() {
            Sentences = new List<XmlIlyaSentence>();
        }
    }
    /// <summary>
    /// Once and for all:
    ///  - lang1 is the language we drag in.
    ///  - lang2 is the language we record
    /// </summary>
    public class XmlIlyaSentence {
        public List<XmlWavEvent> Lang1Segments { get; set; }
        public List<XmlWavEvent> Lang2Segments { get; set; }
        public XmlIlyaSentence() {
            Lang1Segments = new List<XmlWavEvent>();
            Lang2Segments = new List<XmlWavEvent>();
        }
    }

    //public enum XmlAbookEventType {
    //    OneSentence = 1,
    //    PurgeSentences = 2,
    //}
    //public class XmlAbookEvent {
    //    public List<XmlWavEvent> Lang1Segments { get; set; }
    //    public List<XmlWavEvent> Lang2Segments { get; set; }
    //    public XmlAbookEventType EventType { get; set; }

    //    public XmlAbookEvent() {
    //        Lang1Segments = new List<XmlWavEvent>();
    //        Lang2Segments = new List<XmlWavEvent>();
    //    }
    //}

    //public class XmlAbookEvent_OneSentence : XmlAbookEvent {
    //    public XmlWavEvent ItalianSegment { get; set; }
    //    public XmlAbookEvent_OneSentence() : base() {
    //        EventType = XmlAbookEventType.OneSentence;
    //    }
    //}
    //public class XmlAbookEvent_PurgeSentences : XmlAbookEvent {
    //    public XmlAbookEvent_PurgeSentences() : base() {
    //        EventType = XmlAbookEventType.PurgeSentences;
    //    }
    //}

    #endregion

    #region audiobook old - delete later

    //public class XmlAudiobook {
    //    [XmlIgnore]
    //    public XmlSegment CurSegment { get; set; }

    //    public XmlAudiobook() {
    //        Segments = new List<XmlSegment>();
    //        SoundEffects = new List<XmlSoundEffect>();
    //    }

    //    public override string ToString() {
    //        var sb = new StringBuilder();
    //        var shit = Segments.Select(x => new ToStringTuple {
    //            //Text = String.Format("{0}: {1}-{2} {3}", "Segment", x.TimeIn.ToTsString(), x.TimeOut.ToTsString(), Utils.SimulateLength(x.TimeIn, x.TimeOut)),
    //            Text = String.Format("{0}: {1}-{2} {3} {4}", "Segment", 0, 1, x.Filename, Utils.SimulateLength(1)),
    //            TimeIn = x.TimeIn,
    //        }).Union(SoundEffects.Select(x => new ToStringTuple {
    //            Text = String.Format("{0}: {1}", x.Filename, x.TimeIn.ToTsString()),
    //            TimeIn = x.TimeIn,
    //        })).OrderBy(x => x.TimeIn);

    //        foreach (var sss in shit) {
    //            sb.Append(sss.Text + "\n");
    //        }
    //        return sb.ToString();
    //    }
    //    private class ToStringTuple {
    //        public TimeSpan TimeIn { get; set; }
    //        public string Text { get; set; }
    //    }

    //    // --------- xml shit ----------------
    //    public List<XmlSegment> Segments { get; set; }
    //    public List<XmlSoundEffect> SoundEffects { get; set; }
    //}

    //public enum XmlSegmentState {
    //    Untouched = 1,
    //    Pending = 2,
    //    Processed = 3,
    //}

    //public class XmlSegment {

    //    [XmlIgnore]
    //    public XmlSegmentState ProcessedState { get; set; }

    //    public XmlSegment() {
    //        ProcessedState = XmlSegmentState.Untouched;
    //    }

    //    // --------- xml shit ----------------
    //    public string Filename { get; set; }

    //    //[XmlIgnore]
    //    //public TimeSpan TimeIn { get; set; }
    //    //[XmlIgnore]
    //    //public TimeSpan TimeOut { get; set; }
    //    //[Browsable(false)]
    //    //[XmlElement(DataType = "duration", ElementName = "TimeIn")]
    //    //public string TimeIn_str {
    //    //    get {
    //    //        return XmlConvert.ToString(TimeIn);
    //    //    }
    //    //    set {
    //    //        TimeIn = string.IsNullOrEmpty(value)
    //    //            ? TimeSpan.Zero
    //    //            : XmlConvert.ToTimeSpan(value);
    //    //    }
    //    //}
    //    //[Browsable(false)]
    //    //[XmlElement(DataType = "duration", ElementName = "TimeOut")]
    //    //public string TimeOut_str {
    //    //    get {
    //    //        return XmlConvert.ToString(TimeOut);
    //    //    }
    //    //    set {
    //    //        TimeOut = string.IsNullOrEmpty(value)
    //    //            ? TimeSpan.Zero
    //    //            : XmlConvert.ToTimeSpan(value);
    //    //    }
    //    //}
    //}

    //public class XmlSoundEffect {
    //    [XmlIgnore]
    //    public TimeSpan TimeIn { get; set; }
    //    public string Filename { get; set; }

    //    [Browsable(false)]
    //    [XmlElement(DataType = "duration", ElementName = "TimeIn")]
    //    public string TimeIn_str {
    //        get {
    //            return XmlConvert.ToString(TimeIn);
    //        }
    //        set {
    //            TimeIn = string.IsNullOrEmpty(value)
    //                ? TimeSpan.Zero
    //                : XmlConvert.ToTimeSpan(value);
    //        }
    //    }
    //}

    #endregion
}
