﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplicationDemo
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s1, e1) =>
            {
                ctkTran.PrevContent = ctkTran.CurrentContent;
                ctkTran.CurrentContent = DateTime.Now + "";
                ctkTran.RunTransition();
            };
            timer.Start();

            List<UserInfo> list = new List<UserInfo> {
                new UserInfo { Id=1, Name="Listbox",Age=27 },
                new UserInfo { Id=2, Name="李四",Age=33 },
                new UserInfo { Id=3, Name="王五",Age=22 }
            };
            lvData.ItemsSource = list;


            listboxDemo.DisplayMemberPath = "Name";
            listboxDemo.SelectedValuePath = "Id";
            listboxDemo.ItemsSource = list;
        }


        protected override void OnRender(DrawingContext drawingContext)
        {

            drawingContext.DrawEllipse(Brushes.Red, new Pen(Brushes.Blue, 2), new Point(300, 300), 50, 50);
            base.OnRender(drawingContext);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            //0.2 0.25 0.4 0.5
            Point pAbsolute = e.GetPosition(imgOriginal);

            double xRelative = pAbsolute.X / imgOriginal.ActualWidth;
            double yRelative = pAbsolute.Y / imgOriginal.ActualHeight;

            ib.Viewbox = new Rect(xRelative - 0.1, yRelative - 0.125, xRelative + 0.1, yRelative + 0.125);


            //DrawingVisual dv = new DrawingVisual();
            //DrawingContext dc = dv.RenderOpen();
            //dc.DrawText(new FormattedText("this is draw text!", new System.Globalization.CultureInfo("en-us"), FlowDirection.LeftToRight, new Typeface("宋体"), 16, Brushes.Red), new Point(100, 300));
            //dc.DrawEllipse(Brushes.Red, new Pen(Brushes.Blue, 2), new Point(300, 300), 50, 50);
            //this.AddVisualChild(dv);
            //dc.Close();



            

        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as System.Windows.Controls.Primitives.Thumb;
            double left = Canvas.GetLeft(thumb)+e.HorizontalChange;
            double top= Canvas.GetTop(thumb)+e.VerticalChange;
            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (left > panelLayout.ActualWidth- thumb.ActualWidth) left = panelLayout.ActualWidth - thumb.ActualWidth;
            if (top > panelLayout.ActualHeight - thumb.ActualHeight) top = panelLayout.ActualHeight - thumb.ActualHeight;
            labLocation.Text = left + ","+ top;
            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top);
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var lab = sender as TextBlock;
           
        }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public string Remark { get; set; }
    }
}
