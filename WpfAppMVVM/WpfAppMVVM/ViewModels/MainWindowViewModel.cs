using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.QueryObjects;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        public TransportationEntities _transportationEntities { get; set; }
        public ObservableCollection<TransportationDTO> ItemsSource { get; set; }
        public DelegateCommand CreateTransportation { get; private set; }
        public DelegateCommand ShowReferencesBook { get; private set; }

        private TransportationDTO _transportation;
        public TransportationDTO TransportationDTO
        {
            get => _transportation;
            set
            {
                _transportation = value;
                OnPropertyChanged(nameof(TransportationDTO));
            }
        }

        public MainWindowViewModel()
        {
            _transportationEntities = (Application.Current as App)._context;
            ItemsSource = new ObservableCollection<TransportationDTO>();

            var list = _transportationEntities.Transportations
                .Include(t => t.Driver)
                .Include(t => t.Customer)
                .Include(t => t.TransportCompany)
                .TransportationToDTO().ToList();
            ItemsSource = new ObservableCollection<TransportationDTO>(list);
            CreateTransportation = new DelegateCommand((obj) => showTransportationWindow());
        }

        private void showTransportationWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            creatingTransportationWindow.DataContext = new CreatingTransportationViewModel();
            creatingTransportationWindow.ShowDialog();
        }

        private void showWindowReferencesBook(object sender, RoutedEventArgs e)
        {
            WindowReferencesBook windowReferencesBook = new WindowReferencesBook();
            windowReferencesBook.ShowDialog();
        }
    }
}
