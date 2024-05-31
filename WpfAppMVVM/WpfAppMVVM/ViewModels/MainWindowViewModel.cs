using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models;
using WpfAppMVVM.Models.Entities;
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
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand GetItemsByFilter { get; set; }
        public ObservableCollection<TransportationDTO> ItemsSource { get; set; }
        private List<StateOrder> _stateOrders;
        public List<StateOrder> StateOrders 
        {
            get => _stateOrders;
            set 
            {
                _stateOrders = value;
                OnPropertyChanged(nameof(StateOrders));
            }
        }

        private StateOrder _selectedState;
        public StateOrder SelectedState
        {
            get => _selectedState;
            set 
            {
                _selectedState = value;
                OnPropertyChanged(nameof(SelectedState));
                getItems();
            }
        }

        private List<int> _years;
        public List<int> Years 
        {
            get => _years;
            set 
            {
                _years = value;
                OnPropertyChanged(nameof(Years));
            }
        }

        private int _selectedYear = DateTime.Now.Year;
        public int SelectedYear 
        {
            get => _selectedYear;
            set 
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                getItems();
            }
        }

        
        private int _selectedMonth = DateTime.Now.Month - 1;
        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
                getItems();
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

        private void setDate(DateTime dateTime) 
        {
            _selectedMonth = dateTime.Month - 1;
            OnPropertyChanged(nameof(SelectedMonth));
            _selectedYear = dateTime.Year;
            OnPropertyChanged(nameof(SelectedYear));
        }

        public MainWindowViewModel()
        {
            _transportationEntities = (Application.Current as App)._context;
            ItemsSource = new ObservableCollection<TransportationDTO>();
            StateOrders = _context.StateOrders.ToList();
            SelectedState = StateOrders.First();
            CreateTransportation = new DelegateCommand((obj) => showTransportationWindow());
            ShowReferencesBook = new DelegateCommand((obj) => showWindowReferencesBook());
            EditData = new DelegateCommand((obj) => showTransportationForEditWindow());
            DeleteCommand = new DelegateCommand((obj) => onDelete());

            Years = _context.Transportations
                            .Distinct()
                            .Select(t => t.DateLoading.Value.Date.Year)
                            .Distinct()
                            .ToList();
        }

        private void getItems() 
        {
            var startDate = new DateTime(SelectedYear, SelectedMonth + 1, 1);
            var endDate = startDate.AddMonths(1);

            var list = _transportationEntities.Transportations
                .Include(t => t.Driver)
                .Include(t => t.Customer)
                .Include(t => t.TransportCompany)
                .Include(t => t.StateOrder)
                .Where(t => t.DateLoading.HasValue &&
                            t.DateLoading.Value >= startDate &&
                            t.DateLoading.Value < endDate &&
                            t.StateOrderId == SelectedState.StateOrderId)
                .TransportationToDTO();

            loadData(list);
        }

        private void loadData(IQueryable<TransportationDTO> list) 
        {
            ItemsSource.Clear();
            foreach (var item in list) ItemsSource.Add(item);
        }

        private void showTransportationWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel();
            creatingTransportationWindow.DataContext = creatingTransportationViewModel;
            creatingTransportationWindow.ShowDialog();

            if (creatingTransportationViewModel.IsContextChanged)
            {
                if (creatingTransportationViewModel.Transportation.DateLoading.Value.Month != SelectedMonth + 1 || creatingTransportationViewModel.Transportation.DateLoading.Value.Year != SelectedYear) 
                {
                    DateTime date = creatingTransportationViewModel.Transportation.DateLoading.Value;
                    setDate(date);
                    getItems();
                }
                else ItemsSource.Add(_context.Transportations.TransportationToDTO().Single(tr => tr.TransportationId == creatingTransportationViewModel.Transportation.TransportationId));
            }
        }

        private void showTransportationForEditWindow()
        {
            CreatingTransportationWindow creatingTransportationWindow = new CreatingTransportationWindow();
            CreatingTransportationViewModel creatingTransportationViewModel = new CreatingTransportationViewModel(TransportationDTO.TransportationId);
            creatingTransportationWindow.DataContext = creatingTransportationViewModel;
            creatingTransportationWindow.ShowDialog();

            if (creatingTransportationViewModel.IsContextChanged) 
            {
                var entity = _context.Transportations.TransportationToDTO().Single(tr => tr.TransportationId == creatingTransportationViewModel.Transportation.TransportationId);
                if (creatingTransportationViewModel.Transportation.DateLoading.Value.Month != SelectedMonth + 1 || creatingTransportationViewModel.Transportation.DateLoading.Value.Year != SelectedYear)
                {
                    DateTime date = creatingTransportationViewModel.Transportation.DateLoading.Value;
                    setDate(date);
                    getItems();                   
                }
                else ItemsSource.Insert(ItemsSource.IndexOf(TransportationDTO), entity);
                ItemsSource.Remove(TransportationDTO);
                TransportationDTO = entity;
            }       
        }

        private void showWindowReferencesBook()
        {
            WindowReferencesBook windowReferencesBook = new WindowReferencesBook();
            windowReferencesBook.ShowDialog();
        }

        private void onDelete() 
        {
            MessageBoxResult result = MessageBox.Show($"Заявка - '{TransportationDTO.RouteName}' будет удалена.", "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) delete();          
        }

        private void delete() 
        {
            _context.Transportations.Remove(_context.Transportations.Single(t => t.TransportationId == TransportationDTO.TransportationId));
            _context.SaveChanges();
            ItemsSource.Remove(TransportationDTO);
            OnPropertyChanged(nameof(ItemsSource));
        }
    }
}
