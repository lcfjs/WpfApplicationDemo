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
using System.Windows.Shapes;

namespace WpfApplicationDemo
{
    /// <summary>
    /// FlipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FlipWindow : Window
    {
        public FlipWindow()
        {
            InitializeComponent();

            this.Loaded += FlipWindow_Loaded;
        }

        private void FlipWindow_Loaded(object sender, RoutedEventArgs e)
        {
            flipControl.FrontContent = panelFront;
            flipControl.BackContent = panelBack;
            gridBack.Children.Clear();
            gridFront.Children.Clear();
        }

        private void btnGoFront_Click(object sender, RoutedEventArgs e)
        {
            flipControl.Flip();
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            flipControl.Flip();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new MainWindow().ShowDialog();
            this.Close();
        }
    }
}
