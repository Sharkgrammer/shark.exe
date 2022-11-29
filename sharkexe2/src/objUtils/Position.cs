using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace sharkexe2.src.util
{
    public class Position
    {

        public double X { get; set; }

        public double Y { get; set; }

        public Boolean mainScreenOnly = true;

        public Position(Offset offset = null, bool random = true, bool atTaskbar = false, bool underTaskbar = false)
        {
            if (random && offset != null)
            {
                getRandomPosition(offset, atTaskbar, underTaskbar);
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

        public void setPosition(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public Position getOffsetPosition(Offset offset)
        {
            return new Position(X + offset.X, Y + offset.Y);
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

        private void getRandomPosition(Offset offset, bool atTaskbar = false, bool underTaskbar = false)
        {
            if (mainScreenOnly)
            {
                X = Utils.random.Next(0, (int)SystemParameters.PrimaryScreenWidth - offset.X);
            }
            else
            {
                X = Utils.random.Next(0, (int)SystemParameters.VirtualScreenWidth - offset.X);
            }

            if (underTaskbar)
            {
                Y = SystemParameters.VirtualScreenHeight;
            }
            else if (atTaskbar)
            {
                Y = SystemParameters.VirtualScreenHeight - offset.Y;
            }
            else
            {
                Y = Utils.random.Next(0, (int)SystemParameters.VirtualScreenHeight - offset.Y);
            }
        }

        public void getRandomPositionNearby(int nearby = 10, Boolean up = false, Boolean down = false)
        {
            X += Utils.random.Next(nearby + 1) * (Utils.random.Next(2) == 1 ? -1 : 1);

            if (up)
            {
                Y -= Utils.random.Next(nearby + 1) * 2;
            }
            else if (down)
            {
                Y += Utils.random.Next(nearby + 1) * 2;
            }
            else
            {
                Y += Utils.random.Next(nearby + 1) * (Utils.random.Next(2) == 1 ? -1 : 1);
            }
        }

        public Position getFleePosition(Position to, int dist = 200)
        {
            Position fleePos = new Position(); 

            if (X > to.X)
            {
                fleePos.X = X + dist;
            }
            else
            {
                fleePos.X = X - dist;
            }


            if (Y > to.Y)
            {
                fleePos.Y = Y + dist;
            }
            else
            {
                fleePos.Y = Y - dist;
            }

            return fleePos;
        }

        public Position getMiddlePosition(int size)
        {
            Position middlePos = new Position();
            middlePos.X = X + size / 2;
            middlePos.Y = Y + size / 2;

            return middlePos;
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
            return nearByY(p.Y, bubble, bubbleAdjustment);
        }

        public Boolean nearByY(double y, double bubble, double bubbleAdjustment = 0)
        {
            return Math.Abs(Y - y) <= bubble + bubbleAdjustment;
        }

        public String toString()
        {
            return "Position.toString() -> X:" + X + " Y:" + Y;
        }
    }
}