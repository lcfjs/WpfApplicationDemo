using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace WpfApplicationDemo
{
    /// <summary>
    /// ICommand辅助类 
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region 实现ICommand接口

        /// <summary>
        /// 定义用于确定此命令是否可以在其当前状态下执行的方法
        /// </summary>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null</param>
        /// <returns>如果可以执行此命令，则为 true；否则为 false</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
       
        /// <summary>
        /// 定义在调用此命令时调用的方法
        /// </summary>
        /// <param name="parameter">此命令使用的数据。如果此命令不需要传递数据，则该对象可以设置为 null</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// 封装一个方法，该方法只有一个参数并且不返回值
        /// </summary>
        readonly Action<object> _execute;

        /// <summary>
        /// 定义一组条件并确定指定对象是否符合这些条件的方法
        /// </summary>
        readonly Predicate<object> _canExecute;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute): this(execute, null)
        { 
        
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                return;
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion
    }

}
