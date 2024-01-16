using System;
using System.Drawing;
using System.Windows.Forms;

namespace Morpher
{
    public class Line
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }
        private bool moving;
        private bool resizingStart;
        private bool resizingEnd;
        private readonly Pen pen;
        private readonly Pen highlightPen;

        public Line(int startX, int startY, int endX = 0, int endY = 0)
        {
            Start = new Point(startX, startY);
            End = new Point(endX, endY);
            pen = new Pen(Color.Black, 3F);
            highlightPen = new Pen(Color.White, 2F);
        }

        public void UpdateEndPoints(int endX, int endY)
        {
            End = new Point(endX, endY);
        }

        public void UpdateStartPoints(int startX, int startY)
        {
            Start = new Point(startX, startY);
        }

        public void Draw(PaintEventArgs e)
        {
            e.Graphics.DrawLine(pen, Start, End);
            DrawHandles(e);
        }

        private void DrawHandles(PaintEventArgs e)
        {
            // Assuming e.Graphics.DrawCircle exists or is implemented elsewhere
            e.Graphics.DrawCircle(highlightPen, Start.X, Start.Y, 3.5F);
            e.Graphics.DrawCircle(highlightPen, (Start.X + End.X) / 2, (Start.Y + End.Y) / 2, 3.5F);
            e.Graphics.DrawCircle(highlightPen, End.X, End.Y, 3.5F);
        }

        public int GetUserIntention(MouseEventArgs e)
        {
            // Adjust the logic to use Start and End properties
            double lineCenterX = (Start.X + End.X) / 2;
            double lineCenterY = (Start.Y + End.Y) / 2;

            // user grabbing the center of the line, move
            if (Math.Abs(e.Location.X - lineCenterX) < 5 && Math.Abs(e.Location.Y - lineCenterY) < 5)
            {
                moving = true;
                resizingStart = false;
                resizingEnd = false;
                return Intention.MOVING;
            }
            // user grabbing the starting end of the line, resize
            else if (Math.Abs(e.Location.X - Start.X) < 6 && Math.Abs(e.Location.Y - Start.Y) < 6)
            {
                moving = false;
                resizingStart = true;
                resizingEnd = false;
                return Intention.RESIZING_START;
            }
            // user grabbing the end of the line, resize
            else if (Math.Abs(e.Location.X - End.X) < 6 && Math.Abs(e.Location.Y - End.Y) < 6)
            {
                moving = false;
                resizingStart = false;
                resizingEnd = true;
                return Intention.RESIZING_END;
            }
            // user is not resizing or moving an existing line, they are creating a new one
            else return Intention.CREATING_NEW_LINE;
        }

        public void Resize(MouseEventArgs e)
        {
            if (moving)
            {
                int offsetX = e.Location.X - (Start.X + End.X) / 2;
                int offsetY = e.Location.Y - (Start.Y + End.Y) / 2;
                Start = new Point(Start.X + offsetX, Start.Y + offsetY);
                End = new Point(End.X + offsetX, End.Y + offsetY);
            }
            else if (resizingStart)
            {
                Start = new Point(e.Location.X, e.Location.Y);
            }
            else if (resizingEnd)
            {
                End = new Point(e.Location.X, e.Location.Y);
            }
        }
    }
}


