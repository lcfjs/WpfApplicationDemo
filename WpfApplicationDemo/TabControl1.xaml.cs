using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplicationDemo
{
    /// <summary>
    /// TabControl1.xaml 的交互逻辑
    /// </summary>
    public partial class TabControl1 : Window
    {
        public TabControl1()
        {
            InitializeComponent();


            Random r = new Random();
            for (int i = 0; i < 50; i++)
            {
                TextBlock lab = new TextBlock();
                lab.Text = DateTime.Now + new string('-', 10 + r.Next(52));
                panelContent.Children.Add(lab);
            }
        }
    }
}
