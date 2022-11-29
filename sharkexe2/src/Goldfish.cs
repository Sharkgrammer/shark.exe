using sharkexe2.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;
using Color = System.Drawing.Color;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace sharkexe2
{
    public class Goldfish : AnimObj
    {
        private int animatecounter = 0;
        private int animateFrameLen = 12;
        private int animateMode = 1;

        private int actionMode = 0;
        private int actionModeAmt = 2;
        private int actionCounter = 0;
        private int size;

        public Boolean inSchool = false;
        public Boolean schoolLeader = false;
        public int schoolPos = 0;
        public Boolean fleeing = false;

        public GoldfishFrame mainFrames;
        private String[] allColours = { "#FF0000", "#FFA500", "#FFFF00", "#008000", "#0000FF", "#4B0082", "#EE82EE" ,"#FF9245" };

        private int rainbowCurrent = 0;
        private List<GoldfishFrame> allRainbowColours = new List<GoldfishFrame>();

        public Goldfish(Window window, Image imageBox) : base(window, imageBox)
        {

            this.size = Utils.random.Next(50, 120);            

            int offset1, offset2, offset3;

            offset1 = size - 25;
            offset2 = size / 2;
            offset3 = offset1 / 2;

            objOffset = new Offset(offset1, offset2, offset3);

            window.Width = size;
            window.Height = size;
            imageBox.Width = size;

            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);

            if (Utils.random.Next(10) >= 9)
            {
                setupRainbowList();
                mainFrames = allRainbowColours[rainbowCurrent];
            }
            else
            {
                mainFrames = new GoldfishFrame(allColours[Utils.random.Next(0, allColours.Count())]);
            }

            imageBox.Source = mainFrames.f1;

            int maxSpeed = Utils.random.Next(2, 4);
            animateFrameLen = 20 - (maxSpeed * 2);

            // Setup the obj varaibles
            objPosition = new Position(objOffset, true);
            objRotation = new Rotation(maxSpeed / 2);
            objSpeed = new SpeedUtils(maxSpeed);
            toPosition = objPosition;

            actionMode = Utils.random.Next(0, 2);
        }

        public override void runActions()
        {
            fleeing = scatterIfSharksNear();

            if (fleeing)
            {
                return;
            }

            switch (actionMode)
            {
                case 0:
                    wanderScreen();
                    break;
                case 1:
                    schoolBehaviour();
                    break;
                default:
                    actionMode = 0;
                    break;
            }

            if (objPosition.nearByPosition(toPosition, 10))
            {
                if (actionCounter > 15)
                {
                    actionCounter = 0;
                    actionMode++;

                    if (schoolLeader)
                    {
                        this.schoolLeader = false;
                        this.schoolPos = 0;
                    }
                }

                if (actionMode == 1)
                {
                    if (schoolLeader)
                    {
                        actionCounter++;
                    }
                }
                else
                {
                    actionCounter++;
                }
            }
        }

        public void wanderScreen()
        {
            if (objPosition.nearByPosition(toPosition, 10))
            {
                int count = Utils.random.Next(0, 11);

                if (count > 5)
                {
                    toPosition = new Position(objOffset, true);
                }
            }

            this.moveTowardsPosition(toPosition);
        }

        public void schoolBehaviour()
        {
            if (!inSchool)
            {
                if (!schoolLeaderExists())
                {
                    this.schoolLeader = true;
                    this.schoolPos = 1;
                }
                else
                {
                    joinSchool();
                }

                this.inSchool = true;
            }
            else
            {
                if (schoolLeader)
                {
                    this.wanderScreen();
                }
                else
                {
                    Position tempPos = getAheadFishPos();

                    if (tempPos != null)
                    {
                        toPosition = tempPos;
                        if (!objPosition.nearByPosition(toPosition, this.size / 2))
                        {
                            this.moveTowardsPosition(toPosition);
                        }
                    }

                    if (!schoolLeaderExists())
                    {
                        this.schoolPos = 0;
                        this.inSchool = false;
                        this.actionMode++;
                    }
                }
            }
        }

        public Boolean scatterIfSharksNear()
        {
            List<Shark> sharks = sharksNearby();

            //Console.WriteLine(sharks.Count);

            if (sharks.Count == 0)
            {
                return false;
            }
            else
            {
                Position tempPos = objPosition.getFleePosition(sharks[0].objPosition, size * 2);

                // objRotation.forceFaceTowardsPosition(objPosition, tempPos, true);

                this.moveTowardsPosition(tempPos);

                return true;
            }
        }

        private Boolean schoolLeaderExists()
        {
            return !(Utils.getListOfType(typeof(Goldfish)).Where(g => ((Goldfish) g).schoolLeader).Count() == 0);
        }

        private void joinSchool()
        {
            this.schoolPos = allFishInSchool().Count() + 1;
        }

        private Position getAheadFishPos()
        {
            return getAheadFishPos(this.schoolPos - 1);
        }

        private Position getAheadFishPos(int pos)
        {
            Position tempPos = Utils.getListOfType(typeof(Goldfish)).Where(g => ((Goldfish) g).schoolPos == pos).Select(g => g.objPosition).FirstOrDefault();

            if (tempPos == null && pos != 1)
            {
                return getAheadFishPos(1);

            }
            else
            {
                return tempPos;
            }
        }

        private List<Goldfish> allFishInSchool()
        {
            return Utils.getListOfType(typeof(Goldfish)).Select(g => (Goldfish)g).Where(g => g.inSchool).ToList();
        }

        private List<Shark> sharksNearby()
        {
            return Utils.getListOfType(typeof(Shark)).Select(s => (Shark)s).Where(s => objPosition.nearByPosition(s.objPosition, size * 2)).ToList();
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

                if (allRainbowColours.Count > 0)
                {
                    if (rainbowCurrent < allRainbowColours.Count() - 1)
                    {
                        rainbowCurrent++;
                    }
                    else
                    {
                        rainbowCurrent = 0;
                    }

                    mainFrames = allRainbowColours[rainbowCurrent];
                }
            }

            imageBox.Source = animateMode == 1 ? mainFrames.f1 : mainFrames.f2;
        }

        private void setupRainbowList()
        {
            String tempCol = allColours[0];
            int count = 0, jump = Utils.random.Next(20, 41);

            allRainbowColours.Add(new GoldfishFrame(tempCol));

            do
            {
                tempCol = Utils.fadeToColour(tempCol, allColours[count + 1], jump);

                if (tempCol == allColours[count + 1])
                {
                    count++;

                    if (count >= allColours.Count() - 1)
                    {
                        count = -1;
                    }
                }

                allRainbowColours.Add(new GoldfishFrame(tempCol));

            } while (tempCol != allColours[0]);

            rainbowCurrent = Utils.random.Next(0, allRainbowColours.Count() - 2);
        }

        public override String childDebug()
        {
            return "Goldfish Idx:" + Utils.getObjIndex(this) + " ActionMode:" + actionMode + " School:" + inSchool + " Leader:" + schoolLeader + " SchoolIdx:" + schoolPos + " Fleeing:" + fleeing;
        }

    }

    public class GoldfishFrame
    {
        public ImageSource f1, f2;
        public String colourName;

        public GoldfishFrame(String name)
        {
            this.colourName = name;

            setFrames();
        }

        private void setFrames()
        {
            // From is the grey used by the default fish
            Color from = ColorTranslator.FromHtml("#ffc8c8c8");
            Color to = ColorTranslator.FromHtml(this.colourName);

            Bitmap b1 = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "res/fish01.png");
            Bitmap b2 = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "res/fish02.png");

            this.f1 = Utils.ChangeColor(b1, from, to);
            this.f2 = Utils.ChangeColor(b2, from, to);
        }

    }
}
