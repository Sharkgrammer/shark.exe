﻿using sharkexe2.src.util;
using System.Timers;
using System;

namespace sharkexe2.src.globalUtils
{
    public class objThread 
    {

        public AnimObj animObj { get; set; }

        public Timer timer { get; set; }

        public App app { get; set; }    

        public objThread(AnimObj animObj, Timer timer, App app)
        {
            this.animObj = animObj;
            this.app = app;
            this.timer = timer;

            this.start();
        }

        public void start()
        {
            timer.Elapsed += timer_thread;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        public void close()
        {
            timer.Stop();
            animObj.window.Close();
        }

        public void timer_thread(object sender, ElapsedEventArgs e)
        {
            if (animObj.pauseObj) return;

            animObj.runActions();
            Utils.runMethodInApp(animObj.update);
            

            if (animObj.deleteObj)
            {
                Utils.objList.Remove(this);
                Utils.runMethodInApp(this.close);
            }
        }

    }
}
