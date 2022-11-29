using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class Bubble : AnimObj
    {

        int bubbleFrames = 10;
        int frameCounter = 0;

        public Bubble(Window window, Image imageBox) : base(window, imageBox)
        {
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/bubble01.png"));
            this.disableAnim = true;

            setWindowSize(Utils.random.Next(10, 61));

            // Setup the obj varaibles
            objOffset = new Offset(0, (int) window.Height, 0);
            objPosition = new Position(objOffset, true, false, true);
            objRotation = new Rotation(0);

            double speed = Utils.random.Next(1, 20) * 0.1;

            objSpeed = new SpeedUtils(speed);
            toPosition = objPosition;
        }

        public override void runActions()
        {
            if (frameCounter >= bubbleFrames)
            {
                frameCounter = 0;
                this.driftUp();
            }
            else
            {
                frameCounter++;
            }
        }

        public void driftUp()
        {
            if (objPosition.nearByY(0 - objOffset.Y, 10))
            {
                deleteObj = true;
            }

            if (objPosition.nearByPosition(toPosition, 0))
            {
                toPosition.getRandomPositionNearby(2, true);
            }

            this.moveTowardsPosition(toPosition);
        }


        public override String childDebug()
        {
            return "Bubble Idx:" + Utils.getObjIndex(this);
        }
    }
}
