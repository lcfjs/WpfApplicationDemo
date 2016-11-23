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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControlToolkit
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ControlToolkit"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ControlToolkit;assembly=ControlToolkit"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:Transition/>
    ///
    /// </summary>
    public class Transition : Control
    {
        static Transition()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Transition), new FrameworkPropertyMetadata(typeof(Transition)));
        }


        /// <summary>
        /// 过度前内容
        /// </summary>
        public string PrevContent
        {
            get { return (string)GetValue(PrevContentProperty); }
            set { SetValue(PrevContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrevContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrevContentProperty =
            DependencyProperty.Register("PrevContent", typeof(string), typeof(Transition), new PropertyMetadata(""));


        /// <summary>
        /// 当前要展示的内容
        /// </summary>
        public string CurrentContent
        {
            get { return (string)GetValue(CurrentContentProperty); }
            set { SetValue(CurrentContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentContentProperty =
            DependencyProperty.Register("CurrentContent", typeof(string), typeof(Transition), new PropertyMetadata(""));


        /// <summary>
        /// 是否可以过度
        /// </summary>
        public bool CanTransition
        {
            get { return (bool)GetValue(CanTransitionProperty); }
            set { SetValue(CanTransitionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanTransition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanTransitionProperty =
            DependencyProperty.Register("CanTransition", typeof(bool), typeof(Transition), new PropertyMetadata(true));




        /// <summary>
        /// 执行内容过度
        /// </summary>
        public void RunTransition()
        {
            if (CanTransition != true)
                CanTransition = true;
            CanTransition = false;
        }
    }
}
