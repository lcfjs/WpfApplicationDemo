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
    /// PageControl.xaml 的交互逻辑
    /// </summary>
    public partial class PageControl : UserControl
    {
        private int pageSize = 20;
        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            //set { pageSize = value; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        private int currentIndex = 1;

        /// <summary>
        /// 将要跳转的页
        /// </summary>
        private int goIndex = 1;

        /// <summary>
        /// 总页数
        /// </summary>
        private int totalPage = 0;

        /// <summary>
        /// 总数据
        /// </summary>
        private int totalRow = 0;

        public event Action<int> OnPageChangedEvent;

        public PageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="total"></param>
        public void Init(int total)
        {
            totalRow = total;
            totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
            labTotalPage.Text = totalPage + "";
            labTotal.Text = total + "";
        }

        /// <summary>
        /// 翻页
        /// </summary>
        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var ctl = sender as Border;
            if (ctl == null) return;

            switch (ctl.Name)
            {
                case "pageFirst":
                    goIndex = 1;
                    break;
                case "pagePrev":
                    goIndex = goIndex > 1 ? goIndex -= 1 : goIndex;
                    break;
                case "pageNext":
                    goIndex = goIndex < totalPage ? goIndex += 1 : goIndex;
                    break;
                case "pageLast":
                    goIndex = totalPage;
                    break;
                default:
                    break;
            }
            if (goIndex != currentIndex)
                Page();
        }


        private void Page()
        {
            if (goIndex > totalPage)
                goIndex = totalPage;
            if (goIndex < 1)
                goIndex = 1;

            if (OnPageChangedEvent != null)
            {
                OnPageChangedEvent(goIndex);
            }

            currentIndex = goIndex;

            labPageIndex.Text = currentIndex + "";
        }

        private void labJump_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ValidationJump();
        }

        private void txtJump_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidationJump();
            }
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
            goIndex = val;
            if (goIndex > totalPage)
            {
                txtJump.Text = totalPage + "";
                goIndex = totalPage;
            }
            else if (goIndex < 1)
            {
                txtJump.Text = "1";
                goIndex = 1;
            }

            if (goIndex != currentIndex)
                Page();
        }

        private void comPageSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int newSize = 0;
            if (!int.TryParse((e.AddedItems[0] as ComboBoxItem).Content + "",out newSize))
            {
                return;
            }
            pageSize = newSize;
            Init(totalRow);
            goIndex = 1;
            Page();
        }
    }
}
