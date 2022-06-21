using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sharkexe2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Basic Details
        private int sx = 0;
        private int sy = 0;
        private int sTickSpeed = 15;

        private int sImgCounter = 0;
        private int sImgCounterMax = 10;
        private Boolean sImgSkip = true;
        private Boolean sImgPause = false;
        private int sDir = 0;
        private int sMode = 2;

        private int sRotationSpeed = 1;
        private double sToRotation = 0;
        private double sRotation = 0;
        private Boolean isRotating = true;

        private double sSpeedX = 1;
        private double sSpeedY = 1;
        private int sTopSpeed = 5;
        private int sTopSpeedStraight = 10;
        private int sStartSpeed = 1;
        private double sAccerlation = 0.3;
        private double sDecerlation = 0.8;

        private int sNearness = 5;
        private int sAccerlationNearness = 30;

        private int CursorOffsetX = 85;
        private int CursorOffsetY = 50;

        //Other functions
        private Boolean followMouse = true;

        private static System.Timers.Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();

            aTimer = new System.Timers.Timer(sTickSpeed);
            aTimer.Elapsed += timer_tick;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private Point getCursorPoint()
        {
            var cursorPoint = System.Windows.Forms.Cursor.Position;
            return new Point(cursorPoint.X - CursorOffsetX, cursorPoint.Y - CursorOffsetY);
        }

        private void setImage()
        {
            if (sImgPause)
            {
                return;
            }

            if (sImgSkip || sImgCounter >= sImgCounterMax)
            {
                sImgSkip = false;
                sImgCounter = 0;

                if (sMode == 1)
                {
                    sMode = 2;
                }
                else
                {
                    sMode = 1;
                }

                imgMain.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/" + sDir.ToString() + sMode.ToString() + ".png"));


                RotateTransform rotateTransform1 = new RotateTransform(sRotation);
                imgMain.RenderTransformOrigin = new Point(0.5, 0.5);
                imgMain.RenderTransform = rotateTransform1;
            }
            else
            {
                sImgCounter++;
            }
        }

        private void setSDir(int val)
        {
            if (sDir == val)
            {
                return;
            }

            sDir = val;
            if ((sToRotation < 0 && sRotation > 0) || (sToRotation > 0 && sRotation < 0))
            {
                sRotation *= -1;
            }

            switch (sDir)
            {
                case 0:
                    CursorOffsetX = 85;
                    CursorOffsetY = 50;
                    break;
                case 4:
                    CursorOffsetX = 10;
                    CursorOffsetY = 50;
                    break;
            }
        }

        private void moveShark(Point to)
        {
            pointAccerlationCalc(to);

            sImgPause = nearByTarget(to, false, 5);

            if (!nearByTargetX(to))
            {
                Boolean nearByTargetAccDist = nearByTargetX(to, true, 10);

                if (sx > to.X)
                {
                    sx -= (int)sSpeedX;
                    if (!nearByTargetAccDist) setSDir(4);
                }
                else if (sx < to.X)
                {
                    sx += (int)sSpeedX;
                    if (!nearByTargetAccDist) setSDir(0);
                }
            }

            if (!nearByTargetY(to))
            {
                if (sy > to.Y)
                {
                    sy -= (int)sSpeedY;
                }
                else
                {
                    sy += (int)sSpeedY;
                }

            }
        }

        private void pointAccerlationCalc(Point to)
        {
            // X calcs
            if (nearByTargetX(to, true))
            {
                if (sSpeedX > sStartSpeed)
                {
                    sSpeedX -= sDecerlation;
                }
            }
            else
            {
                if (sSpeedX < sTopSpeed || (!isRotating && sSpeedX < sTopSpeedStraight))
                {
                    sSpeedX += sAccerlation;
                }
            }

            // Y calcs
            if (nearByTargetY(to, true))
            {
                if (sSpeedY > sStartSpeed)
                {
                    sSpeedY -= sDecerlation;
                }
            }
            else
            {
                if(sSpeedY < sTopSpeed || (!isRotating && sSpeedY < sTopSpeedStraight))
                {
                    sSpeedY += sAccerlation;
                }

            }

            // Check if rotating & it its above sTopSpeed for larger decerlation?

            if (isRotating)
            {
                if (sSpeedY > sTopSpeed)
                {
                    sSpeedY -= sDecerlation;
                }

                if (sSpeedX > sTopSpeed)
                {
                    sSpeedX -= sDecerlation;
                }
            }

        }

        private void faceTarget(Point to)
        {

            if (nearByTarget(to, true))
            {
                return;
            }

            double x = to.X - sx;
            double y = to.Y - sy;

            sToRotation = Math.Atan(y/x) * 180 / Math.PI;

            if (sRotation < sToRotation)
            {
                sRotation += getRotationSpeed();
            }
            else if (sRotation > sToRotation)
            {
                sRotation -= getRotationSpeed();
            }
        }

        private double getRotationSpeed()
        {
            isRotating = true;
            double rotTest = Math.Abs(sRotation) - Math.Abs(sToRotation);
            double tempRotSpeed = sRotationSpeed;

            if (Math.Abs(rotTest) < sRotationSpeed)
            {
                tempRotSpeed = sRotationSpeed - Math.Abs(rotTest);

                if (tempRotSpeed <= 1)
                {
                    isRotating = false;
                    tempRotSpeed = 0;
                }
            }

            return tempRotSpeed;
        }

        private Boolean nearByTarget(Point to, Boolean accNear = false, int targetAdd = 0)
        {
            return (nearByTargetX(to, accNear, targetAdd) && nearByTargetY(to, accNear, targetAdd));
        }

        private Boolean nearByTargetX(Point t, Boolean accNear = false, int targetAdd = 0)
        {
            if (accNear)
            {
                return Math.Abs(sx - t.X) <= sAccerlationNearness + targetAdd;
            }
            else
            {
                return Math.Abs(sx - t.X) <= sNearness + targetAdd;
            }
        }

        private Boolean nearByTargetY(Point t, Boolean accNear = false, int targetAdd = 0)
        {
            if (accNear)
            {
                return Math.Abs(sy - t.Y) <= sAccerlationNearness + targetAdd;
            }
            else
            {
                return Math.Abs(sy - t.Y) <= sNearness + targetAdd;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(setImage));

            if (!followMouse)
            {
                spinShark();
            }
            else
            {
                moveTowardsMousePoint();
            }

            Dispatcher.BeginInvoke(new Action(updateFormLocation));
        }

        private void spinShark()
        {
            if (Math.Abs(sRotation) >= 360)
            {
                sRotation = 0;
            }
            else
            {
                sRotation += 30;
            }
        }

        private void moveTowardsMousePoint()
        {
            Point cursor = getCursorPoint();
            faceTarget(cursor);
            moveShark(cursor);

            Console.WriteLine(sRotation + " : " + sToRotation);
        }

        private void updateFormLocation()
        {
            this.Left = sx;
            this.Top = sy;
        }

    }
}
