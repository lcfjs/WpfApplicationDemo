using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplicationDemo
{
    /// <summary>
    /// API实现圆角窗体
    /// </summary>
    public class NativeMethods
    {
        /// <summary> 
        /// 带有外边框和标题的windows的样式 
        /// </summary> 
        public const long WS_BORDER = 0X0080000L;
        public const int WS_CAPTION = 0xC00000;
        public const long WS_CAPTION_2 = 0X00C0000L;

        /// <summary> 
        /// window 扩展样式 分层显示 
        /// </summary> 
        public const long WS_EX_LAYERED = 0x00080000L;

        /// <summary> 
        /// 带有alpha的样式 
        /// </summary> 
        public const uint LWA_ALPHA = 0x00000002;

        /// <summary> 
        /// 颜色设置 
        /// </summary> 
        public const uint LWA_COLORKEY = 0x00000001;

        /// <summary>
        /// window的基本样式 
        /// </summary> 
        public const int GWL_STYLE = -16;

        /// <summary> 
        /// window的扩展样式 
        /// </summary> 
        public const int GWL_EXSTYLE = -20;

        /// <summary> 
        /// 设置窗体的样式 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="oldStyle">进行设置窗体的样式类型.</param> 
        /// <param name="newStyle">新样式</param> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary> 
        /// 获取窗体指定的样式. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄</param> 
        /// <param name="style">要进行返回的样式</param> 
        /// <returns>当前window的样式</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr handle, int style);

        /// <summary> 
        /// 设置窗体的工作区域. 
        /// </summary> 
        /// <param name="handle">操作窗体的句柄.</param> 
        /// <param name="handleRegion">操作窗体区域的句柄.</param> 
        /// <param name="regraw">if set to <c>true</c> [regraw].</param> 
        /// <returns>返回值</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);

        /// <summary> 
        /// 创建带有圆角的区域. 
        /// </summary> 
        /// <param name="x1">左上角坐标的X值.</param> 
        /// <param name="y1">左上角坐标的Y值.</param> 
        /// <param name="x2">右下角坐标的X值.</param> 
        /// <param name="y2">右下角坐标的Y值.</param> 
        /// <param name="width">圆角椭圆的width.</param> 
        /// <param name="height">圆角椭圆的height.</param> 
        /// <returns>hRgn的句柄</returns> 
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int width, int height);

        /// <summary> 
        /// Sets the layered window attributes. 
        /// </summary> 
        /// <param name="handle">要进行操作的窗口句柄</param> 
        /// <param name="colorKey">RGB的值</param> 
        /// <param name="alpha">Alpha的值，透明度</param> 
        /// <param name="flags">附带参数</param> 
        /// <returns>true or false</returns> 
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr handle, uint colorKey, byte alpha, uint flags);

        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="CornerRadius"></param>
        public static void InitWindow(IntPtr hwnd, double Width, double Height, int CornerRadius = 5)
        {
            // 获得窗体的 样式 
            int oldstyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);

            // 更改窗体的样式为无边框窗体 
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, oldstyle & ~(NativeMethods.WS_CAPTION | (int)NativeMethods.WS_CAPTION_2));

//            // 设置窗体为透明窗体
//            NativeMethods.SetLayeredWindowAttributes(hwnd, 1 | 2 << 8 | 3 << 16, 0, NativeMethods.LWA_ALPHA);

            // 创建圆角窗体  9 这个值可以根据自身项目进行设置 
            NativeMethods.SetWindowRgn(hwnd, NativeMethods.CreateRoundRectRgn(0, 0, Convert.ToInt32(Width), Convert.ToInt32(Height), 5, CornerRadius), true);
        }

        /// <summary>
        /// 窗体Size变化
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="CornerRadius"></param>
        public static void WindowSizeChange(IntPtr hwnd, double Width, double Height, int CornerRadius = 5)
        {
            NativeMethods.SetWindowRgn(hwnd, NativeMethods.CreateRoundRectRgn(0, 0, Convert.ToInt32(Width), Convert.ToInt32(Height), 5, CornerRadius), true);
        }

    }
}
