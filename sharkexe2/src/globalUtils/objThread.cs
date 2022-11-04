using sharkexe2.src.util;
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
            animObj.delete();
        }

        public void timer_thread(object sender, ElapsedEventArgs e)
        {
            if (animObj.pauseObj) return;

            animObj.runActions();
            app.Dispatcher.BeginInvoke(new Action(animObj.update));
            

            if (animObj.deleteObj)
            {
                Utils.objList.Remove(this);
                app.Dispatcher.BeginInvoke(new Action(this.close));
            }
        }

    }
}
