using Miktemk.Wpf.Constants;
using Miktemk.Wpf.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Miktemk;
using AudioBooker.classes;
using AudioBooker.Wpf.Code;

namespace AudioBooker.Wpf.Controls
{
    public class PhotographBookReaderControl : UserControl
    {
        //
        //{
        //    if (DraggyImageSequence?.CurImageFilenameFull != null)
        //    {
        //        var bs = new BitmapImage(new Uri(DraggyImageSequence.CurImageFilenameFull, UriKind.Absolute));
        //        var imageRelativeHeight = bs.Height * ActualWidth / bs.Width;
        //        var yOffset = DraggyImageSequence.VerticalScroll * ActualHeight / 4;
        //        g.DrawImage(bs, new Rect(0, -yOffset, ActualWidth, imageRelativeHeight));
        //    }
        //}

        //public static readonly Pen penDefault = new Pen(Color.Black, 1); // new Pen(Color.FromArgb(255, 30, 30, 30), 1);
        //public static readonly Brush brushDefault = new SolidBrush(Color.Black);
        //public static readonly Brush brushWhite = new SolidBrush(Color.White);
        //public static readonly Font fontDefault = SystemFonts.DefaultFont;

        private const int LAYOUT_PERCENT_MARGIN_LEFT = 20;
        private const int LAYOUT_PERCENT_INIT_WINDOW_WIDTH = 40;
        private const double ZoomDelta = 0.9;

        private BitmapImage pageImage;
        private Rect rectDest, rectSrc;
        private int PixelDeltaX = 100;
        private int PixelDeltaY = 50;
        private double zoomFactor = 0.5;
        private string[] filenamesAll = null;
        private int curIndex = -1;
        private FormattedText textDragHere = DrawUtils.GetNormalFormattedText("Drag an image you stole from the library");

        //public StringHandler PageLoaded;

        public PhotographBookReaderControl()
        {
            //InitializeComponent();
            rectDest = new Rect();
            rectSrc = new Rect();
        }

        protected override void OnRender(DrawingContext g)
        {
            base.OnRender(g);
            if (pageImage == null)
            {
                g.DrawRectangle(Brushes.AliceBlue, PPP.penWhite, new Rect(0, 0, ActualWidth, ActualHeight));
                //g.FillRectangle(brushWhite, 0, 0, Width, Height);
                //g.DrawText("Drag an image you stole from the library", fontDefault, brushDefault, Width / 2, Height / 2);
                g.DrawText(textDragHere, new Point(ActualWidth / 2, ActualHeight / 2));
                return;
            }
            rectDest.X = 0;
            rectDest.Y = 0;
            rectDest.Width = Width;
            rectDest.Height = Height;
            g.DrawImage(pageImage, rectDest); //, rectSrc);
        }

        //private void PhotographBookReader_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
        //    {
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //        var firstFile = files.FirstOrDefault();
        //        if (UtilsCore.IsFilenameImage(firstFile))
        //            e.Effect = DragDropEffects.Copy;
        //        //string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //    }
        //}

        //private void PhotographBookReader_DragDrop(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
        //    {
        //        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //        var firstFile = files.FirstOrDefault();
        //        LoadImage(firstFile);
        //    }
        //}

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
            InvalidateVisual();
            //if (PageLoaded != null)
            //    PageLoaded(filename);
        }

        //public void keyPressed(KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.Left)
        //        gotoPage(curIndex - 1);
        //    else if (e.Control && e.KeyCode == Keys.Right)
        //        gotoPage(curIndex + 1);
        //    else if (e.KeyCode == Keys.Home)
        //        gotoPage(0);
        //    else if (e.KeyCode == Keys.End)
        //        gotoPage((filenamesAll != null) ? filenamesAll.Length - 1 : 0);
        //    else if (e.KeyCode == Keys.OemOpenBrackets)
        //        frameToDefaultLeft();
        //    else if (e.KeyCode == Keys.OemCloseBrackets)
        //        frameToDefaultRight();
        //    else if (e.KeyCode == Keys.P)
        //        frameToPanoramic();
        //    else if (e.KeyCode == Keys.Down)
        //    {
        //        if (SrcRectAlreadyOutsideAtBottom())
        //        {
        //            if (SrcRectOnLeftSide())
        //                frameToDefaultRight();
        //            else
        //                gotoPage(curIndex + 1);
        //        }
        //        else
        //            frameDelta(0, PixelDeltaY);
        //    }
        //    else if (e.KeyCode == Keys.Up)
        //    {
        //        if (SrcRectAlreadyAtTop())
        //        {
        //            if (!SrcRectOnLeftSide())
        //                frameToDefaultLeft();
        //            else
        //            {
        //                gotoPage(curIndex - 1);
        //                frameToDefaultRight();
        //            }
        //            frameToBottom();
        //        }
        //        else
        //            frameDelta(0, -PixelDeltaY);
        //    }
        //    else if (e.KeyCode == Keys.Left)
        //        frameDelta(-PixelDeltaX, 0);
        //    else if (e.KeyCode == Keys.Right)
        //        frameDelta(PixelDeltaX, 0);
        //    else if (e.KeyCode == Keys.OemMinus)
        //        zoom(1 / ZoomDelta);
        //    else if (e.KeyCode == Keys.Oemplus)
        //        zoom(ZoomDelta);
        //    else
        //        return;
        //    Invalidate();
        //}

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
            InvalidateVisual();
        }

    }
}
