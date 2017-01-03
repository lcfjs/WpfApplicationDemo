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
    /// PagingControl.xaml 的交互逻辑
    /// </summary>
    public partial class PagingControl : UserControl
    {

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            private set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PagingControl), new PropertyMetadata(0));



        /// <summary>
        /// 跳转至第几页(值从1开始)(点击查询时 外部重置为第一页)
        /// </summary>
        public int JumpToPage
        {
            get;
            set;
        }


        /// <summary>
        /// 总数据行数
        /// </summary>
        public int TotalRow
        {
            get { return (int)GetValue(TotalRowProperty); }
            set { SetValue(TotalRowProperty, value); SetText(); }
        }

        // Using a DependencyProperty as the backing store for TotalRow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalRowProperty =
            DependencyProperty.Register("TotalRow", typeof(int), typeof(PagingControl), new PropertyMetadata(0));


        /// <summary>
        /// 每页显示数据行数
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagingControl), new PropertyMetadata(20));

        private int totalPage = 0;

        /// <summary>
        /// 
        /// </summary>
        public event Action<int> OnPageChangedEvent;

        public PagingControl()
        {
            InitializeComponent();

            JumpToPage = 1;
        }


        private void SetText()
        {
            totalPage = TotalRow % PageSize == 0 ? TotalRow / PageSize : TotalRow / PageSize + 1;
            labTotalPage.Text = totalPage + "";
            CurrentPage = JumpToPage;
        }

        private void Jump()
        {
            if (JumpToPage > totalPage)
                JumpToPage = totalPage;
            if (JumpToPage < 1)
                JumpToPage = 1;

            if (OnPageChangedEvent != null)
            {
                OnPageChangedEvent(JumpToPage);
            }

            CurrentPage = JumpToPage;
        }

        private void txtJump_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidationJump();
            }
        }

        private void labJump_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ValidationJump();
        }

        /// <summary>
        /// 跳转到指定页之前判断
        /// </summary>
        private void ValidationJump()
        {
            int val = 0;
            if (!int.TryParse(txtJump.Text, out val))
            {
                return;
            }
            JumpToPage = val;
            if (JumpToPage > totalPage)
            {
                txtJump.Text = totalPage + "";
                JumpToPage = totalPage;
            }
            else if (JumpToPage < 1)
            {
                txtJump.Text = "1";
                JumpToPage = 1;
            }

            if (JumpToPage != CurrentPage)
                Jump();
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var ctl = sender as Border;
            if (ctl == null) return;

            switch (ctl.Name)
            {
                case "pageFirst":
                    JumpToPage = 1;
                    break;
                case "pagePrev":
                    JumpToPage = JumpToPage > 1 ? JumpToPage -= 1 : JumpToPage;
                    break;
                case "pageNext":
                    JumpToPage = JumpToPage < totalPage ? JumpToPage += 1 : JumpToPage;
                    break;
                case "pageLast":
                    JumpToPage = totalPage;
                    break;
                default:
                    break;
            }
            if (JumpToPage != CurrentPage)
                Jump();
        }

        private bool isValueChange = false;
        private void comPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isValueChange = true;
        }

        private void comPageSize_DropDownClosed(object sender, EventArgs e)
        {
            if (isValueChange)
            {
                SetText();
                Jump();
                isValueChange = false;
            }
        }
    }
}
