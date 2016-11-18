using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplicationDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static bool runFlag = false;
        /// <summary>
        /// 互斥体
        /// </summary>
        static Mutex mutex = new Mutex(true, "v1.0", out runFlag);

        protected override void OnStartup(StartupEventArgs e)
        {
            if (runFlag)
            {
            }
            else
            {
                Environment.Exit(1);
            }

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            base.OnStartup(e);

            mutex.ReleaseMutex();
        }



        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is AggregateException)
            {
                var curE = e.ExceptionObject as AggregateException;
                CommonHelper.WriteLog("非UI线程异常", curE.InnerException);
                curE.InnerExceptions.ToList().ForEach((ex) =>
                {
                    CommonHelper.WriteLog("非UI线程异常", ex);
                });
            }
            
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            CommonHelper.WriteLog("Task线程异常", e.Exception);
            e.SetObserved();
        }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            CommonHelper.WriteLog("UI线程异常", e.Exception);

            e.Handled = true;
        }
    }
}
