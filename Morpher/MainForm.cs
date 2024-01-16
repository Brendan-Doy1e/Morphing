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
    public partial class MainForm : Form
    {
        private ImageWindows destination;
        private ImageWindows source;
        private ImageWindows morphed;

        public MainForm()
        {
            InitializeComponent();

            source = new ImageWindows(ImageBaseType.SOURCE);
            source.MdiParent = this;
            source.Show();

            destination = new ImageWindows(ImageBaseType.DESTINATION);
            destination.MdiParent = this;
            destination.Show();

            morphed = new ImageWindows(ImageBaseType.MORPHED);
            morphed.MdiParent = this;
            morphed.Show();
        }

        public void Reflect(Line line, int origin)
        {
            Line copiedLine = new Line(line.Start.X, line.Start.Y, line.End.X, line.End.Y);

            if (origin == ImageBaseType.SOURCE)
            {
                destination.AddLines(copiedLine);
            }
            else if (origin == ImageBaseType.DESTINATION)
            {
                source.AddLines(copiedLine);
            }

            source.Invalidate();
            destination.Invalidate();
        }



        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void menu_strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
