using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ResourceSearcher.UILogic
{
    public class Command : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public Command(Action execute) : this(execute, () => true)
        {
        }

        public Command(Action execute, Func<bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }

    public class Command<T> : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public Command(Action<T> execute) : this(execute, o => true)
        {
        }

        public Command(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute is null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            if (canExecute is null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            this.execute = o => execute((T)o);
            this.canExecute = o => canExecute((T)o);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute(parameter);
            }
        }
    }
}
