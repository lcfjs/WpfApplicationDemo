﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplicationDemo"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplicationDemo;assembly=WpfApplicationDemo"
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
    ///     <MyNamespace:Loading/>
    ///
    /// </summary>
    public class Loading : Control
    {
        static Loading()
        {
            //重载默认样式
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading)));
            //DependencyProperty 注册 FillColor
            FillColorProperty = DependencyProperty.Register("FillColor",
            typeof(Color),
            typeof(Loading),
            new UIPropertyMetadata(Colors.DarkBlue,
            new PropertyChangedCallback(OnUriChanged))
            );
            //Colors.DarkBlue为控件初始化默认值

        }
        //属性变更回调函数
        private static void OnUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Border b = (Border)d;
            //MessageBox.Show(e.NewValue.ToString());

        }
        #region 自定义Fields
        // DependencyProperty属性定义 FillColorProperty=FillColor+Property组成
        public static readonly DependencyProperty FillColorProperty;
        #endregion
        //VS设计器属性支持
        [Description("背景色"), Category("个性配置"), DefaultValue("#FF18A9E4")]
        public Color FillColor
        {
            //GetValue,SetValue为固定写法，此处一般不建议处理其他逻辑
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        private Grid grid = null;

        public Loading(Grid grid, Action operate)
        {
            this.grid = grid;
            operate?.BeginInvoke(OnComplate, null);
        }
        private void OnComplate(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                for (int i = grid.Children.Count - 1; i > -1; i--)
                {
                    if (grid.Children[i] is Loading)
                    {
                        grid?.Children.Remove(grid.Children[i] as Loading);
                        break;
                    }
                }
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operate"></param>
        public static void Show(Grid grid, Action operate)
        {
            Loading loading = new Loading(grid, operate);
            loading.Background = new SolidColorBrush(Color.FromArgb(125, 0, 0, 0));
            int rowCount = grid.RowDefinitions.Count > 0 ? grid.RowDefinitions.Count : 1;
            int columnCount = grid.ColumnDefinitions.Count > 0 ? grid.ColumnDefinitions.Count : 1;
            Grid.SetRowSpan(loading, rowCount);
            Grid.SetColumnSpan(loading, columnCount);
            Panel.SetZIndex(loading, 999);
            grid.Children.Add(loading);

        }

        public static void Show(DependencyObject obj, Action operate)
        {
            var grid = GetFirstGrid(obj);
            if (grid != null)
            {
                Loading loading = new Loading(grid, operate);
                loading.Background = new SolidColorBrush(Color.FromArgb(125, 0, 0, 0));
                int rowCount = grid.RowDefinitions.Count > 0 ? grid.RowDefinitions.Count : 1;
                int columnCount = grid.ColumnDefinitions.Count > 0 ? grid.ColumnDefinitions.Count : 1;
                Grid.SetRowSpan(loading, rowCount);
                Grid.SetColumnSpan(loading, columnCount);
                Panel.SetZIndex(loading, 999);
                grid.Children.Add(loading);
            }

        }

        private static Grid GetFirstGrid(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var control = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                if (control is Grid)
                {
                    return control as Grid;
                }
                else
                {
                    var item = GetFirstGrid(control);
                    if (item != null)
                        return item;
                }
            }
            return null;
        }
    }

}
