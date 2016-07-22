using AudioBooker.classes;
using AudioBooker.Properties;
using Miktemk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioBooker
{
    public partial class Form_Audiobooker2 : Form
    {
        // ... stolen from vidka editor
        private const float RATIO_sidebarW_of_screenW = 500f / 3200;
        private const float RATIO_windowW_of_screenW = 1 / 1.2f;
        private const float RATIO_windowH_of_screenH = 1 / 1.5f;


        private LogicShit logicShit;

        public string LastFolder { get; set; }

        public Form_Audiobooker2()
        {
            InitializeComponent();
            CustomLayout();

            logicShit = new LogicShit(recordingControls.RecMan);
            LastFolder = Path.GetDirectoryName(Application.ExecutablePath);

            txtConsole.ReadOnly = true;
            txtConsole.BackColor = Color.White;
            txtConsole.TextChanged += (object sender, EventArgs e) =>
            {
                txtConsole.SelectionStart = txtConsole.Text.Length; //Set the current caret position at the end
                txtConsole.ScrollToCaret(); //Now scroll it automatically
            };

            recordingControls.setForm(this);
            recordingControls.setLogic(logicShit);
            recordingControls.PopulateDevices();

            // ... remember and load the image from last session
            photographBookReader.PageLoaded += (string filename) =>
            {
                Settings.Default.LastImageFile = filename;
                Settings.Default.Save();
            };
            if (!String.IsNullOrEmpty(Settings.Default.LastImageFile))
                photographBookReader.LoadImage(Settings.Default.LastImageFile);

            logicShit.CurSegmentUpdated += () =>
            {
                //segmentProgressView.CurSegment = logicShit.CurSegment;
            };
            logicShit.XmlUpdated += () =>
            {
                if (logicShit.AudiobookXml != null)
                {
                    txtConsole.Text = logicShit.AudiobookXml.ToString();
                }
            };
            logicShit.SegmentTimeUpdated += (TimeSpan ts) =>
            {
                //if (logicShit.AudiobookXml != null)
                //{
                //    txtConsole.Text = logicShit.AudiobookXml.ToString();
                //}
            };
            logicShit.RecStopped += () =>
            {
                saveTheShit();
            };
        }

        private void CustomLayout()
        {
            var resolutionW = Screen.PrimaryScreen.Bounds.Width;
            var resolutionH = Screen.PrimaryScreen.Bounds.Height;
            var sidebarW = (int)(resolutionW * RATIO_sidebarW_of_screenW);

            var panelLeft = new Panel()
            {
                Dock = DockStyle.Right,
                MinimumSize = new Size(sidebarW, 400),
            };
            var panelControlHolder = new Panel()
            {
                Dock = DockStyle.Top,
                MinimumSize = new Size(sidebarW, 200),
            };
            this.Controls.Remove(recordingControls);
            this.Controls.Remove(txtConsole);
            panelLeft.Controls.Add(txtConsole);
            panelLeft.Controls.Add(panelControlHolder);
            panelControlHolder.Controls.Add(recordingControls);
            txtConsole.Dock = DockStyle.Fill;
            recordingControls.Dock = DockStyle.Fill;
            photographBookReader.Dock = DockStyle.Fill;
            this.Controls.Add(panelLeft);
            this.Width = (int)(resolutionW * RATIO_windowW_of_screenW);
            this.Height = (int)(resolutionH * RATIO_windowH_of_screenH);
        }

        private void Form_Audiobooker2_Load(object sender, EventArgs e) {}

        private int kkk = 1;
        private void Form_Audiobooker2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up ||
                e.KeyData == Keys.Down ||
                e.KeyData == Keys.Left ||
                e.KeyData == Keys.Right)
                return;
            logicShit.keyPressed(e);
            photographBookReader.keyPressed(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                //case Keys.Control | Keys.Left:
                //case Keys.Control | Keys.Right:
                //case Keys.Shift | Keys.Left:
                //case Keys.Shift | Keys.Right:
                    var e = new KeyEventArgs(keyData);
                    logicShit.keyPressed(e);
                    photographBookReader.keyPressed(e);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region other shit

        private void saveTheShit()
        {
            var fff = new SaveFileDialog()
            {
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

            UtilsCore.OpenWinExplorerAndSelectThisFile(fff.FileName);
        }

        #endregion

    }
}
