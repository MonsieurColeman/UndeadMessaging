/*
 Template file used to make use of conditional commands
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ColemanPeerToPeer.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object paramenter)
        {
            return this.canExecute == null || this.canExecute(paramenter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}

/*
 Maintenance History

0.9: Ctrl+C 
1.0: Ctrl+V
 */

