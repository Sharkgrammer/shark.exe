using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class Coral : AnimObj
    {

        public Coral(Window window, Image imageBox) : base(window, imageBox)
        {
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            int imgNumber = Utils.random.Next(1, 3);
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/coral0" + imgNumber + ".png"));
            this.disableAnim = true;

            int size = Utils.random.Next(50, 151);
            window.Width = size;
            window.Height = size;
            imageBox.Width = size;

            // Setup the obj varaibles
            objOffset = new Offset(0, (int) window.Height, 0);
            objPosition = new Position(objOffset, true, true);
            objRotation = new Rotation(0);

            double speed = Utils.random.Next(1, 5) * 0.1;

            objSpeed = new SpeedUtils(speed);
            toPosition = objPosition;
        }

        public override void runActions()
        {
            this.driftOverTaskbar();
        }

        public void driftOverTaskbar()
        {
            if (objPosition.nearByPosition(toPosition, 10))
            {
                int count = Utils.random.Next(0, 20);

                if (count > 18)
                {
                    toPosition = new Position(objOffset, true, true);
                }
            }

            this.moveTowardsPosition(toPosition);
        }


        public override String childDebug()
        {
            return "Coral Idx:" + Utils.getObjIndex(this);
        }
    }
}
