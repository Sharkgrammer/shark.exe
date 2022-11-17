using sharkexe2.src.util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace sharkexe2
{
    /// <summary>
    /// Interaction logic for FishWindow.xaml
    /// </summary>
    public partial class FishWindow : Window
    {
        public FishWindow()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Hand;
        }

        public void paintCursorPoint(Offset points, AnimObj obj)
        {
            this.Background = Brushes.Gray;
            //this.AllowsTransparency = false;

            canvas.Children.Clear();


            TextBlock textWidth = new TextBlock();
            textWidth.Text = " (" + Utils.getObjIndex(obj) + ") <"+ this.Width + ">" ;
            textWidth.FontSize = this.Width / 5.5;

            canvas.Children.Add(textWidth);
            Canvas.SetLeft(textWidth, (this.Width / 4) - 10);
            Canvas.SetTop(textWidth, 0);

            TextBlock textHeight = new TextBlock();
            textHeight.Text = " (" + Utils.getObjIndex(obj) + ") <" + this.Height + ">";

            textHeight.FontSize = this.Height / 5.5;

            RotateTransform rotateText = new RotateTransform(90);
            textHeight.RenderTransformOrigin = new Point(0.3, 0.3);
            textHeight.RenderTransform = rotateText;

            canvas.Children.Add(textHeight);
            Canvas.SetLeft(textHeight, 0);
            Canvas.SetTop(textHeight, (this.Height / 4));

            Rectangle rec = new Rectangle()
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red
            };

            canvas.Children.Add(rec);
            Canvas.SetLeft(rec, points.X);
            Canvas.SetTop(rec, points.Y);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
            this.Activate();
            this.Focus();
        }

    }

}
