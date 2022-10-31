using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using Application = System.Windows.Application;
using sharkexe2.src.util;

namespace sharkexe2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Timers.Timer timer;
        private int tickSpeed = 15;
        private List<Fish> fishList = new List<Fish>();
        private Random rand;


        private Shark shark;

        public App()
        {

            //makeNewFish(true);
            //temp();


            FishWindow fish = new FishWindow();
            fish.Show();

            shark = new Shark(fish, fish.imgMain, "shark");
            ///*
            rand = new Random();
            timer = new System.Timers.Timer(tickSpeed);
            timer.Elapsed += timer_tick;
            timer.AutoReset = true;
            timer.Enabled = true;//*/
        }

        private void temp()
        {
            AnimObj animObj = new AnimObj();

            Position pos = new Position(1000, 1000);

            Console.WriteLine("To Position -> X:" + pos.X + " Y:" + pos.Y);

            int x = 0;


            while(x++ < 100)
            {
                animObj.moveTowardsPosition(pos);
            }

            //SpeedUtils s = animObj.getRotationSpeed(0);
        }

        private void makeNewFish(Boolean shark)
        {
            FishWindow fish = new FishWindow();
            fish.Show();

            String imageName = shark ? "shark" : "blahaj";

            fishList.Add(new Fish(fish, fish.imgMain, imageName));
        }

        private void timer_tick(object sender, EventArgs e)
        {

            shark.huntCursor();
            Dispatcher.BeginInvoke(new Action(shark.updateFormLocation));

            /*
            foreach (Fish fish in fishList)
            {
                Dispatcher.BeginInvoke(new Action(fish.setImage));

                if (!fish.lockModeSwitch)
                {
                    int randInt = rand.Next(0, 100);

                    if (randInt < 10)
                    {
                        fish.lockModeSwitch = true;

                        fish.sMode += rand.Next(0, 3);
                        if (fish.sMode > 3)
                        {
                            fish.sMode = 1;
                        }
                    }
                }

                //sMode controls what exactly the shark does
                switch (fish.sMode)
                {
                    case 0:
                        fish.spinShark();
                        break;
                    case 1:
                        fish.moveTowardsMousePoint();
                        break;
                    case 2:
                        fish.moveAtRandom();
                        break;
                    case 3:
                        fish.huntUnderTaskBar();
                        break;
                }


                Dispatcher.BeginInvoke(new Action(fish.updateFormLocation));

            }
            */
        }
    }








    public class Fish
    {
        //Form Details
        public Window window;
        public Image image;
        private String imageName = "";

        //Basic Details
        private int sx = 0;
        private int sy = 0;
        public int sMode = 1;
        public Boolean lockModeSwitch = false;

        private int sImgCounter = 0;
        private int sImgCounterMax = 10;
        private Boolean sImgSkip = true;
        private Boolean sImgPause = false;
        private int sDir = 0;
        private int sAnimMode = 2;

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
        private int ScreenHeight = 0;
        private int ScreenWidth = 0;

        // For moveTowardsMousePoint
        private int moveTowardsCount = 0;

        // For moveAtRandom
        private Point randPoint = new Point();
        private int randCount = 0;
        private Random rand;


        public Fish(Window window, Image image, String imageName = "shark")
        {
            this.window = window;
            this.image = image;
            this.imageName = imageName;
            this.ScreenHeight = (int)SystemParameters.VirtualScreenHeight;
            this.ScreenWidth = (int)SystemParameters.VirtualScreenWidth;

            rand = new Random();
            sMode = rand.Next(1, 3);
            sx = rand.Next(0, ScreenWidth - CursorOffsetX);
            sy = rand.Next(0, ScreenHeight - CursorOffsetY);
        }

        private void setSharkChill(Boolean chill)
        {

            if (sTopSpeed == 5 && chill)
            {
                sSpeedX = 0.2;
                sSpeedY = 0.1;
                sTopSpeed = 3;
                sTopSpeedStraight = 5;
                sStartSpeed = 0;
                sAccerlation = 0.1;
            }
            else if (sTopSpeed != 5 && !chill)
            {
                sSpeedX = 1;
                sSpeedY = 1;
                sTopSpeed = 5;
                sTopSpeedStraight = 10;
                sStartSpeed = 1;
                sAccerlation = 0.3;
            }

        }

        private Point generateRandomPoint(Boolean underTaskbar)
        {
            Point result = new Point();

            result.X = rand.Next(0, ScreenWidth);

            if (underTaskbar)
            {
                result.Y = Screen.PrimaryScreen.Bounds.Height - 23;
            }
            else
            {
                result.Y = rand.Next(0, Screen.PrimaryScreen.Bounds.Height);
            }

            return result;
        }

        private Point getCursorPoint()
        {
            var cursorPoint = System.Windows.Forms.Cursor.Position;
            return new Point(cursorPoint.X - CursorOffsetX, cursorPoint.Y - CursorOffsetY);
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
                if (sSpeedY < sTopSpeed || (!isRotating && sSpeedY < sTopSpeedStraight))
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

            sToRotation = Math.Atan(y / x) * 180 / Math.PI;

            //Console.WriteLine("X:" + x + " Y:" + y + " ATAN:" + sToRotation);

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

        public void setImage()
        {
            if (sImgPause)
            {
                return;
            }

            if (sImgSkip || sImgCounter >= sImgCounterMax)
            {
                sImgSkip = false;
                sImgCounter = 0;

                if (sAnimMode == 1)
                {
                    sAnimMode = 2;
                }
                else
                {
                    sAnimMode = 1;
                }

                image.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/" + imageName + sDir.ToString() + sAnimMode.ToString() + ".png"));

                RotateTransform rotateTransform1 = new RotateTransform(sRotation);
                image.RenderTransformOrigin = new Point(0.5, 0.5);
                image.RenderTransform = rotateTransform1;
            }
            else
            {
                sImgCounter++;
            }
        }

        public void spinShark()
        {
            lockModeSwitch = false;
            if (Math.Abs(sRotation) >= 360)
            {
                sRotation = 0;
            }
            else
            {
                sRotation += 30;
            }
        }

        public void moveTowardsMousePoint()
        {
            setSharkChill(false);
            Point cursor = getCursorPoint();
            faceTarget(cursor);
            moveShark(cursor);

            if (nearByTarget(cursor, true))
            {
                if (moveTowardsCount++ > 25)
                {
                    moveTowardsCount = 0;
                    lockModeSwitch = false;
                }
            }

        }

        public void moveAtRandom()
        {
            setSharkChill(true);

            if ((randPoint.X == 0 && randPoint.Y == 0) || nearByTarget(randPoint, false, 25))
            {
                randPoint = generateRandomPoint(false);

                if (randCount++ > 5)
                {
                    randCount = 0;
                    lockModeSwitch = false;
                }
            }

            faceTarget(randPoint);
            moveShark(randPoint);
        }

        public void huntUnderTaskBar()
        {
            setSharkChill(true);

            if ((randPoint.X == 0 && randPoint.Y == 0) || nearByTarget(randPoint, false, 25))
            {
                randPoint = generateRandomPoint(true);

                if (randCount++ > 5)
                {
                    randCount = 0;
                    lockModeSwitch = false;
                }
            }

            faceTarget(randPoint);
            moveShark(randPoint);
        }

        public void updateFormLocation()
        {
            window.Left = sx;
            window.Top = sy;
        }

    }
}
