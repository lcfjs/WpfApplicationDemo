using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// WindowAsync.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAsync : Window
    {
        public WindowAsync()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //var val =await GetValAsync();
            //txtName.Text = val;

            Action del = () =>
            {

                Thread.Sleep(3000);
                txtName.Dispatcher.Invoke(() =>
                {
                    txtName.Text = "123";
                });
            };
            del.BeginInvoke((ar) =>
            {
                Console.WriteLine(ar.IsCompleted);
            }, null);

        }



        private async Task<string> GetValAsync()
        {
            //System.Threading.Thread.Sleep(2000);
            var r = Task<string>.Factory.StartNew(() =>
            {

                System.Threading.Thread.Sleep(2000);

                return "hello time!" + DateTime.Now;
            });
            return await r;
        }

        AutoResetEvent are1 = new AutoResetEvent(true);
        AutoResetEvent are2 = new AutoResetEvent(false);
        AutoResetEvent are3 = new AutoResetEvent(false);
        CancellationTokenSource cts = null;
        int count = 1;
        object objLock = new object();
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            Task t = new Task(() =>
            {

                //while (!cts.Token.IsCancellationRequested)
                //{
                //    System.Threading.Thread.Sleep(200);
                //}
                while (true)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    System.Threading.Thread.Sleep(200);
                }

                txtName.Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtName.Text = "Task.Factory.StartNew()";
                }));

            }, cts.Token);
            t.ContinueWith((k) =>
            {
                var t1 = k.IsCompleted;
                var t2 = k.IsCanceled;
                var t3 = k.IsFaulted;
                var exp = k.Exception;
            });


            //---------------------------
            Task.Factory.StartNew(() =>
            {
                while (count < 100)
                {
                    are1.WaitOne();
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine("t1-" + count);
                        count++;
                    }
                    Thread.Sleep(1000);
                    are2.Set();
                    are1.Reset();
                }

            });


            Task.Factory.StartNew(() =>
            {
                while (count < 100)
                {
                    are2.WaitOne();
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine("t22----" + count);
                        count++;
                    }
                    Thread.Sleep(1000);
                    are3.Set();
                }

            });


            Task.Factory.StartNew(() =>
            {
                while (count < 100)
                {
                    are3.WaitOne();
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine("t333----" + count);
                        count++;
                    }
                    Thread.Sleep(1000);
                    are1.Set();
                }

            });



        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            are1.Set();

        }
    }
}
