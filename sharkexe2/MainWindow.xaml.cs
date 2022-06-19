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
        private int sRotation = 0;

        private double sSpeedX = 1;
        private double sSpeedY = 1;
        private int sTopSpeed = 5;
        private int sStartSpeed = 1;
        private double sAccerlation = 0.5;
        private double sDecerlation = 1;

        private int sNearness = 5;
        private int sAccerlationNearness = 30;

        private int CursorOffsetX = 105;
        private int CursorOffsetY = 25;

        //Other functions
        private Boolean followMouse = true;


        private static System.Timers.Timer aTimer;

        public MainWindow()
        {
            InitializeComponent();


            /*
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, sTickSpeed);
            timer.Start();*/

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

        private Point towardsMousePoint()
        {
            moveShark(getCursorPoint());

            return new Point(sx, sy);
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

                /*RotateTransform rotateTransform1 = new RotateTransform(sRotation);
                imgBorder.RenderTransform = rotateTransform1;

                TranslateTransform trans = new TranslateTransform(-10, -10);
                imgMain.RenderTransform = trans;

                Point position = imgMain.PointToScreen(new Point(0d, 0d));

                Console.WriteLine("X:" + position.X + " Y:" + position.Y);

                if (sRotation >= 360)
                {
                    sRotation = 0;
                }
                else
                {
                    sRotation += 90;
                }*/
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
            sImgSkip = true;

            switch (sDir)
            {
                case 0:
                    CursorOffsetX = 105;
                    CursorOffsetY = 25;
                    break;
                case 4:
                    CursorOffsetX = 10;
                    CursorOffsetY = 25;
                    break;
            }
        }

        private void moveShark(Point to)
        {

            pointAccerlationCalc(to);

            //sImgPause = nearByTarget(to);

            if (!nearByTargetX(to))
            {
                if (sx > to.X)
                {
                    sx -= (int)sSpeedX;
                    setSDir(4);
                }
                else
                {
                    sx += (int)sSpeedX;
                    setSDir(0);
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
                if (sSpeedX < sTopSpeed)
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
                if (sSpeedY < sTopSpeed)
                {
                    sSpeedY += sAccerlation;
                }
            }

        }

        private Boolean atTarget(Point to)
        {
            return to == new Point(sx, sy);
        }

        private Boolean nearByTarget(Point to, Boolean accNear = false)
        {
            return (nearByTargetX(to, accNear) && nearByTargetY(to, accNear));
        }

        private Boolean nearByTargetX(Point t, Boolean accNear = false)
        {
            if (accNear)
            {
                return Math.Abs(sx - t.X) <= sAccerlationNearness;
            }
            else
            {
                return Math.Abs(sx - t.X) <= sNearness;
            }
        }


        private Boolean nearByTargetY(Point t, Boolean accNear = false)
        {
            if (accNear)
            {
                return Math.Abs(sy - t.Y) <= sAccerlationNearness;
            }
            else
            {
                return Math.Abs(sy - t.Y) <= sNearness;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(setImage));

            if (!followMouse)
            {
                if (sy < SystemInformation.VirtualScreen.Height - this.Height)
                {
                    sy += (int)sSpeedY;
                }

                if (sx < SystemInformation.VirtualScreen.Width - this.Width)
                {
                    sx += (int)sSpeedX;
                }

            }
            else
            {
                towardsMousePoint();
            }

            Dispatcher.BeginInvoke(new Action(updateFormLocation));
        }

        private void updateFormLocation()
        {
            this.Left = sx;
            this.Top = sy;
        }

    }
}
