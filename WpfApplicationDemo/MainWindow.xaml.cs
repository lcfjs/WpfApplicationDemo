using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApplicationDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new WindowAsync().ShowDialog();


            


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Test[] arr = new[] { new Test { Age = 20, Name = "zhangsan" }, new Test { Age = 21, Name = "wangwu" } };
            string str = "";
            foreach (var item in arr)
            {
                str += item.Name + ",";
            }
            Console.WriteLine(str);
            str = "";

            ChangeVal(arr[0]);
            foreach (var item in arr)
            {
                str += item.Name + ",";
            }
            Console.WriteLine(str);
            str = "";


            List<Test> list = new List<Test>();
            for (int i = 0; i < 133; i++)
            {
                list.Add(new Test {Age = i, Name=(i+500) +"" });
            }
            dgData.ItemsSource = list.Take(pageControl.PageSize).ToList();
            pageControl.TotalRow = list.Count;
            pageControl.OnPageChangedEvent += (pageIndex) => {
                dgData.ItemsSource = list.Skip(pageControl.PageSize*(pageIndex - 1)).Take(pageControl.PageSize).ToList();
            };
        }

        private void ChangeVal( Test val)
        {
            val.Name = "world";
            val = new Test { Age = 10, Name = "hello" };
        }

        class Test {
            public int Age { get; set; }
            public string Name { get; set; }
        }

        struct ValDemo
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            new TabControl1().ShowDialog();

            System.Windows.Controls.Primitives.Thumb t = new  System.Windows.Controls.Primitives.Thumb();
            
        }

        private void btnVisual_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnVisual_Click");
        }
    }
}
