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
            Utils.app = this;
            Utils.startBrain();
        }

    }
}
