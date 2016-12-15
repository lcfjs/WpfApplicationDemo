using Microsoft.Practices.Prism.Commands;
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
            this.DataContext = this;

            //WindowBehaviorHelper w = new WindowBehaviorHelper(this);
            //w.RepairBehavior();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new WindowAsync().ShowDialog();



            System.Windows.Controls.Primitives.Popup p; 
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


            var os = Environment.Is64BitOperatingSystem;
            Console.WriteLine("Is64BitOperatingSystem:" + os);
            Console.WriteLine(Environment.OSVersion.VersionString);
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.OSVersion.Platform);
            Console.WriteLine(Environment.ProcessorCount);
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(Environment.SystemDirectory);
            Console.WriteLine(Environment.SystemPageSize);
            Console.WriteLine(Environment.TickCount/60000);
            Console.WriteLine(Environment.UserDomainName);
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            foreach (var item in Environment.GetLogicalDrives())
            {
                Console.WriteLine(item);
            }

        }


        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

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



        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show((sender as FrameworkElement).Name);
        }
        

        private void btnRoute_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("btnRoute_Click:"+ (sender as FrameworkElement).Name);
        }

        private void localMR_NewClick(object sender, MyEventArgs e)
        {
            e.Tag = (sender as FrameworkElement).Name;
            System.Windows.Forms.MessageBox.Show((sender as FrameworkElement).Name);

        }

        private void localMR_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Test");
            DragDrop.DoDragDrop(localMR, localMR.Content, DragDropEffects.Copy);
            e.Handled = true;


            //CheckBox chk = new CheckBox();
            var w = App.Current.MainWindow;
            pInfo.IsOpen = true;
        }


        #region Command Test

        //1.
        public ICommand ButtonMouseDown { get { return new DelegateCommand<CommandParameterEx>((parameter)=> { ButtonMouseDownMethod(parameter); }); } }

        private void ButtonMouseDownMethod(CommandParameterEx parameter)
        {
            var btn = parameter.Sender as Button;
            btn.Content = " woow";
            //
            System.Windows.Forms.MessageBox.Show(parameter+"");

            //System.Windows.Forms.MessageBox.Show(SystemParameters.FullPrimaryScreenHeight + ","+ SystemParameters.PrimaryScreenHeight+","+ SystemParameters.WorkArea.Height);
            //System.Windows.Forms.MessageBox.Show((parameter.Sender as FrameworkElement).Name+"");
        }

        //2.
        public RelayCommand ButtonMouseDown2 { get { return new RelayCommand((parameter) => { BMDMethod2(parameter); }); } }

        private void BMDMethod2(object parameter)
        {
            System.Windows.Forms.MessageBox.Show(parameter + "");
            
        }


        #endregion

        private void chkKeyboardC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Y && e.KeyboardDevice.Modifiers== ModifierKeys.Control)
            {
                chkKeyboardC.IsChecked = !chkKeyboardC.IsChecked;
            }


        }

        private void chkSlider_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnWindow1_Click(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
        }

        private void ThumbButtonInfo_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }
    }
}
