#define DEBUG_FNAMES

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using AudioBooker.classes;
using IlyaFranker.Properties;
using Mp3SplitterCommon;

namespace IlyaFranker {
    public partial class Form_Franker : Form {

        private LogicShit logicShit;

        public Form_Franker() {
            InitializeComponent();

            logicShit = new LogicShit(recordingControls.RecMan);
            LastFolder = Path.GetDirectoryName(Application.ExecutablePath);

            txtArea.ReadOnly = true;
            txtArea.BackColor = Color.White;
            txtArea.AllowDrop = true;
            txtArea.DragEnter += file_DragEnter;
            txtArea.DragDrop += file_DragDrop;

            // recording controls
            recordingControls.setForm(this);
            recordingControls.RecStarted += () => {
            };
            recordingControls.RecStopped += () => {
                UpdateXmlString();
                //TODO: use a button?
                saveTheShit();
            };
            recordingControls.PopulateDevices();

            // misc window events
            this.FormClosing += (sender, e) => {
                recordingControls.RecMan.StopRecording();
                recordingControls.RecMan.DisposeOfLastRecording();
            };

            // keyboard input
            this.KeyPreview = true;
            this.KeyDown += (sender, e) => { keyDowwwwwwwwn(e.KeyCode); };
            webBrowserControl.WebBrowserShortcutsEnabled = false;
            webBrowserControl.PreviewKeyDown += (sender, e) => { keyDowwwwwwwwn(e.KeyCode); };

            // logic shit callbacks
            logicShit.CurSegmentUpdated += () => {
                segmentProgressView.CurSegment = logicShit.CurSegment;
                recordingControls.ForceFocusDiversion(); // done more so to repaint the bg gray if PrematureAdvancement was previously triggered
            };
            logicShit.CurParagraphUpdated += () => { UpdateXmlString(); };
            logicShit.PrematureAdvancement += () => { this.BackColor = Color.DarkRed; };

            lblReverse.Text = Settings.Default.FlipLanguages ? "Languages reversed!" : "";
            lblReverse.BorderStyle = BorderStyle.FixedSingle;

#if (DEBUG_FNAMES)
            //setMp3AndXml(@"C:\Users\mtemkine\Desktop\snd\privet.wav");
            setMp3AndXml(@"C:\Users\mtemkine\Music\abooks\librivox\pinocchio\avventurepinocchio_01_collodi.mp3");
            //webBrowserControl.Url = new Uri(@"file:///C:/Users/mtemkine/Documents/mikhailshit/books/Pars_vite_et_reviens_tard_-_VargasFred.pdf");
            //webBrowserControl.Url = new Uri(@"file:///C:/Users/mtemkine/Documents/mikhailshit/books/Уйди скорей и не спеши обратно (fb2)   Либрусек.htm");
#endif
        }

        public string LastFolder { get; set; }

        #region buttons and shit

        private void file_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
                e.Effect = DragDropEffects.All;
            }
        }
        private void file_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var file = files.FirstOrDefault();
            if (file == null)
                return;
            if (Path.GetExtension(file) != ".mp3" && Path.GetExtension(file) != ".wav") {
                MessageBox.Show("File must be an mp3 or wav!", "Mp3 or Wav only!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!File.Exists(file + ".xml")) {
                MessageBox.Show("You need the XML meta file as well", "Xml missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            setMp3AndXml(file);
        }
        private void keyDowwwwwwwwn(Keys keyCode) {
            logicShit.EatCommand(keyCode);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(LastFolder);
        }

        #endregion

        #region other shit

        private void setMp3AndXml(string fileMp3) {
            XmlAudiobook xml = null;
            if (File.Exists(fileMp3 + ".xml"))
                xml = XmlFactory.LoadFromFile<XmlAudiobook>(fileMp3 + ".xml");
            logicShit.SetAudioAndMeta(fileMp3, xml);
        }
        private void UpdateXmlString() {
            UtilsForms.PrintXmlAudiobookToTextBoxForFrankie_simple(
                txtArea, logicShit.CurIlyaParagraph, logicShit.CurSegmentType);
        }

        private void saveTheShit() {
            var fff = new SaveFileDialog() {
                //CheckFileExists = true,
                Filter = "WAV|*.wav",
            };
            var result = fff.ShowDialog();
            if (result != DialogResult.OK)
                return;
            LastFolder = Path.GetDirectoryName(fff.FileName);

            // save xml
            logicShit.cleanUpFilenames(fff.FileName);
            XmlFactory.WriteToFile<XmlIlyaFrankAbook>(logicShit.IlyaXml, fff.FileName + ".xml");

            // save wav
            AudiobookerMp3Utils.InterleaveAndSaveIlyaFrankerMp3(
                fff.FileName,
                logicShit.IlyaXml,
                (Settings.Default.SpeechChange1 != 0) ? (double?)Settings.Default.SpeechChange1 : null,
                (Settings.Default.SpeechChange2 != 0) ? (double?)Settings.Default.SpeechChange2 : null,
                Settings.Default.FlipLanguages,
                (progress) => {
                    progressBar.Value = (int)progress;
                });
        }

        #endregion

    }
}
