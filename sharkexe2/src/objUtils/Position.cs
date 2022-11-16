using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace sharkexe2.src.util
{
    public class Position
    {

        public double X { get; set; }

        public double Y { get; set; }

        public Boolean mainScreenOnly = false;

        public Position(Offset offset = null, bool random = true, bool atTaskbar = false)
        {
            if (random && offset != null)
            {
                getRandomPosition(offset, atTaskbar);
            }
            else
            {
                X = 0;
                Y = 0;
            }
        }

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Position(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Position getOffsetPosition(Offset offset)
        {
            return new Position(X + offset.X, Y + offset.Y);
        }

        private void getRandomPosition(Offset offset, bool atTaskbar)
        {
            if (mainScreenOnly)
            {
                X = Utils.random.Next(0, (int)SystemParameters.PrimaryScreenWidth - offset.X);
            }
            else
            {
                X = Utils.random.Next(0, (int)SystemParameters.VirtualScreenWidth - offset.X);
            }

            if (atTaskbar)
            {
                Y = SystemParameters.VirtualScreenHeight - offset.Y;
            }
            else
            {
                Y = Utils.random.Next(0, (int)SystemParameters.VirtualScreenHeight - offset.Y);
            }
        }

        public void calcPosition(Position to, Velocity velocity, int bubble)
        {
            // X position
            if (!nearByPositionX(to, bubble))
            {
                if (X > to.X)
                {
                    X -= velocity.X;
                }
                else
                {
                    X += velocity.X;
                }
            }

            // Y position
            if (!nearByPositionY(to, bubble))
            {
                if (Y > to.Y)
                {
                    Y -= velocity.Y;
                }
                else
                {
                    Y += velocity.Y;
                }
            }
        }

        public Boolean nearByPosition(Position p, double bubble, double bubbleAdjustment = 0)
        {
            return (nearByPositionX(p, bubble, bubbleAdjustment) && nearByPositionY(p, bubble, bubbleAdjustment));
        }

        public Boolean nearByPositionX(Position p, double bubble, double bubbleAdjustment = 0)
        {
            return Math.Abs(X - p.X) <= bubble + bubbleAdjustment;
        }

        public Boolean nearByPositionY(Position p, double bubble, double bubbleAdjustment = 0)
        {
            return Math.Abs(Y - p.Y) <= bubble + bubbleAdjustment;
        }

        public String toString()
        {
            return "Position.toString() -> X:" + X + " Y:" + Y;
        }
    }
}