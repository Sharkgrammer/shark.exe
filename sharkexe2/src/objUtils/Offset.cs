using System;
using System.Windows;

namespace sharkexe2.src.util
{
    public class Offset
    {

        public int X { get; set; }

        public int Y { get; set; }

        public int Radius { get; set; }

        public Offset(int x, int y, int radius)
        {
            this.X = x;
            this.Y = y;
            this.Radius = radius;    
        }

        public void adjustOffset(double rotation = 0.0, int width = 50)
        {
            rotation = (Math.PI / 180) * rotation;

            X = Math.Abs((int) ((width / 2) + Radius * Math.Cos(rotation)));
            Y = Math.Abs((int) ((width / 2) + Radius * Math.Sin(rotation)));
        }
        
        public void flip(double width, double height)
        {
            // Width is always higher than X
            X = (int) (width - X);
            Y = (int) (height - Y);
        }

        public String toString()
        {
            return "Offset.toString() -> X:" + X + " Y:" + Y;
        }
    }
}