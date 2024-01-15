using System.Collections.Generic; // benötigt für Listen

namespace gdi_PointAndClick
{
    public partial class FrmMain : Form
    {
        List<Rectangle> rectangles = new List<Rectangle>();
        List<Brush> rectangleColors = new List<Brush>();

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
        }

        private void FrmMain_MouseClick(object sender, MouseEventArgs e)
        {
            Point mausposition = e.Location;
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