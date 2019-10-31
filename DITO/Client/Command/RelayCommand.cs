using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Protos.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> job;

        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> job,Predicate<object> canExecute = null)
        {
            this.job = job ?? throw new ArgumentNullException(nameof(job));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter) => this.canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.job.Invoke(parameter);
    }
}
