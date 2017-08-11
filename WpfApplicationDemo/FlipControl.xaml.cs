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
    /// FlipControl.xaml 的交互逻辑
    /// </summary>
    public partial class FlipControl : UserControl
    {
        private bool isFront = true;


        private Object frontContent;

        /// <summary>
        /// 前面内容
        /// </summary>
        public Object FrontContent
        {
            get { return frontContent; }
            set
            {
                frontContent = value;
                this.transition1.Content = frontContent;
            }
        }


        /// <summary>
        /// 背面内容
        /// </summary>
        public Object BackContent { get; set; }

        public FlipControl()
        {
            InitializeComponent();

            this.Loaded += FlipControl_Loaded;
        }

        private void FlipControl_Loaded(object sender, RoutedEventArgs e)
        {
            transition1.Transition = new Transitionals.Transitions.RotateTransition() { Angle = 0, Direction = Transitionals.Transitions.RotateDirection.Up, Contained = true };
        }

        public void Flip()
        {
            if (isFront)
            {
                this.transition1.Content = BackContent;
                isFront = false;
            }
            else
            {
                this.transition1.Content = FrontContent;
                isFront = true;
            }
        }
    }
}
