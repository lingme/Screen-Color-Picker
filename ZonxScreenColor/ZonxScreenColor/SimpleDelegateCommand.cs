using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZonxScreenColor
{
    public class SimpleDelegateCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 委托命令，不包含canExecute
        /// </summary>
        /// <param name="execute"></param>
        public SimpleDelegateCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// 委托命令
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public SimpleDelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged == null)
                return;

            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
