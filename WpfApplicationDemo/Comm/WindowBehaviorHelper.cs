using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfApplicationDemo
{
    /// <summary>
    /// 窗体行为帮助类,最大化窗体时不遮住任务栏(在窗体构造函数中初始化)
    /// </summary>
    public class WindowBehaviorHelper
    {
        #region 定义变量

        //测试消息
        private const int WM_NCHITTEST = 0x0084;
        //大小变化
        private const int WM_GETMINMAXINFO = 0x0024;
        private const int SC_MAXIMIZE = 0xF030;
        //目标窗口
        private Window WindowTarget;
        //拐角宽度 3       
        private int WidthCorner = 2;  //3   
        //阴影宽度 5   
        private int ThicknessTransparentBorder = 1;
        //边框宽度 4
        private int ThicknessBorder = 1;
        //鼠标坐标
        private Point PointMouse = new Point();
        //窗口最大宽度
        private int m_MaxWidth = 0;
        //窗口最大高度
        private int m_MaxHeight = 0;
        //窗口最小宽度
        private int m_MinWidth = 0;
        //窗口最小高度
        private int m_MinHeight = 0;
        //是否控制窗口大小
        private bool m_LimitSize = false;
        //句柄
        public enum HitTest : int
        {
            #region 句柄

            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTSIZE = HTGROWBOX,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTREDUCE = HTMINBUTTON,
            HTZOOM = HTMAXBUTTON,
            HTSIZEFIRST = HTLEFT,
            HTSIZELAST = HTBOTTOMRIGHT,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21

            #endregion
        }

        #endregion

        /// <summary>
        /// 设置窗口大小范围控制(该方法可不调用)
        /// </summary>
        /// <param name="MaxWidth">最大宽度 小于等于0不进行设置</param>
        /// <param name="MinWidth">最小宽度 小于等于0不进行设置</param>
        /// <param name="MaxHeight">最大高度 小于等于0不进行设置</param>
        /// <param name="MinHeight">最小高度 小于等于0不进行设置</param>
        public void LimitFormSize(int MaxWidth, int MinWidth, int MaxHeight, int MinHeight)
        {
            m_LimitSize = true;
            m_MaxHeight = MaxHeight;
            m_MaxWidth = MaxWidth;
            m_MinHeight = MinHeight;
            m_MinWidth = MinWidth;

        }

        /// <summary>
        /// 处理最大化时窗体不覆盖状态栏 构造函数
        /// </summary>
        /// <param name="window">要处理的窗体对象</param>
        public WindowBehaviorHelper(Window window)
        {
            this.WindowTarget = window;
        }

        /// <summary>
        /// 添加钩子
        /// </summary>
        public void RepairBehavior()
        {
            if (WindowTarget == null)
                return;
            this.WindowTarget.SourceInitialized += delegate
            {
                IntPtr handle = (new WindowInteropHelper(WindowTarget)).Handle;
                HwndSource hwndSource = HwndSource.FromHwnd(handle);
                if (hwndSource != null)
                {
                    hwndSource.AddHook(WindowProc);
                }
            };
        }

        /// <summary>
        /// 取消钩子
        /// </summary>
        public void ReSetpairBehavior()
        {
            IntPtr handle = (new WindowInteropHelper(WindowTarget)).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(handle);
            if (hwndSource != null)
            {
                hwndSource.RemoveHook(WindowProc);
            }
        }
       
        /// <summary>
        /// 消息循环
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_NCHITTEST:
                    if (WindowTarget.WindowState != WindowState.Normal)
                    {
                        break;
                    } 
                    this.PointMouse.X = (lParam.ToInt32() & 0xFFFF);
                    this.PointMouse.Y = (lParam.ToInt32() >> 16);
                    //窗口左上角
                    if (this.PointMouse.X > this.WindowTarget.Left + this.ThicknessTransparentBorder
                        && this.PointMouse.X <= this.WindowTarget.Left + this.ThicknessTransparentBorder + this.WidthCorner
                        && this.PointMouse.Y > this.WindowTarget.Top + this.ThicknessTransparentBorder
                        && this.PointMouse.Y <= this.WindowTarget.Top + this.ThicknessTransparentBorder + this.WidthCorner)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOPLEFT);
                    }
                    //窗口左下角
                    else if (this.PointMouse.X > this.WindowTarget.Left + this.ThicknessTransparentBorder
                        && this.PointMouse.X <= this.WindowTarget.Left + this.ThicknessTransparentBorder + this.WidthCorner
                        && this.PointMouse.Y < this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder
                        && this.PointMouse.Y >= this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder - this.WidthCorner)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOMLEFT);
                    }
                    //窗口右上角
                    else if (this.PointMouse.X < this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder
                        && this.PointMouse.X >= this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder - this.WidthCorner
                        && this.PointMouse.Y > this.WindowTarget.Top + this.ThicknessTransparentBorder
                        && this.PointMouse.Y <= this.WindowTarget.Top + this.ThicknessTransparentBorder + this.WidthCorner)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOPRIGHT);
                    }
                    //窗口右下角
                    else if (this.PointMouse.X < this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder
                        && this.PointMouse.X >= this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder - this.WidthCorner
                        && this.PointMouse.Y < this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder
                        && this.PointMouse.Y >= this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder - this.WidthCorner)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOMRIGHT);
                    }
                    //窗口左侧
                    else if (this.PointMouse.X > this.WindowTarget.Left + this.ThicknessTransparentBorder
                        && this.PointMouse.X <= this.WindowTarget.Left + this.ThicknessTransparentBorder + this.ThicknessBorder
                        && this.PointMouse.Y > this.WindowTarget.Top + this.ThicknessTransparentBorder
                        && this.PointMouse.Y < this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTLEFT);
                    }
                    //窗口右侧
                    else if (this.PointMouse.X < this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder
                        && this.PointMouse.X >= this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder - this.ThicknessBorder
                        && this.PointMouse.Y > this.WindowTarget.Top + this.ThicknessTransparentBorder
                        && this.PointMouse.Y < this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTRIGHT);
                    }
                    //窗口上方
                    else if (this.PointMouse.X > this.WindowTarget.Left + this.ThicknessTransparentBorder
                        && this.PointMouse.X < this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder
                        && this.PointMouse.Y > this.WindowTarget.Top + this.ThicknessTransparentBorder
                        && this.PointMouse.Y <= this.WindowTarget.Top + this.ThicknessTransparentBorder + this.ThicknessBorder)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTTOP);
                    }
                    //窗口下方
                    else if (this.PointMouse.X > this.WindowTarget.Left + this.ThicknessTransparentBorder
                        && this.PointMouse.X < this.WindowTarget.Left + this.WindowTarget.ActualWidth - this.ThicknessTransparentBorder
                        && this.PointMouse.Y < this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder
                        && this.PointMouse.Y >= this.WindowTarget.Top + this.WindowTarget.ActualHeight - this.ThicknessTransparentBorder - this.ThicknessBorder)
                    {
                        handled = true;
                        return new IntPtr((int)HitTest.HTBOTTOM);
                    }
                    //其他消息
                    else
                    {
                        break;
                    }
                case WM_GETMINMAXINFO:
                        WmGetMinMaxInfo(hwnd, lParam);
                        handled = true;
                    break;

                default:
                    break;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 更改最小化最大化时窗口位置大小
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            //限制窗口可改变大小的最大范围和最小范围
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);  
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left) - 3;
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top) - 3;
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left) + 6;
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top) + 6;
                mmi.ptMinTrackSize.x = (int)this.WindowTarget.MinWidth;
                mmi.ptMinTrackSize.y = (int)this.WindowTarget.MinHeight;
            }
            if (m_LimitSize)
            {
                if (m_MaxWidth > 0)
                    mmi.ptMaxTrackSize.x = m_MaxWidth;
                if (m_MaxHeight > 0)
                    mmi.ptMaxTrackSize.y = m_MaxHeight;
                if (m_MinWidth > 0)
                    mmi.ptMinTrackSize.x = m_MinWidth;
                if (m_MinHeight > 0)
                    mmi.ptMinTrackSize.y = m_MinHeight;

            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        #region 定义结构体:MINMAXINFO
        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
        #endregion

        #region 定义结构体: MONITORINFO
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }
        #endregion

        #region 定义结构体: POINT
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        #endregion

        #region 定义结构体: RECT
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty;
            public int Width
            {
                get { return Math.Abs(right - left); }
            }
            public int Height
            {
                get { return bottom - top; }
            }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty
            {
                get
                {
                    return left >= right || top >= bottom;
                }
            }
            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }
                return (this == (RECT)obj);
            }
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }
        #endregion

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
    }
}
