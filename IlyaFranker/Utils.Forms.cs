using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioBooker.classes;

namespace IlyaFranker {
    public static class UtilsForms {
        public static void PrintXmlAudiobookToTextBoxForFrankie(System.Windows.Forms.RichTextBox txtArea, AudioBooker.classes.XmlIlyaParagraph xmlPara) {
            txtArea.Clear();
            if (xmlPara == null)
                return;
            var shit = xmlPara.Sentences
                .SelectMany(x => x.Lang2Segments)
                .Select(x => new ToStringTuple {
                    Text = String.Format("{0}: {1}", "Segment", x.Filename),
                    TimeIn = x.TimeIn,
                });
            foreach (var sss in shit) {
                txtArea.SelectionColor = sss.SelectionColor;
                txtArea.SelectionBackColor = sss.SelectionBackColor;
                //if (sss.State == XmlSegmentState.Pending)
                //    txtArea.SelectionBackColor = Color.Orange;
                //if (sss.IsCurrent) {
                //    txtArea.SelectionBackColor = Color.Black;
                //    txtArea.SelectionColor = Color.White;
                //}
                txtArea.AppendText(sss.Text + "\n");
            }
        }
        private class ToStringTuple {
            public TimeSpan TimeIn { get; set; }
            public string Text { get; set; }
            public Color SelectionColor { get; set; }
            public Color SelectionBackColor { get; set; }
        }

        public static void PrintXmlAudiobookToTextBoxForFrankie_simple(
            System.Windows.Forms.RichTextBox txtArea,
            AudioBooker.classes.XmlIlyaParagraph xmlPara,
            SegmentRecordingType curRecordingSession
        ) {
            var sb = new StringBuilder();
            if (xmlPara == null)
                return;
            sb.AppendLine("paragraph(" + xmlPara.Sentences.Count + ")");
            foreach (var sen in xmlPara.Sentences)
            {
                var firstOfLang1 = sen.Lang1Segments.FirstOrDefault();
                if (firstOfLang1 != null)
                    sb.AppendLine(" - L1: " + firstOfLang1.Filename);
                sb.Append("     - recorded L2: " + String.Join(", ", sen.Lang2Segments.Select(x => x.Filename)));
                if (sen == xmlPara.Sentences.Last() && curRecordingSession == SegmentRecordingType.Session2)
                    sb.Append(" [RECORDING]");
                sb.AppendNL();
                var lang1Additional = sen.Lang1Segments.Skip(1);
                if (lang1Additional.Count() > 0 || (sen == xmlPara.Sentences.Last() && curRecordingSession == SegmentRecordingType.Session1))
                {
                    sb.Append("     - additional L1: " + String.Join(", ", lang1Additional.Select(x => x.Filename)));
                    if (sen == xmlPara.Sentences.Last() && curRecordingSession == SegmentRecordingType.Session1)
                        sb.Append(" [RECORDING]");
                    sb.AppendNL();
                }
            }
            txtArea.Text = sb.ToString();

            //txtArea.SelectionBackColor = Color.White;
            //txtArea.SelectionColor = Color.Black;
            //txtArea.AppendText(sss.Text + "\n");
        }

        public static void AppendNL(this StringBuilder sb) {
            sb.Append("\n");
        }
    }
}
