using sharkexe2.src.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Rotation = sharkexe2.src.util.Rotation;

namespace sharkexe2
{
    public class AnimObj
    {
        //Form Details
        public Window window;
        public Image imageBox;
        private String imageName = "";
        private Random rand = new Random();

        //Basic Details
        public Position objPosition;
        public Rotation objRotation;
        public SpeedUtils objSpeed;


        private int modelBubble = 5;
        private int accerlationBubble = 30;


        private int CursorOffsetX = 85;
        private int CursorOffsetY = 50;


        public AnimObj(Window window, Image imageBox, String imageName = "shark")
        {
            objSpeed = new SpeedUtils();

            this.window = window;
            this.imageBox = imageBox;
            this.imageName = imageName;

            imageBox.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "res/shark01.png"));


            rand = new Random();
            objSpeed = new SpeedUtils();
            objPosition = new Position(10, 10);
            objRotation = new Rotation();

        }

        public AnimObj()
        {
            objSpeed = new SpeedUtils();
            objPosition = new Position(0, 0);
            objRotation = new Rotation();

            Console.WriteLine("Current Position -> X:" + objPosition.X + " Y:" + objPosition.Y);
        }

        public void moveAtRandom()
        {

        }

        public void moveTowardsPosition(Position to)
        {
            objRotation.faceTowardsPosition(objPosition, to);

            objSpeed.calcVelocity(objRotation.currentRotation);

            objPosition.calcPosition(to, objSpeed.currentVelocity);
        }


        public SpeedUtils getRotationSpeed(double rotation)
        {
            Velocity v = objSpeed.calcVelocity(rotation, false);


            return objSpeed;
        }

        public void moveAtHalf()
        {
            objSpeed.setHalfVelocity(true);
        }

        public Position getCursorPoint()
        {
            var cursorPoint = System.Windows.Forms.Cursor.Position;
            return new Position(cursorPoint.X - CursorOffsetX, cursorPoint.Y - CursorOffsetY);
        }

        public void updateFormLocation()
        {
            window.Left = objPosition.X;
            window.Top = objPosition.Y;

            RotateTransform rotateBox = new RotateTransform(objRotation.currentRotation);
            imageBox.RenderTransformOrigin = new Point(0.5, 0.5);
            imageBox.RenderTransform = rotateBox;
        }

    }
}