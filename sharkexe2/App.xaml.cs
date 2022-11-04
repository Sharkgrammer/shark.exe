using System;
using System.Collections.Generic;
using Application = System.Windows.Application;
using sharkexe2.src.util;

namespace sharkexe2
{
    public partial class App : Application
    {
       
        public App()
        {
            int counter = 0;

            Utils.app = this;

            while(counter++ <= 19)
            {
                int rand = Utils.random.Next(0, 2);

                Utils.createNewShark(rand == 0 ? "shark" : "blahaj");
            }

        }

    }
}
