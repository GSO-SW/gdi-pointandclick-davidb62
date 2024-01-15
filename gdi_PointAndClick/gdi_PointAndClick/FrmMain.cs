using System.Collections.Generic;
using System.Diagnostics.Metrics; // benötigt für Listen

namespace gdi_PointAndClick
{
    public partial class FrmMain : Form
    {
        List<Rectangle> rectangles = new List<Rectangle>();
        List<Brush> rectangleColors = new List<Brush>();

        List<Rectangle> rectangleCrossings = new List<Rectangle>();
        List<Brush> rectangleCrossingColors = new List<Brush>();

        List<String> StateOfAction = new List<String>();

        public FrmMain()
        {
            InitializeComponent();
            ResizeRedraw = true;
        }

        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            // Hilfsvarablen
            Graphics g = e.Graphics;
            
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            Random ran = new Random();
            int randomR = ran.Next(20, 256);
            int randomG = ran.Next(20, 256);
            int randomB = ran.Next(20, 256);
            // Zeichenmittel
            Brush b = new SolidBrush(Color.FromArgb(randomR, randomG, randomB));

            rectangleColors.Add(b);

            for (int i = 0; i < rectangles.Count; i++)
            {
                g.FillRectangle(rectangleColors[i], rectangles[i]);
            }

            for (int i = 0; i < rectangles.Count; i++)
            {
                for (int ii = 0; ii < rectangles.Count; ii++)
                {
                    if(i != ii)
                    {
                        rectangleCrossings.Add(Rectangle.Intersect(rectangles[i], rectangles[ii]));
                        rectangleCrossingColors.Add(new SolidBrush(Color.FromArgb(
                            ((SolidBrush)rectangleColors[i]).Color.R / 2 + ((SolidBrush)rectangleColors[ii]).Color.R / 2,
                            ((SolidBrush)rectangleColors[i]).Color.G / 2 + ((SolidBrush)rectangleColors[ii]).Color.R / 2,
                            ((SolidBrush)rectangleColors[i]).Color.B / 2 + ((SolidBrush)rectangleColors[ii]).Color.R / 2)));
                    }
                }
            }

            for (int i = 0; i < rectangleCrossings.Count; i++)
            {
                g.FillRectangle(rectangleCrossingColors[i], rectangleCrossings[i]);
            }
        }

        private void FrmMain_MouseClick(object sender, MouseEventArgs e)
        {
            Point mausposition = e.Location;

            if (e.Button == MouseButtons.Left)
            {
                foreach (Rectangle rectangle in rectangles)
                {
                    if (rectangle.Contains(mausposition)) return;
                }

                Rectangle r;
                do
                {
                    Random ran = new Random();
                    int random = ran.Next(20, 81);
                    r = new Rectangle(mausposition.X - random / 2, mausposition.Y - random / 2, random, random);

                } while (rectangles.Contains(r));

                rectangles.Add(r);  // Kurze Variante: rectangles.Add( new Rectangle(...)  );

                Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                foreach (Rectangle rectangle in rectangles)
                {
                    if (rectangle.Contains(mausposition))
                    {
                        rectangleColors.Remove(rectangleColors[rectangles.IndexOf(rectangle)]);
                        rectangles.Remove(rectangle);
                        break;
                    }
                }

                Refresh();
            }
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                rectangles.Clear();
                Refresh();
            }
        }
    }
}