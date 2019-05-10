using System;
using System.Windows.Input;

namespace Biblioteca_del_Papa
{
    public partial class Command : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public Command(Action execute) : this(execute, () => true)
        {
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute();
            }
        }
    }

    public partial class Command<T> : ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        public Command(Action<T> execute) : this(execute, o => true)
        {
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute((T)parameter);
            }
        }
    }
}
