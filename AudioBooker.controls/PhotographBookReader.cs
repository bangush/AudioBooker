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
using AudioBooker.classes;
using Miktemk;

namespace Audiobooker.controls
{
    public partial class PhotographBookReader : UserControl
    {
        public static readonly Pen penDefault = new Pen(Color.Black, 1); // new Pen(Color.FromArgb(255, 30, 30, 30), 1);
        public static readonly Brush brushDefault = new SolidBrush(Color.Black);
        public static readonly Brush brushWhite = new SolidBrush(Color.White);
        public static readonly Font fontDefault = SystemFonts.DefaultFont;

        private const int LAYOUT_PERCENT_MARGIN_LEFT = 20;
        private const int LAYOUT_PERCENT_INIT_WINDOW_WIDTH = 40;
        private const double ZoomDelta = 0.9;

        private Image pageImage;
        private Rectangle rectDest, rectSrc;
        private int PixelDeltaX = 100;
        private int PixelDeltaY = 50;
        private double zoomFactor = 0.5;
        private string[] filenamesAll = null;
        private int curIndex = -1;

        public StringHandler PageLoaded;

        public PhotographBookReader()
        {
            InitializeComponent();
            rectDest = new Rectangle();
            rectSrc = new Rectangle();
        }

        private void PhotographBookReader_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            if (pageImage == null)
            {
                g.FillRectangle(brushWhite, 0, 0, Width, Height);
                g.DrawString("Drag an image you stole from the library", fontDefault, brushDefault, Width/2, Height/2);
                return;
            }
            rectDest.X = 0;
            rectDest.Y = 0;
            rectDest.Width = Width;
            rectDest.Height = Height;
            e.Graphics.DrawImage(pageImage, rectDest, rectSrc, GraphicsUnit.Pixel);
        }

