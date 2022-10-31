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

        public void adjustVelocity(double accerlation, double decerlation, Velocity maxSpeeds, Velocity minSpeeds, Boolean forceDecerlate = false)
        {
            if (forceDecerlate)
            {
                decelerate(decerlation, minSpeeds);
            }
            else
            {
                accelerate(accerlation, decerlation, maxSpeeds, minSpeeds);
            }

        }

        private void accelerate(double accerlation, double decerlation, Velocity maxSpeeds, Velocity minSpeeds)
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

        private void decelerate(double decerlation, Velocity minSpeeds)
        {
            decelerateX(decerlation, minSpeeds);
            decelerateY(decerlation, minSpeeds);
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

        public String toString()
        {
            return "Velocity.toString() -> X:" + X + " Y:" + Y + " half:" + half;
        }

    }
}
