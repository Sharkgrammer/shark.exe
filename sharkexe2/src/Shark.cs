using System;
using System.Windows;
using System.Windows.Controls;

namespace sharkexe2
{
    class Shark : AnimObj
    {

        public Shark(Window window, Image imageBox, String imageName = "shark") : base(window, imageBox, imageName)
        {
            
        }

        public Shark()
        {

        }

        public void huntCursor()
        {
            this.moveTowardsPosition(this.getCursorPoint());

            Console.WriteLine();
            Console.WriteLine(objPosition.toString());
            Console.WriteLine(objRotation.toString());
            Console.WriteLine(objSpeed.toString());
            Console.WriteLine();

        }
    }
}
