using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace Common
{
    public class CommonHelper
    {
        private static string logDirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");

        public static void WriteLog(string msg, Exception e = null)
        {
            if (!System.IO.Directory.Exists(logDirPath))
            {
                Directory.CreateDirectory(logDirPath);
            }

            var logPath = Path.Combine(logDirPath, $"log_{DateTime.Now.ToString("yyyyMMdd")}.txt");
            File.AppendAllText(logPath, $"【时间】：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\r\n【消息】：{msg}\r\n【异常】：{e}\r\n\r\n");
        }

        /// <summary>
        ///  将文件读取到Byte数组
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadFileToBytes(string fileName)
        {
            if (File.Exists(fileName))
            {
                byte[] buffer = null;
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
                return buffer;
            }
            return null;
        }

        /// <summary>
        /// 转二进制流
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmpToBytes(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 删除指定目录下的所有文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DelteFile(string dir)
        {
            if (Directory.Exists(dir))
            {
                string[] files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                foreach (var item in files)
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 播放Wav音频文件
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlayerWav(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    SoundPlayer player = new SoundPlayer(fileName);
                    player.Play();
                }
                catch (Exception ex)
                {
                    WriteLog("播放声音文件(wav)异常", ex);
                }
            }

        }



        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        /// <summary>
        /// Bitmap转成BitmapSource
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bmp)
        {
            IntPtr ptr = bmp.GetHbitmap();

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            bmp.Dispose();

            DeleteObject(ptr); // 释放内存，否则内存泄露
            return bs;
        }

        #region 配置文件
        /// <summary>
        ///  获取配置文件中指定Key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 更新置顶Key的值(若未找到该节点则创建)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static void UpdateConfigValue(string key, string newValue)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, newValue);
                }
                else
                {
                    config.AppSettings.Settings[key].Value = newValue;
                }
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
        #endregion

        #region WPF控件

        /// <summary>
        /// 在指定元素的后代元素中查找指定Name的元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        //public static object FindChildControl(DependencyObject element, string name)
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        //    {
        //        var control = VisualTreeHelper.GetChild(element, i) as FrameworkElement;
        //        if (control != null && control.Name == name)
        //            return control;
        //        else
        //        {
        //            var item = FindChildControl(control, name);
        //            if (item != null)
        //                return item;
        //        }
        //    }
        //    return null;
        //}


        /// <summary>
        /// 返回在指定元素的后代元素中查找到指定类型的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        //public static T FindChildControl<T>(DependencyObject element) where T : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
        //    {
        //        DependencyObject obj = VisualTreeHelper.GetChild(element, i);
        //        if (obj != null && obj is T)
        //        {
        //            return (T)obj;
        //        }
        //        else
        //        {
        //            T child = FindChildControl<T>(obj);
        //            if (child != null)
        //                return child;
        //        }
        //    }
        //    return default(T);
        //}
        #endregion

    }


    //public delegate bool EqualsComparer<T>(T x, T y);
    /// <summary>
    /// 自定义比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomCompare<T> : IEqualityComparer<T>
    {
        private Func<T, T, bool> _equalsComparer;

        public CustomCompare(Func<T, T, bool> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }

        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
                return this._equalsComparer(x, y);
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
        //demo
        //使用匿名方法
        //List<Person> delegateList = personList.Distinct(new CustomCompare<Person>(
        //    delegate(Person x, Person y)
        //    {
        //        if (null == x || null == y) return false;
        //        return x.ID == y.ID;
        //    })).ToList();
    }
}
