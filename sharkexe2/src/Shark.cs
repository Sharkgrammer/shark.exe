using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    class Shark : AnimObj
    {
        private String imageName;

        private int animatecounter = 0;
        private int animateFrameLen = 12;
        private int animateMode = 1;

        public Shark(Window window, Image imageBox, String imageName) : base(window, imageBox)
        {
            switch (imageName)
            {
                case "shark":
                case "blahaj":
                    break;
                default:
                    imageName = "shark";
                    break;
            }


            this.imageName = imageName;
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/" + imageName + "01.png"));

            int maxSpeed = Utils.random.Next(2, 6);
            animateFrameLen = 20 - (maxSpeed * 2);

            // Setup the obj varaibles
            objOffset = new Offset(80, 50, 40);
            objPosition = new Position(objOffset, true);
            objRotation = new Rotation(maxSpeed / 2);
            objSpeed = new SpeedUtils(maxSpeed);
            toPosition = objPosition;
            toPosition.active = false;
        }

        public void huntCursor()
        {
            //this.moveTowardsPosition(new src.util.Position(300, 300));
            this.moveTowardsPosition(getCursorPoint());
        }

        public void wanderScreen()
        {
            if (objPosition.nearByPosition(toPosition, 10))
            {
                int count = Utils.random.Next(0, 10);

                if (count > 8)
                {
                    toPosition = new Position(objOffset, true);
                }
            }

            this.moveTowardsPosition(toPosition);
        }

        public void moveAtHalf()
        {
            objSpeed.setHalfVelocity(true);
        }

        public override void animateObj()
        {
            if (objSpeed.currentVelocity.currentSpeed() <= objSpeed.decerlation * 2)
            {
                return;
            }

            if (animatecounter++ > animateFrameLen)
            {
                animateMode = animateMode == 1 ? animateMode = 2 : animateMode = 1;
                animatecounter = 0;
            }

            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/" + imageName + "0" + animateMode + ".png"));
        }

    }
}
