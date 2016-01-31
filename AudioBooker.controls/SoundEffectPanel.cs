using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Audiobooker.controls
{
    public partial class SoundEffectPanel: UserControl
    {
        private string key;

        public SoundEffectPanel()
        {
            InitializeComponent();

            // drag drop
            txtFilename.ReadOnly = true;
            txtFilename.AllowDrop = true;
            txtFilename.DragEnter += file_DragEnter;
            txtFilename.DragDrop += file_DragDrop;
        }

        public delegate void FileUpdatedHandler(string key, string filename);
        public event FileUpdatedHandler FileUpdated;

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
            if (Path.GetExtension(file) != ".mp3") {
                MessageBox.Show("File must be an mp3!", "Mp3 only!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Mp3Filename = file;
        }

        #region properties

        private string _mp3Filename;
        public string Mp3Filename {
            get { return _mp3Filename; }
            set {
                _mp3Filename = value;
                txtFilename.Text = Path.GetFileName(value);
                if (FileUpdated != null)
                    FileUpdated(key, value);
            }
        }
        public string KeyNumber {
            set {
                key = value;
                lblKey.Text = "#" + value;
            }
        }

        #endregion properties
    }
}
