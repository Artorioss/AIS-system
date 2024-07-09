using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal abstract class ReferenceBook : BaseViewModel
    {
        protected TransportationEntities _context;
        private Mode _mode;
        private Window _wind;
        public bool changedExist { get; set; } = false;

        public DelegateCommand OnLoadedCommand { get; set; }
        public DelegateCommand AcceptСhangesCommand { get; set; }

        public Mode mode
        {
            get => _mode;
            set 
            {
                _mode = value;
                setButtonText();
            }
        }

        private string _buttonText = "Добавить запись";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public ReferenceBook()
        {
            _context = (Application.Current as App)._context;
            OnLoadedCommand = new DelegateCommand(onLoaded);
            AcceptСhangesCommand = new DelegateCommand((obj) => action());
            setCommands();
        }

        private void onLoaded(object obj) 
        {
            _wind = obj as Window;
        }

        public async Task ShowDialog() 
        {
            await (Application.Current as App).DisplayRootRegistry.ShowModalPresentation(this);
        }

        private void setButtonText() 
        {
            if (_mode == Mode.Additing) ButtonText = "Создать";
            else ButtonText = "Сохранить изменения";
        }

        private async Task action()
        {
            if (dataIsCorrect())
            {
                try
                {
                    if (_mode == Mode.Additing) addEntity();
                    else updateEntity();
                    await _context.SaveChangesAsync();
                    changedExist = true;
                    _wind?.Close();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Не удалось сохранить изменения - {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        protected abstract void setCommands();
        protected abstract bool dataIsCorrect();
        protected abstract void updateEntity();
        protected abstract void addEntity();
        public abstract ICloneable GetEntity();
    }
}
