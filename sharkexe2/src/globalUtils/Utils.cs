using sharkexe2.src.globalUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace sharkexe2.src.util
{
    public static class Utils
    {
        private static readonly int tickSpeed = 10;
        
        public static Random random = new Random();
        public static List<objThread> objList = new List<objThread>();
        public static App app { get; set; }
        public static Timer brainTimer { get; set; }

        private static int maxBubbles = 7;
        private static int bubblesFrames = 30;
        private static int bubblesCounter = 0;
        private static int maxCoral = 7;

        public static void startBrain()
        {
            int counter = 0;

            while (counter++ <= 2)
            {
                int rand = Utils.random.Next(0, 2);

                createNewShark(rand == 0 ? "shark" : "shark");
            }

            brainTimer = new Timer(tickSpeed * 10);
            brainTimer.Elapsed += brain_thread;
            brainTimer.AutoReset = true;
            brainTimer.Enabled = true;
            brainTimer.Start();
        }

        public static void brain_thread(object sender, ElapsedEventArgs e)
        {
            if (objList.Count == 0)
            {
                brainTimer.Stop();
            }

            // Bubble code
            var bubbleCount = 0;
            try
            {
                bubbleCount = (from obj in objList where obj.animObj.GetType() == typeof(Bubble) select obj).Count();
            } 
            catch (Exception)
            {
                // Another thread modified objList whilst Linq was running
                bubbleCount = maxBubbles;
            }

            if (bubbleCount < maxBubbles)
            {
                if (bubblesCounter >= bubblesFrames){
                    bubblesCounter = 0;
                    runMethodInApp(createNewBubble);
                }
                else
                {
                    bubblesCounter++;
                }
            }

            // Coral code
            var coralCount = 0;
            try
            {
                coralCount = (from obj in objList where obj.animObj.GetType() == typeof(Coral) select obj).Count();
            }
            catch (Exception)
            {
                // Another thread modified objList whilst Linq was running
                coralCount = maxCoral;
            }

            if (coralCount < maxCoral)
            {
                runMethodInApp(createNewCoral);
            }

            Console.WriteLine("ObjCount: " + objList.Count + " BubbleCount: " + bubbleCount + " CoralCount: " + coralCount);
        }

            public static void addNewObj(AnimObj obj, App app)
        {
            Timer timer = new Timer(tickSpeed);

            objThread objThread = new objThread(obj, timer, app);

            objList.Add(objThread);
        }

        public static int getObjIndex(AnimObj obj)
        {
            return objList.FindIndex(t => t.animObj == obj);
        }

        public static int? getRandomObj(AnimObj yourObj)
        {
            if (objList.Count == 1)
            {
                return null;
            }
            int yourIndex = objList.FindIndex(t => t.animObj == yourObj);

            int index = yourIndex;

            while(index == yourIndex)
            {
                index = random.Next(0, objList.Count);
            }

            return index;
        }

        public static AnimObj getObj(int? index)
        {
            if (index != null && objList.Count > index)
            {
                return objList[(int) index].animObj;
            }
            else
            {
                return null;
            }
        }

        public static Shark getShark(int index)
        {
            AnimObj obj = objList[index].animObj;

            if (obj is Shark)
            {
                return (Shark) obj;
            }
            else
            {
                return null;
            }
        }

        public static void runMethodInApp(Action method)
        {
            app.Dispatcher.BeginInvoke(new Action(method));
        }

        private static FishWindow createFishWindow()
        {
            FishWindow window = new FishWindow();
            window.Show();

            return window;
        }

        public static void createNewShark(String name = "shark")
        {
            FishWindow window = createFishWindow();

            Shark shark = new Shark(window, window.imgMain, name);
            shark.debug = false;

            addNewObj(shark, app);
        }

        public static void createNewCoral()
        {
            FishWindow window = createFishWindow();

            Coral coral = new Coral(window, window.imgMain);

            addNewObj(coral, app);
        }

        public static void createNewBubble()
        {
            FishWindow window = createFishWindow();

            Bubble bubble = new Bubble(window, window.imgMain);

            addNewObj(bubble, app);
        }
    }
}
