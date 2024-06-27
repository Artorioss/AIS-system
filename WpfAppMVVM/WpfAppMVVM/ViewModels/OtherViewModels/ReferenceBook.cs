using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal abstract class ReferenceBook: BaseViewModel
    {
        protected TransportationEntities _context;
        private Mode _mode;
        Window _wind;

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

        private void setButtonText() 
        {
            if (_mode == Mode.Additing) ButtonText = "Создать";
            else ButtonText = "Сохранить изменения";
        }

        private void action()
        {
            if (dataIsCorrect())
            {
                try
                {
                    if (_mode == Mode.Additing) addEntity();
                    else updateEntity();
                    _context.SaveChanges();
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
    }
}
