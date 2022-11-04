using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sharkexe2.src.util
{
    public class Velocity
    {
        public double X { get; set; } = 0.0;

        public double Y { get; set; } = 0.0;

        public bool half = false;

        public void adjustVelocity(double accerlation, double decerlation, Velocity maxSpeeds, Velocity minSpeeds, Boolean decerlateX = false, Boolean decerlateY = false)
        {
            if (decerlateX)
            {
                decelerateX(decerlation, minSpeeds);
            }
            else
            {
                accelerateX(accerlation, decerlation, maxSpeeds, minSpeeds);
            }

            if (decerlateY)
            {
                decelerateY(decerlation, minSpeeds);
            }
            else
            {
                accelerateY(accerlation, decerlation, maxSpeeds, minSpeeds);
            }

        }

        private void accelerateX(double accerlation, double decerlation, Velocity maxSpeeds, Velocity minSpeeds)
        {
            if (this.X < maxSpeeds.X)
            {
                this.X += this.half ? accerlation / 2 : accerlation;

                if (this.X > maxSpeeds.X)
                {
                    this.X = maxSpeeds.X;
                }
            }
            else
            {
                decelerateX(decerlation, minSpeeds);
            }
        }

        private void accelerateY(double accerlation, double decerlation, Velocity maxSpeeds, Velocity minSpeeds)
        {
            if (this.Y < maxSpeeds.Y)
            {
                this.Y += this.half ? accerlation / 2 : accerlation;

                if (this.Y > maxSpeeds.Y)
                {
                    this.Y = maxSpeeds.Y;
                }
            }
            else
            {
                decelerateY(decerlation, minSpeeds);
            }
        }

        private void decelerateX(double decerlation, Velocity minSpeeds)
        {
            if (this.X > minSpeeds.X)
            {
                this.X -= this.half ? decerlation / 2 : decerlation;

                if (this.X < minSpeeds.X)
                {
                    this.X = minSpeeds.X;
                }
            }
        }

        private void decelerateY(double decerlation, Velocity minSpeeds)
        {
            if (this.Y > minSpeeds.Y)
            {
                this.Y -= this.half ? decerlation / 2 : decerlation;

                if (this.Y < minSpeeds.Y)
                {
                    this.Y = minSpeeds.Y;
                }
            }
        }

        public double currentSpeed()
        {
            return X + Y;
        }
        public String toString()
        {
            return "Velocity.toString() -> currentSpeed:" + currentSpeed() + " X:" + X + " Y:" + Y + " half:" + half;
        }

    }
}
