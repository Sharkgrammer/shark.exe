using System;

namespace sharkexe2.src.util
{
    public class Rotation
    {
        // Rotation only has one axis, so no need for Velocity
        private int maxSpeed = 2;
        private double currentSpeed = 0;
        private double accerlation = 0.1;
        private double decerlation = 0.2;

        public double currentRotation { get; set; } = 0;
        public double toRotation { get; set; } = 0;

        public Rotation(int maxSpeed = 2)
        {
            this.maxSpeed = maxSpeed;
        }

        public void faceTowardsPosition(Position current, Position to)
        {
            if (current.nearByPosition(to, 10))
            {
                return;
            }

            calcToRotation(current, to);
            rotate();
        }

        public void forceFaceTowardsPosition(Position current, Position to, Boolean byPass = false)
        {
            if (!byPass && current.nearByPosition(to, 10))
            {
                return;
            }

            calcToRotation(current, to);

            currentRotation = toRotation;
        }

        public void forceRotation(double rotation)
        {
            toRotation = rotation;
            currentRotation = toRotation;
        }

        private void calcToRotation(Position current, Position to)
        {
            double x = to.X - current.X;
            double y = to.Y - current.Y;

            toRotation = (Math.Atan(y / x) * 180) / Math.PI;
            
            if (Math.Abs(currentRotation + toRotation) < 5){
                currentRotation = toRotation;
            }
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