        private void PhotographBookReader_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var firstFile = files.FirstOrDefault();
                if (UtilsCore.IsFilenameImage(firstFile))
                    e.Effect = DragDropEffects.Copy;
                //string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            }
        }

        private void PhotographBookReader_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var firstFile = files.FirstOrDefault();
                LoadImage(firstFile);
            }
        }

        public void LoadImage(string firstFile)
        {
            if (!File.Exists(firstFile))
                return;

            loadPage(firstFile);

            filenamesAll = Directory.GetFiles(Path.GetDirectoryName(firstFile))
                .Where(f => UtilsCore.IsFilenameImage(f))
                .OrderBy(f => f)
                .ToArray();
            curIndex = filenamesAll.IndexOf(firstFile, (x, y) => x == y);
        }

        private void loadPage(string filename)
        {
            pageImage = UtilsUi.LoadImageWithExifOrientation(filename);

            PixelDeltaX = pageImage.Width / 50;
            PixelDeltaY = pageImage.Height / 10;
            frameToDefaultLeft();
            Invalidate();
            if (PageLoaded != null)
                PageLoaded(filename);
        }

        public void keyPressed(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Left)
                gotoPage(curIndex - 1);
            else if (e.Control && e.KeyCode == Keys.Right)
                gotoPage(curIndex + 1);
            else if (e.KeyCode == Keys.Home)
                gotoPage(0);
            else if (e.KeyCode == Keys.End)
                gotoPage((filenamesAll != null) ? filenamesAll.Length - 1 : 0);
            else if (e.KeyCode == Keys.OemOpenBrackets)
                frameToDefaultLeft();
            else if (e.KeyCode == Keys.OemCloseBrackets)
                frameToDefaultRight();
            else if (e.KeyCode == Keys.P)
                frameToPanoramic();
            else if (e.KeyCode == Keys.Down)
            {
                if (SrcRectAlreadyOutsideAtBottom())
                {
                    if (SrcRectOnLeftSide())
                        frameToDefaultRight();
                    else
                        gotoPage(curIndex + 1);
                }
                else
                    frameDelta(0, PixelDeltaY);
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (SrcRectAlreadyAtTop())
                {
                    if (!SrcRectOnLeftSide())
                        frameToDefaultLeft();
                    else
                    {
                        gotoPage(curIndex - 1);
                        frameToDefaultRight();
                    }
                    frameToBottom();
                }
                else
                    frameDelta(0, -PixelDeltaY);
            }
            else if (e.KeyCode == Keys.Left)
                frameDelta(-PixelDeltaX, 0);
            else if (e.KeyCode == Keys.Right)
                frameDelta(PixelDeltaX, 0);
            else if (e.KeyCode == Keys.OemMinus)
                zoom(1 / ZoomDelta);
            else if (e.KeyCode == Keys.Oemplus)
                zoom(ZoomDelta);
            else
                return;
            Invalidate();
        }

        private void gotoPage(int newIndex)
        {
            if (filenamesAll == null)
                return;
            curIndex = newIndex;
            if (curIndex < 0)
                curIndex = 0;
            if (curIndex > filenamesAll.Length - 1)
                curIndex = filenamesAll.Length - 1;
            DisposeCurImage();
            loadPage(filenamesAll[curIndex]);
        }

        private void DisposeCurImage()
        {
            if (pageImage == null)
                return;
            pageImage.Dispose();
            pageImage = null;
        }

        private bool SrcRectAlreadyOutsideAtBottom()
        {
            return (rectSrc.Y + rectSrc.Height > pageImage.Height);
        }

        private bool SrcRectAlreadyAtTop()
        {
            return (rectSrc.Y == 0);
        }

        private bool SrcRectOnLeftSide()
        {
            return (rectSrc.X + rectSrc.Width / 2 < pageImage.Width / 2);
        }

        private void zoom(double zDelta)
        {
            zoomFactor *= zDelta;
            var prevW = rectSrc.Width;
            var prevH = rectSrc.Height;

            // keep this still centered on the doggamn center point
            updateSrcRectWHFromZoomFactor();
            rectSrc.X -= (rectSrc.Width - prevW) / 2;
            rectSrc.Y -= (rectSrc.Height - prevH) / 2;
        }

        private void frameDelta(int dx, int dy)
        {
            rectSrc.X += dx;
            rectSrc.Y += dy;
            if (rectSrc.X < 0)
                rectSrc.X = 0;
            if (rectSrc.Y < 0)
                rectSrc.Y = 0;
        }

        private void frameToDefaultLeft()
        {
            if (pageImage == null)
                return;
            zoomFactor = LAYOUT_PERCENT_INIT_WINDOW_WIDTH / 100.0;
            var centerX = (50 + LAYOUT_PERCENT_MARGIN_LEFT) / 2;
            rectSrc.X = pageImage.Width * (centerX - LAYOUT_PERCENT_INIT_WINDOW_WIDTH / 2) / 100;
            rectSrc.Y = 0;
            updateSrcRectWHFromZoomFactor();
        }

        private void frameToDefaultRight()
        {
            if (pageImage == null)
                return;
            zoomFactor = LAYOUT_PERCENT_INIT_WINDOW_WIDTH / 100.0;
            var centerX = (150 - LAYOUT_PERCENT_MARGIN_LEFT) / 2;
            rectSrc.X = pageImage.Width * (centerX - LAYOUT_PERCENT_INIT_WINDOW_WIDTH / 2) / 100;
            rectSrc.Y = 0;
            updateSrcRectWHFromZoomFactor();
        }

        /// <summary>
        /// Called after frameToDefaultLeft or frameToDefaultRight
        /// </summary>
        private void frameToBottom()
        {
            if (pageImage == null)
                return;
            rectSrc.Y = pageImage.Height - rectSrc.Height;
        }


        private void frameToPanoramic()
        {
            if (pageImage == null)
                return;
            zoomFactor = 1;
            rectSrc.X = 0;
            rectSrc.Y = 0;
            rectSrc.Width = pageImage.Width;
            updateSrcRectWHFromZoomFactor();
        }

        private void updateSrcRectWHFromZoomFactor()
        {
            rectSrc.Width = (int)(pageImage.Width * zoomFactor);
            computeSrcRectHeightBasedOnWidth();
        }
        private void computeSrcRectHeightBasedOnWidth()
        {
            if (Width == 0)
                return;
            rectSrc.Height = (int)(rectSrc.Width * Height / Width);
        }

        private void PhotographBookReader_Resize(object sender, EventArgs e)
        {
            computeSrcRectHeightBasedOnWidth();
            Invalidate();
        }

        private void PhotographBookReader_Load(object sender, EventArgs e) {}
    }
}
