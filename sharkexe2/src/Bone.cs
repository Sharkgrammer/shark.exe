using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class Bone : AnimObj
    {

        int boneFrameCounter = 0;
        int boneFrames = 20;

        public Bone(Window window, Image imageBox, Position startPos) : base(window, imageBox)
        {
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            int imgNumber = Utils.random.Next(1, 2);
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/bone0" + imgNumber + ".png"));
            this.disableAnim = true;

            int size = Utils.random.Next(10, 51);
            setWindowSize(size);

            // Setup the obj varaibles
            objOffset = new Offset();

            startPos.X += Utils.random.Next(0, 20) * (Utils.random.Next(0, 2) == 1 ? -1 : 1);
            startPos.Y += Utils.random.Next(0, 20) * (Utils.random.Next(0, 2) == 1 ? -1 : 1);

            objPosition = startPos;
            objRotation = new Rotation(0);

            RotateTransform rotateBone = new RotateTransform(Utils.random.Next(0, 361));
            imageBox.RenderTransform = rotateBone;

            double speed = (size / 2) * 0.01;

            objSpeed = new SpeedUtils(speed);
            toPosition = objPosition;
        }

        public override void runActions()
        {
            this.driftDown();
        }

        public void driftDown()
        {
            if (boneFrameCounter++ >= boneFrames)
            {
                if (objPosition.nearByY((int) SystemParameters.VirtualScreenHeight, 10))
                {
                    deleteObj = true;
                }

                if (objPosition.nearByPosition(toPosition, 0))
                {
                    toPosition.getRandomPositionNearby(3, false, true);
                }

                this.moveTowardsPosition(toPosition);
                boneFrameCounter = 0;
            }
        }

        public override String childDebug()
        {
            return "Bone Idx:" + Utils.getObjIndex(this);
        }
    }
}
