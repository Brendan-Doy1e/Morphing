using Morpher;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Morpher
{
    public partial class ImageWindows : Form
    {
        private List<Line> lines = new List<Line>();
        protected Line currentLine;
        protected Line selectedLine;
        protected Bitmap backgroundImage;
        protected int type;

        public ImageWindows(int type)
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            this.type = type;
            Text = TypeToString();
        }

        private string TypeToString()
        {
            switch (type)
            {
                case ImageBaseType.SOURCE:
                    return ImageBaseType.SOURCE_STR;
                case ImageBaseType.DESTINATION:
                    return ImageBaseType.DESTINATION_STR;
                case ImageBaseType.MORPHED:
                    return ImageBaseType.MORPHED_STR;
                default:
                    throw new InvalidOperationException("Unknown type");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBackgroundImage(e);
            DrawLines(e);
            DrawCurrentLine(e);
        }

        private void DrawBackgroundImage(PaintEventArgs e)
        {
            if (backgroundImage != null)
            {
                e.Graphics.DrawImage(backgroundImage, 0, 0, ClientSize.Width, ClientSize.Height);
            }
        }

        private void DrawLines(PaintEventArgs e)
        {
            foreach (Line line in lines)
            {
                line.Draw(e);
            }
        }

        private void DrawCurrentLine(PaintEventArgs e)
        {
            currentLine?.Draw(e);
        }




        private void ImageBase_MouseDown(object sender, MouseEventArgs e)
        {
            if (TrySelectLineUnderMouse(e)) return;
            CreateNewLineIfApplicable(e);
        }

        private bool TrySelectLineUnderMouse(MouseEventArgs e)
        {
            foreach (Line line in lines)
            {
                if (line.GetUserIntention(e) != Intention.CREATING_NEW_LINE)
                {
                    selectedLine = line;
                    return true;
                }
            }
            return false;
        }

        private void CreateNewLineIfApplicable(MouseEventArgs e)
        {
            if (backgroundImage != null && e.Button == MouseButtons.Left)
            {
                currentLine = new Line(e.Location.X, e.Location.Y);
            }
        }


        private void ImageBase_MouseMove(object sender, MouseEventArgs e)
        {
            if (UpdateCurrentLine(e) || ResizeSelectedLine(e))
            {
                Refresh();
            }
        }

        private bool UpdateCurrentLine(MouseEventArgs e)
        {
            if (currentLine != null)
            {
                currentLine.UpdateEndPoints(e.Location.X, e.Location.Y);
                return true;
            }
            return false;
        }

        private bool ResizeSelectedLine(MouseEventArgs e)
        {
            if (selectedLine != null)
            {
                selectedLine.Resize(e);
                return true;
            }
            return false;
        }


        private void ImageBase_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ProcessLeftButtonRelease();
            }
            RefreshIfNeeded();
        }

        private void ProcessLeftButtonRelease()
        {
            if (backgroundImage != null && currentLine != null)
            {
                AddCurrentLine();
            }
            else if (selectedLine != null)
            {
                DeselectLine();
            }
        }

        private void AddCurrentLine()
        {
            lines.Add(currentLine);
            ((MainForm)MdiParent).Reflect(currentLine, this.type);
            currentLine = null;
            selectedLine = null;
        }

        private void DeselectLine()
        {
            selectedLine = null;
        }

        private void RefreshIfNeeded()
        {
            if (currentLine != null || selectedLine != null)
            {
                Refresh();
            }
        }


        public void AddLines(Line line)
        {
            lines.Add(line);
        }


        private Bitmap ResizeBitmap(Bitmap originalBitmap, int width, int height)
        {
            if (originalBitmap == null)
            {
                throw new ArgumentNullException(nameof(originalBitmap), "Original bitmap cannot be null.");
            }

            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Width and height must be greater than zero.");
            }

            var resizedBitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedBitmap))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(originalBitmap, new Rectangle(0, 0, width, height));
            }
            return resizedBitmap;
        }


        private void ImageBase_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff|All files|*.*";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap loadedBitmap = new Bitmap(openFileDialog.FileName);

                    backgroundImage = ResizeBitmap(loadedBitmap, ClientSize.Width, ClientSize.Height);

                    Refresh();
                }
            }
        }
    }
}
