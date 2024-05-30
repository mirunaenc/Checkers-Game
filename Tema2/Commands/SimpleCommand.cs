using System;
using System.Windows.Input;

namespace Tema2.Commands
{
    class SimpleCommand : ICommand
    {

        private Action commandTask;
        private Predicate<object> canExecuteTask;

        public SimpleCommand(Action toDo) : this(toDo, DefaultCanExecute)
        {
            commandTask = toDo;
        }

        public SimpleCommand(Action toDo, Predicate<object> canExecute)
        {
            commandTask = toDo;
            canExecuteTask = canExecute;
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
        public bool CanExecute(object parameter)
        {
            return canExecuteTask != null && canExecuteTask(parameter);
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

        public void Execute(object parameter)
        {
            commandTask();
        }
    }
}
