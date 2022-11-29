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
using System.Globalization;
using System.Windows;
using Point = System.Drawing.Point;

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

            for (int x = 0; x < random.Next(0, 4); x++)
            {
                createNewShark();
            }

            for (int x = 0; x < random.Next(0, 6); x++)
            {
                createNewGoldfish();
            }

            for (int x = 0; x < random.Next(0, 10); x++)
            {
                createNewCoral();
            }

            for (int x = 0; x < random.Next(0, 3); x++)
            {
                createNewCastle();
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

        public static String fadeToColour(String currentColour, String toColour, int fadeDist = 15)
        {
            int cr, cg, cb, tr, tg, tb;

            cr = int.Parse(currentColour.Substring(1, 2), NumberStyles.AllowHexSpecifier);
            cg = int.Parse(currentColour.Substring(3, 2), NumberStyles.AllowHexSpecifier);
            cb = int.Parse(currentColour.Substring(5, 2), NumberStyles.AllowHexSpecifier);

            tr = int.Parse(toColour.Substring(1, 2), NumberStyles.AllowHexSpecifier);
            tg = int.Parse(toColour.Substring(3, 2), NumberStyles.AllowHexSpecifier);
            tb = int.Parse(toColour.Substring(5, 2), NumberStyles.AllowHexSpecifier);

            cr = fadeToColourHelper(cr, tr, fadeDist);
            cg = fadeToColourHelper(cg, tg, fadeDist);
            cb = fadeToColourHelper(cb, tb, fadeDist);
            
            String crStr = $"{cr:X}";
            String cgStr = $"{cg:X}";
            String cbStr = $"{cb:X}";

            if (crStr.Length == 1) crStr = "0" + crStr;
            if (cgStr.Length == 1) cgStr = "0" + cgStr;
            if (cbStr.Length == 1) cbStr = "0" + cbStr;

            // Return hex
            return "#" + crStr + cgStr + cbStr;
        }

        private static int fadeToColourHelper(int current, int to, int fadeDist)
        {

            if (current < to)
            {
                current += fadeDist;

                if (current > 255)
                {
                    current = 255;
                } 
                else if (current > to)
                {
                    current = to;
                }
            }
            else
            {
                current -= fadeDist;

                if (current < 0)
                {
                    current = 0;
                } 
                else if (current < to)
                {
                    current = to;
                }
            }

            return current;
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

        public static void createNewCastle()
        {
            FishWindow window = createFishWindow();

            Castle castle = new Castle(window, window.imgMain);
            castle.debug = false;

            addNewObj(castle, app);
        }

    }
}
