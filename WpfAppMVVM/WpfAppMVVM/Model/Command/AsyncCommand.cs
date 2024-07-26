using System.Windows.Input;

namespace WpfAppMVVM.Model.Command
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            try
            {
                await _execute(parameter);
            }
            catch (Exception ex) 
            {
            
            }
            finally
            {
                _isExecuting = false;
            }
        }
    }
}
