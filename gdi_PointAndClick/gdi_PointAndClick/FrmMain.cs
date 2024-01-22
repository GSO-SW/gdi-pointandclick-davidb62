using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics; // benötigt für Listen

namespace gdi_PointAndClick
{
    public partial class FrmMain : Form
    {
        List<Paintable> rectangles = new List<Paintable>();
        //List<Rectangle> rectangles = new List<Rectangle>();
        //List<Brush> rectangleColors = new List<Brush>();

        List<Rectangle> rectangleCrossings = new List<Rectangle>();
        List<Brush> rectangleCrossingColors = new List<Brush>();

        private int selectedRectangleIndex = -1;

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

            rectangles.Add(new Paintable(new Rectangle(0, 0, 1, 1), new SolidBrush(Color.Green)));

            foreach (var rectangle in rectangles)
            {
                g.FillRectangle(rectangle.Brush, rectangle.Rectangle);
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
                Paintable p = new Paintable(new Rectangle(-1, -1, -1, -1), new SolidBrush(Color.Black));
                foreach (var rectangle in rectangles)
                {
                    if (rectangle.Rectangle.Contains(mausposition)) return;

                    Rectangle r;
                    do
                    {
                        Random ran = new Random();
                        int random = ran.Next(50, 201);
                        r = new Rectangle(mausposition.X - random / 2, mausposition.Y - random / 2, random, random);
                        p = new Paintable(r, new SolidBrush(Color.Green));

                    } while (rectangle.Rectangle.Contains(r));
                }

                if (p.Equals(new Paintable(new Rectangle(-1, -1, -1, -1), new SolidBrush(Color.Black))))
                    rectangles.Add(p);

                UpdateRectangleCrossings();

                Refresh();
            }
            if (e.Button == MouseButtons.Right)
            {
                foreach (var rectangle in rectangles)
                {
                    if (rectangle.Rectangle.Contains(mausposition))
                    {
                        rectangles.Remove(rectangle);
                        break;
                    }
                }
                foreach (Rectangle rectangle in rectangleCrossings)
                {
                    if (rectangle.Contains(mausposition))
                    {
                        rectangleCrossingColors.Remove(rectangleCrossingColors[rectangleCrossings.IndexOf(rectangle)]);
                        rectangleCrossings.Remove(rectangle);
                        break;
                    }
                }

                UpdateRectangleCrossings();

                Refresh();
            }
            if (e.Button == MouseButtons.Middle)
            {
                foreach (var rectangle in rectangles)
                {
                    if (rectangle.Rectangle.Contains(mausposition))
                    {
                        selectedRectangleIndex = rectangles.IndexOf(rectangle);
                        this.Focus(); // Fokussiere das Formular, damit es Tastatureingaben empfängt
                        break;
                    }
                }

                Refresh();
            }
        }
        private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Verhindere, dass der Fokus auf ein Steuerelement verschoben wird (Standardverhalten für Pfeiltasten)
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
        //private void OnKeysArrows(object sender, KeyEventArgs e)
        //{
        //    if (selectedRectangleIndex > -1 && rectangles.Contains(rectangles[selectedRectangleIndex].Rectangle))
        //    {
        //        Rectangle selectedRectangle = rectangles[selectedRectangleIndex].Rectangle;

        //        // Bewege das ausgewählte Rechteck basierend auf den Pfeiltasten
        //        switch (e.KeyCode)
        //        {
        //            case Keys.Up:
        //                selectedRectangle.Y -= 5;
        //                break;
        //            case Keys.Down:
        //                selectedRectangle.Y += 5;
        //                break;
        //            case Keys.Left:
        //                selectedRectangle.X -= 5;
        //                break;
        //            case Keys.Right:
        //                selectedRectangle.X += 5;
        //                break;
        //            default:
        //                break;
        //        }
        //        rectangles[selectedRectangleIndex].Rectangle = selectedRectangle;

        //        this.Refresh();
        //    }
        //}

        private void UpdateRectangleCrossings()
        {
            rectangleCrossings.Clear();
            rectangleCrossingColors.Clear();

            foreach (var rectanglex in rectangles)
            {
                foreach (var rectangley in rectangles)
                {
                    if (rectanglex != rectangley && !rectangleCrossings.Contains(Rectangle.Intersect(rectanglex.Rectangle, rectangley.Rectangle)))
                    {
                        Rectangle crossing = Rectangle.Intersect(rectanglex.Rectangle, rectangley.Rectangle);
                        if (!crossing.IsEmpty && !rectangleCrossings.Contains(crossing))
                        {
                            rectangleCrossings.Add(crossing);
                            rectangleCrossingColors.Add(new SolidBrush(Color.FromArgb(
                                ((SolidBrush)rectanglex.Brush).Color.R / 2 + ((SolidBrush)rectangley.Brush).Color.R / 2,
                                ((SolidBrush)rectanglex.Brush).Color.G / 2 + ((SolidBrush)rectangley.Brush).Color.R / 2,
                                ((SolidBrush)rectanglex.Brush).Color.B / 2 + ((SolidBrush)rectangley.Brush).Color.R / 2)));
                        }
                    }
                }
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

    public class Paintable

    {
        public Rectangle Rectangle { get; init; }

        public SolidBrush Brush { get; init; }

        public Paintable() : this(Rectangle.Empty, new SolidBrush(Color.Black)) { }

        public Paintable(Rectangle rectangle, SolidBrush brush)
        {
            Rectangle = rectangle;
            Brush = brush;
        }
    }
}