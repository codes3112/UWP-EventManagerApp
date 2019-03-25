using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPQuickStart.Utils
{
    public class GenericCommand
    {
        public event EventHandler CanExecuteChanged;
        public event Action<string> AddMyEvent;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            var command = parameter as string;
            AddMyEvent?.Invoke(command);
        }
    }
}
