//#define DEBUG_FNAMES

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
using Mp3SplitterCommon;

namespace AudioBooker {
    public partial class Form_Audiobooker : Form {

        private LogicShit logicShit;

        public Form_Audiobooker() {
            InitializeComponent();

            logicShit = new LogicShit(recordingControls.RecMan);
            LastFolder = Path.GetDirectoryName(Application.ExecutablePath);

            setupSoundEffectsPanel(soundEffectPanel1, "1");
            setupSoundEffectsPanel(soundEffectPanel2, "2");
            setupSoundEffectsPanel(soundEffectPanel3, "3");
            setupSoundEffectsPanel(soundEffectPanel4, "4");
            setupSoundEffectsPanel(soundEffectPanel5, "5");
            setupSoundEffectsPanel(soundEffectPanel6, "6");
            setupSoundEffectsPanel(soundEffectPanel7, "7");
            setupSoundEffectsPanel(soundEffectPanel8, "8");
            setupSoundEffectsPanel(soundEffectPanel9, "9");
            setupSoundEffectsPanel(soundEffectPanel10, "0");

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            txtArea.ReadOnly = true;
            txtArea.BackColor = Color.White;

            recordingControls.setForm(this);
            recordingControls.RecStarted += RecStarted;
            recordingControls.RecStopped += RecStopped;
            recordingControls.PopulateDevices();

            logicShit.CurSegmentUpdated += () => {
                segmentProgressView.CurSegment = logicShit.CurSegment;
            };

#if (DEBUG_FNAMES)
            webBrowserControl.Url = new Uri(@"file:///C:/Users/mtemkine/Documents/mikhailshit/books/Pars_vite_et_reviens_tard_-_VargasFred.pdf");
#endif
        }

        public string LastFolder { get; set; }

        #region buttons and shit

        private void RecStarted() {
            logicShit.startRecording();
        }

        private void RecStopped() {
            logicShit.stopRecording();
            UpdateXmlString();
            // TODO: use a button?
            saveTheShit();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Space:
                case Keys.Enter:
                    logicShit.commitSegment();
                    break;
                case Keys.Back:
                    logicShit.rollbackSegment();
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    logicShit.addSoundEffect(soundEffectPanel1.Mp3Filename);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    logicShit.addSoundEffect(soundEffectPanel2.Mp3Filename);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    logicShit.addSoundEffect(soundEffectPanel3.Mp3Filename);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    logicShit.addSoundEffect(soundEffectPanel4.Mp3Filename);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    logicShit.addSoundEffect(soundEffectPanel5.Mp3Filename);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    logicShit.addSoundEffect(soundEffectPanel6.Mp3Filename);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    logicShit.addSoundEffect(soundEffectPanel7.Mp3Filename);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    logicShit.addSoundEffect(soundEffectPanel8.Mp3Filename);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    logicShit.addSoundEffect(soundEffectPanel9.Mp3Filename);
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    logicShit.addSoundEffect(soundEffectPanel10.Mp3Filename);
                    break;
                default:
                    break;
            }
            UpdateXmlString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(LastFolder);
        }

        #endregion

        #region other shit

        private void UpdateXmlString() {
            txtArea.Text = logicShit.AudiobookXml.ToString();
            txtArea.SelectionStart = txtArea.Text.Length;
            txtArea.ScrollToCaret();
        }

        private void setupSoundEffectsPanel(MyControlz.SoundEffectPanel sp, string key) {
            sp.KeyNumber = key;
            sp.FileUpdated += SoundEffectsFileUpdated;
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            var filename = config.AppSettings.Settings["SoundEffectFile-" + key];
            if (filename != null) {
                if (File.Exists(filename.Value))
                    sp.Mp3Filename = filename.Value;
            }
        }

        private void SoundEffectsFileUpdated(string id, string filename) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.AddOrUpdate("SoundEffectFile-" + id, filename);
            config.Save(ConfigurationSaveMode.Minimal);
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
            XmlFactory.WriteToFile<XmlAudiobook>(logicShit.AudiobookXml, fff.FileName + ".xml");
            
            // save wav
            AudiobookerMp3Utils.SaveMusicMp3ToSegmentedFile(fff.FileName, logicShit.AudiobookXml);
        }

        #endregion

        #region sound effects panel loads
        private void soundEffectPanel1_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel5_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel2_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel6_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel4_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel3_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel7_Load(object sender, EventArgs e) {

        }

        private void soundEffectPanel10_Load(object sender, EventArgs e) {

        } 
        #endregion

    }
}
