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
using Miktemk;

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
            txtArea.TextChanged += (object sender, EventArgs e) =>
            {
                txtArea.SelectionStart = txtArea.Text.Length; //Set the current caret position at the end
                txtArea.ScrollToCaret(); //Now scroll it automatically
            };

            recordingControls.setForm(this);
            recordingControls.PopulateDevices();

            logicShit.CurSegmentUpdated += () =>
            {
                segmentProgressView.CurSegment = logicShit.CurSegment;
            };
            logicShit.XmlUpdated += () =>
            {
                if (logicShit.AudiobookXml != null)
                {
                    txtArea.Text = logicShit.AudiobookXml.ToString();
                }
            };
            logicShit.RecStopped += () =>
            {
                saveTheShit();
            };

#if (DEBUG_FNAMES)
            webBrowserControl.Url = new Uri(@"file:///C:/Users/mtemkine/Documents/mikhailshit/books/Pars_vite_et_reviens_tard_-_VargasFred.pdf");
#endif
        }

        public string LastFolder { get; set; }

        #region buttons and shit

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
                logicShit.addSoundEffect(soundEffectPanel1.Mp3Filename);
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
                logicShit.addSoundEffect(soundEffectPanel2.Mp3Filename);
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
                logicShit.addSoundEffect(soundEffectPanel3.Mp3Filename);
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
                logicShit.addSoundEffect(soundEffectPanel4.Mp3Filename);
            else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
                logicShit.addSoundEffect(soundEffectPanel5.Mp3Filename);
            else if (e.KeyCode == Keys.D6 || e.KeyCode == Keys.NumPad6)
                logicShit.addSoundEffect(soundEffectPanel6.Mp3Filename);
            else if (e.KeyCode == Keys.D7 || e.KeyCode == Keys.NumPad7)
                logicShit.addSoundEffect(soundEffectPanel7.Mp3Filename);
            else if (e.KeyCode == Keys.D8 || e.KeyCode == Keys.NumPad8)
                logicShit.addSoundEffect(soundEffectPanel8.Mp3Filename);
            else if (e.KeyCode == Keys.D9 || e.KeyCode == Keys.NumPad9)
                logicShit.addSoundEffect(soundEffectPanel9.Mp3Filename);
            else if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.NumPad0)
                logicShit.addSoundEffect(soundEffectPanel10.Mp3Filename);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(LastFolder);
        }

        #endregion

        #region other shit

        private void setupSoundEffectsPanel(Audiobooker.controls.SoundEffectPanel sp, string key) {
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
