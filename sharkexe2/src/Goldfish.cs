﻿using sharkexe2.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

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
            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/fish01.png"));

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

            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/fish0" + animateMode + ".png"));
        }

        public override String childDebug()
        {
            return "Goldfish Idx:" + Utils.getObjIndex(this) + " ActionMode:" + actionMode + " School:" + inSchool + " Leader:" + schoolLeader + " SchoolIdx:" + schoolPos;
        }
    }
}