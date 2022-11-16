using sharkexe2.src.globalUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace sharkexe2.src.util
{
    public static class Utils
    {
        private static readonly int tickSpeed = 10;
        
        public static Random random = new Random();
        public static List<objThread> objList = new List<objThread>();
        public static App app { get; set; }

        public static void addNewObj(AnimObj obj, App app)
        {
            System.Timers.Timer timer = new System.Timers.Timer(tickSpeed);

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

            Utils.addNewObj(shark, app);
        }

        public static void createNewCoral()
        {
            FishWindow window = createFishWindow();

            Coral coral = new Coral(window, window.imgMain);

            Utils.addNewObj(coral, app);
        }
    }
}
