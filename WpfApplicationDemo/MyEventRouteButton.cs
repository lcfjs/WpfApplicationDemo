using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplicationDemo
{
    public class MyEventRouteButton : Button
    {
        protected override void OnClick()
        {
            base.OnClick();

            MyEventArgs eh = new MyEventArgs(NewClickEvent,this);
            eh.Tag = this.Name;
            this.RaiseEvent(eh);
        }

        public event RoutedEventHandler NewClick
        {
            add { this.AddHandler(NewClickEvent, value); }
            remove { this.RemoveHandler(NewClickEvent, value); }
        }


        
        public static readonly RoutedEvent NewClickEvent =
            EventManager.RegisterRoutedEvent("NewClick", RoutingStrategy.Bubble, typeof(EventHandler<MyEventArgs>), typeof(MyEventRouteButton));


        
    }

    public class MyEventArgs : RoutedEventArgs
    {
        public MyEventArgs(RoutedEvent routedEvent, object source) :base(routedEvent ,source)
        {

        }

        public string Tag { get; set; }
    }
}
