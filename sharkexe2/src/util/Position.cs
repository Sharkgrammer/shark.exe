﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace sharkexe2.src.util
{
    public class Position
    {

        public double X { get; set; }

        public double Y { get; set; }

        public Boolean active { get; set; } = true;

        public Position(Offset offset = null, bool random = true)
        {
            if (random && offset != null)
            {
                getRandomPosition(offset);
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

        private void getRandomPosition(Offset offset)
        {
            X = Utils.random.Next(0, (int) SystemParameters.FullPrimaryScreenWidth - offset.X);
            Y = Utils.random.Next(0, (int) SystemParameters.VirtualScreenHeight - offset.Y);
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