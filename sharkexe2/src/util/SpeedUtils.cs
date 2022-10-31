using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sharkexe2.src.util
{
    public class SpeedUtils
    {
        public Velocity currentVelocity { get; set; }
        private Velocity maxVelocity { get; set; }
        private Velocity minVelocity { get; set; }


        private double maxSpeed { get; set; } = 10;
        private double accerlation { get; set; } = 0.3;
        private double decerlation { get; set; } = 0.6;

        public SpeedUtils()
        {
            this.currentVelocity = new Velocity()
            {
                X = 0,
                Y = 0,
            };

            this.minVelocity = new Velocity()
            {
                X = 0,
                Y = 0,
            };

            this.maxVelocity = new Velocity()
            {
                X = 0,
                Y = 0,
            };
        }

        public Velocity calcVelocity(double rotation = 0.0, Boolean decerlate = false)
        {
            calcMaxRotationVelocity(rotation);

            currentVelocity.adjustVelocity(accerlation, decerlation, maxVelocity, minVelocity, decerlate);

            return currentVelocity;
        }

        public void calcMaxRotationVelocity(double rotation)
        {
            bool posX, posY;
            double rotationChange = 0.0;

            posX = Math.Abs(rotation) < 90.0;
            posY = rotation > 0.0;

            rotation = Math.Abs(rotation);

            while (rotation > 90.0)
            {
                rotation -= 90.0;
            }

            rotationChange = rotation / 90;

            maxVelocity.Y = rotationChange * maxSpeed;
            maxVelocity.X = (1 - Math.Abs(rotationChange)) * maxSpeed;

            if (!posX) maxVelocity.X *= -1;
            if (!posY) maxVelocity.Y *= -1;

            if (maxVelocity.half)
            {
                maxVelocity.X /= 2;
                maxVelocity.Y /= 2;
            }
        }

        public void setHalfVelocity(bool val)
        {
            currentVelocity.half = val;
            maxVelocity.half = val;
        }

        public String toString()
        {
            return "SpeedUtils.toString() -> currentVelc: " + currentVelocity.toString();
        }
    }
}
