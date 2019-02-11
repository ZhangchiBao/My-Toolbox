using System;
using System.Windows.Input;

namespace Book
{
    public partial class Command<T> : ICommand
    {
        private readonly Action<T> _Execute;
        private readonly Func<T, bool> _CanExecute;

        public Command(Action<T> execute) : this(execute, o => true)
        {
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _Execute((T)parameter);
            }
        }
    }

    public partial class Command : ICommand
    {
        private readonly Action _Execute;
        private readonly Func<bool> _CanExecute;

        public Command(Action execute) : this(execute, () => true)
        {
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _CanExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                _Execute();
            }
        }
    }
}
