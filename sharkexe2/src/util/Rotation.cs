using System;

namespace sharkexe2.src.util
{
    public class Rotation
    {
        // Rotation only has one axis, so no need for Velocity
        private int maxSpeed = 1;
        private double currentSpeed = 0;
        private double accerlation = 0.1;
        private double decerlation = 0.1;

        public double currentRotation { get; set; } = 0;
        public double toRotation { get; set; } = 0;


        public void faceTowardsPosition(Position current, Position to)
        {
            if (current.nearByPosition(to, 10))
            {
                return;
            }

            calcToRotation(current, to);
            rotate();
        }

        private void calcToRotation(Position current, Position to)
        {
            double x = to.X - current.X;
            double y = to.Y - current.Y;

            toRotation = Math.Atan(y / x) * 180 / Math.PI;
        }

        private void rotate()
        {
            if (currentRotation < toRotation)
            {
                currentRotation += getRotationSpeed();
            }
            else if (currentRotation > toRotation)
            {
                currentRotation -= getRotationSpeed();
            }
        }

        private double getRotationSpeed()
        {
            // Decerlate rotation if its within 4 degrees.
            // TODO -> do better
            Boolean decerlate = Math.Abs(Math.Abs(currentRotation) - Math.Abs(toRotation)) <= 4;

            //Console.WriteLine("Rot Speed:" + currentSpeed);

            if (!decerlate)
            {
                currentSpeed += accerlation;

                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
            }
            else
            {
                currentSpeed -= decerlation;

                if (currentSpeed <= 0)
                {
                    currentSpeed = 0;
                }
            }
            
            return currentSpeed;
        }

        public String toString()
        {
            return "Rotation.toString() -> currentRot:" + currentRotation + " toRot:" + toRotation + " currentSpeed:" + currentSpeed;
        }
    }
}
