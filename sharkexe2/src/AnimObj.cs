using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class AnimObj
    {
        public Window window;
        public Image imageBox;
        public Boolean debug = false;

        public Position objPosition;
        public Position toPosition;
        public Rotation objRotation;
        public Offset objOffset;
        public SpeedUtils objSpeed;

        public Boolean flipCorrectionLeft = false;
        public Boolean flipCorrectionRight = false;
        public int flipCorrectionCounter = 0;

        public AnimObj(Window window, Image imageBox)
        {
            this.window = window;
            this.imageBox = imageBox;
        }

        public void moveTowardsPosition(Position to)
        {
            toPosition = to;
            objRotation.faceTowardsPosition(objPosition, to);

            bool decX = objPosition.nearByPositionX(to, objSpeed.getDecerlationBubble());
            bool decY = objPosition.nearByPositionY(to, objSpeed.getDecerlationBubble());

            objSpeed.calcVelocity(objRotation.currentRotation, decX, decY);

            objPosition.calcPosition(to, objSpeed.currentVelocity, 10);
        }

        public Position getCursorPoint()
        {
            var cursorPoint = System.Windows.Forms.Cursor.Position;
            return new Position(cursorPoint.X - objOffset.X, cursorPoint.Y - objOffset.Y);
        }

        public void update()
        {
            if (this.imageBox.Visibility == Visibility.Hidden)
            {
                this.imageBox.Visibility = Visibility.Visible;
            }

            window.Left = objPosition.X;
            window.Top = objPosition.Y;

            manageImage();
            animateObj();

            if (debug) runDebug();
        }

        private void manageImage()
        {
            if (toPosition.active)
            {
                if (!objPosition.nearByPosition(toPosition, window.Width / 3))
                {
                    objOffset.adjustOffset(objRotation.currentRotation);

                    RotateTransform rotateBox;
                    ScaleTransform scaleBox = new ScaleTransform();

                    if (Math.Round(objPosition.X, 0) < Math.Round(toPosition.X, 0))
                    {
                        scaleBox.ScaleX = 1;
                        rotateBox = new RotateTransform(objRotation.currentRotation);
                        flipCorrectionLeft = true;

                        if (flipCorrectionRight)
                        {
                            flipCorrectionCounter++;
                            objRotation.forceFaceTowardsPosition(objPosition, toPosition);

                            if (flipCorrectionCounter > 2)
                            {
                                flipCorrectionCounter = 0;
                                flipCorrectionRight = false;
                            }
                        }
                    }
                    else
                    {
                        scaleBox.ScaleX = -1;
                        objOffset.flip(window.Width, window.Height);
                        rotateBox = new RotateTransform(360 - objRotation.currentRotation);
                        flipCorrectionRight = true;

                        if (flipCorrectionLeft)
                        {
                            flipCorrectionCounter++;
                            objRotation.forceFaceTowardsPosition(objPosition, toPosition);

                            if (flipCorrectionCounter > 2)
                            {
                                flipCorrectionCounter = 0;
                                flipCorrectionLeft = false;
                            }
                        }
                    }

                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(rotateBox);
                    transformGroup.Children.Add(scaleBox);

                    imageBox.RenderTransform = transformGroup;
                }
            }
            else
            {
                imageBox.RenderTransform = new RotateTransform(objRotation.currentRotation);
            }
        }

        public virtual void animateObj()
        {
            // Extend to animate what the obj does
        }

        private void runDebug()
        {
            Console.WriteLine();
            Console.WriteLine(this.ToString() + "<" + this.GetHashCode() + "> -> " + imageBox.Source);
            Console.WriteLine(objOffset.toString());
            Console.WriteLine(objPosition.toString());
            Console.WriteLine(toPosition.toString());
            Console.WriteLine(objRotation.toString());
            Console.WriteLine(objSpeed.toString());
            Console.WriteLine();

            ((FishWindow) window).paintCursorPoint(objOffset);
        }

    }
}