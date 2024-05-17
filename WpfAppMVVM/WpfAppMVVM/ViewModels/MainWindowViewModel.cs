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
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.Views;

namespace WpfAppMVVM.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        public TransportationEntities _transportationEntities { get; set; }
        public DelegateCommand CreateTransportation { get; private set; }
        public DelegateCommand ShowReferencesBook { get; private set; }
        public DelegateCommand EditData { get; private set; }

        public List<TransportationDTO> _itemsSource;
        public List<TransportationDTO> ItemsSource 
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value;
                OnPropertyChanged(nameof(ItemsSource));
            }
        }

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
            loadTransportations();
            CreateTransportation = new DelegateCommand((obj) => showTransportationWindow());
            ShowReferencesBook = new DelegateCommand((obj) => showWindowReferencesBook());
            EditData = new DelegateCommand((obj) => showTransportationForEditWindow());
        }

        private void loadTransportations() 
        {
            ItemsSource = _transportationEntities.Transportations
                .Include(t => t.Driver)
                .Include(t => t.Customer)
                .Include(t => t.TransportCompany)
                .TransportationToDTO().ToList();
        }

        private void showTransportationWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            creatingTransportationWindow.DataContext = new CreatingTransportationViewModel();
            creatingTransportationWindow.ShowDialog();
            loadTransportations();

        }

        private void showTransportationForEditWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            creatingTransportationWindow.DataContext = new CreatingTransportationViewModel(TransportationDTO.TransportationId);
            creatingTransportationWindow.ShowDialog();
            loadTransportations();
        }

        private void showWindowReferencesBook()
        {
            WindowReferencesBook windowReferencesBook = new WindowReferencesBook();
            windowReferencesBook.ShowDialog();
        }
    }
}
