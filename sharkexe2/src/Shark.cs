using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class Shark : AnimObj
    {
        private String imageName;

        private int animatecounter = 0;
        private int animateFrameLen = 12;
        private int animateMode = 1;

        private int actionMode = 0;
        private int actionModeAmt = 2;
        private int actionCounter = 0;

        private int? huntingObjIndex = null;

        public Shark(Window window, Image imageBox, String imageName, int size = 0) : base(window, imageBox)
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

            switch (size)
            {
                // 0 -> Random size
                case 0:
                    size = Utils.random.Next(100, 200);
                    break;
                case 1:
                    size = 75;
                    break;
                case 2:
                    size = 100;
                    //objOffset = new Offset(80, 50, 40);
                    break;
                default:
                    size = 100 * size;
                    break;
            }

            setWindowSize(size);

            int offset1, offset2, offset3;

            offset1 = size - 25;
            offset2 = size / 2;
            offset3 = offset1 / 2;

            objOffset = new Offset(offset1, offset2, offset3);

            this.imageName = imageName;
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/" + imageName + "01.png"));

            int maxSpeed = Utils.random.Next(4, 6);
            animateFrameLen = 20 - (maxSpeed * 2);

            // Setup the obj varaibles
            objPosition = new Position(objOffset, true);
            objRotation = new Rotation(maxSpeed / 2);
            objSpeed = new SpeedUtils(maxSpeed);
            toPosition = objPosition;

            actionMode = Utils.random.Next(0, actionModeAmt);
        }

        public override void runActions()
        {
            //actionMode = 0;
            switch (actionMode)
            {
                case 0:
                    huntCursor();
                    break;

                case 1:
                    wanderScreen();
                    break;

                case 2:
                    //huntObj();
                    break;

                case 3:
                   // huntObj();
                   // break;

                case 4:
                   // makeBaby();
                   // break;

                default:
                    actionMode = 0;
                    break;
            }


            if (objPosition.nearByPosition(toPosition, 10))
            {
                if (actionCounter > 15)
                {
                    actionCounter = 0;
                    actionMode = Utils.random.Next(0, actionModeAmt);
                }

                actionCounter++;
            }
        }

        public void huntCursor()
        {
            this.moveTowardsPosition(getCursorPoint());
        }

        public void wanderScreen()
        {
            if (objPosition.nearByPosition(toPosition, 10))
            {
                int count = Utils.random.Next(0, 11);

                if (count > 8)
                {
                    toPosition = new Position(objOffset, true);
                }
            }

            this.moveTowardsPosition(toPosition);
        }

        public void stealCursor()
        {
            if (objPosition.nearByPosition(getCursorPoint(), 20))
            {
                
                if (objPosition.nearByPosition(toPosition, 20))
                {
                    toPosition = new Position(objOffset, true);
                }

                Cursor.Position = new System.Drawing.Point((int)objPosition.getOffsetPosition(objOffset).X, (int)objPosition.getOffsetPosition(objOffset).Y);
            }
            else
            {
                toPosition = getCursorPoint();
            }

            this.moveTowardsPosition(toPosition);
        }


        public void huntObj()
        {
            if (huntingObjIndex == null)
            {
                huntingObjIndex = Utils.getRandomObj(this);
            }

            AnimObj obj = Utils.getObj(huntingObjIndex);

            if (obj == null || huntingObjIndex == null)
            {
                huntingObjIndex = null;
                actionMode++;
                return;             
            }

            this.moveTowardsPosition(obj.objPosition);

            if (objPosition.nearByPosition(obj.objPosition, 10))
            {
                obj.deleteObj = true;
                huntingObjIndex = null;
            }

        }

        public void makeBaby()
        {
            if (huntingObjIndex == null)
            {
                huntingObjIndex = Utils.getRandomObj(this);
            }

            AnimObj obj = Utils.getObj(huntingObjIndex);

            if (obj == null || huntingObjIndex == null)
            {
                huntingObjIndex = null;
                actionMode++;
                return;
            }

            this.moveTowardsPosition(obj.objPosition);

            if (objPosition.nearByPosition(obj.objPosition, 10))
            {
                int rand = Utils.random.Next(0, 2);

                Utils.app.Dispatcher.BeginInvoke(new Action(() => Utils.createNewShark(rand == 0 ? "shark" : "blahaj")));
                huntingObjIndex = null;
                actionMode++;
                return;
            }

        }

        public void moveAtHalf()
        {
            objSpeed.setHalfVelocity(true);
        }

        public override void animateObj()
        {
            if (objSpeed.currentVelocity.currentSpeed() <= objSpeed.decerlation * 3)
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

        public override String childDebug()
        {
            return "Shark Idx:" + Utils.getObjIndex(this) + " Mode:" + actionMode + " huntIdx:" + huntingObjIndex;
        }
    }
}
