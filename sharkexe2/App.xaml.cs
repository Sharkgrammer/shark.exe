using System;
using System.Collections.Generic;
using Application = System.Windows.Application;
using sharkexe2.src.util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace sharkexe2
{
    public partial class App : Application
    {
       
        public App()
        {
            int counter = 0;

            Utils.app = this;

            while(counter++ <= 0)
            {
                int rand = Utils.random.Next(0, 2);

                Utils.createNewShark(rand == 0 ? "shark" : "blahaj");
            }

            counter = 0;

            while (counter++ <= 20)
            {
                Utils.createNewCoral();
            }

        }

    }
}
