using sharkexe2.src.globalUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.IO;
using System.Windows.Media.Imaging;

namespace sharkexe2.src.util
{
    public static class Utils
    {
        private static readonly int tickSpeed = 10;
        
        public static Random random = new Random();
        private static List<objThread> objList = new List<objThread>();
        public static App app { get; set; }
        public static Timer brainTimer { get; set; }

        private static int bubblesFrames = 70;
        private static int bubblesCounter = 0;

        public static void startBrain()
        {

            for (int x = 0; x < 1; x++)
            {
                int rand = Utils.random.Next(0, 2);

                createNewShark(rand == 0 ? "shark" : "shark");
            }

            for (int x = 0; x < 3; x++)
            {
                createNewGoldfish();
            }

            for (int x = 0; x < 2; x++)
            {
                createNewCoral();
            }

            createNewBubble();

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

            // Bubble code -> Generate bubble every bubblesFrame tick
            if (bubblesCounter >= bubblesFrames){
                bubblesCounter = 0;
                runMethodInApp(createNewBubble);
            }
            else
            {
                bubblesCounter++;
            }
        }

        public static void addNewObj(AnimObj obj, App app)
        {
            Timer timer = new Timer(tickSpeed);

            objThread objThread = new objThread(obj, timer, app);

            objList.Add(objThread);
        }

        public static void removeObj(objThread obj)
        {
            objList.Remove(obj);
        }

        public static List<AnimObj> getListOfType(Type type)
        {
            try
            {
                return objList.Where(t => t.animObj.GetType() == type).Select(t => t.animObj).ToList();
            }
            catch (Exception)
            {
                // Another thread modifed objList during this. Come back and try again later
                return null;
            }
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

        public static BitmapImage ChangeColor(Bitmap image, Color fromColor, Color toColor)
        {
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetRemapTable(new ColorMap[]
            {
                new ColorMap()
                {
                    OldColor = fromColor,
                    NewColor = toColor,
                }
            }, ColorAdjustType.Bitmap);


            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(image, new Rectangle(Point.Empty, image.Size), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
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

            addNewObj(shark, app);
        }

        public static void createNewCoral()
        {
            FishWindow window = createFishWindow();

            Coral coral = new Coral(window, window.imgMain);
            coral.debug = false;

            addNewObj(coral, app);
        }

        public static void createNewBubble()
        {
            FishWindow window = createFishWindow();

            Bubble bubble = new Bubble(window, window.imgMain);
            bubble.debug = false;

            addNewObj(bubble, app);
        }

        public static void createNewGoldfish()
        {
            FishWindow window = createFishWindow();

            Goldfish fish = new Goldfish(window, window.imgMain);
            fish.debug = false;

            addNewObj(fish, app);
        }
    }
}
