using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfAppMVVM.ViewModels.OtherViewModels
{
    internal class FilterStateViewModel : BaseViewModel
    {
        StateFilter _stateFilter;
        public DelegateCommand GetStatesCommand { get; set; }
        public AsyncCommand AddStateAsyncCommand { get; set; }
        public DelegateCommand AddStateByKeyboardCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public ObservableCollection<StateOrder> StateOrders 
        {
            get => _stateFilter.StateOrders;
        }
        public FilterStateViewModel()
        {
            mode = Mode.Additing;
            _stateFilter = new StateFilter();
        }

        public FilterStateViewModel(StateFilter stateFilter)
        {
            mode = Mode.Editing;
            _stateFilter = stateFilter;
            WindowName = "Редактирование фильтра";
        }

        protected override void cloneEntity()
        {
            _stateFilter = _stateFilter.Clone() as StateFilter;
        }

        protected override async Task loadReferenceData()
        {
            if (!_context.Entry(_stateFilter).Collection(c => c.StateOrders).IsLoaded)
            {
                _stateFilter.StateOrders.Clear();
                await _context.Entry(_stateFilter).Collection(c => c.StateOrders).LoadAsync();
            }
        }

        private string _windowName = "Создание фильтра";
        public string WindowName
        {
            get => _windowName;
            set
            {
                _windowName = value;
                OnPropertyChanged(nameof(WindowName));
            }
        }

        public string FilterName
        {
            get => _stateFilter.Name;
            set
            {
                _stateFilter.Name = value;
                OnPropertyChanged(nameof(FilterName)); ;
            }
        }

        private List<StateOrder> _states;
        public List<StateOrder> StateSource
        {
            get => _states;
            set
            {
                _states = value;
                OnPropertyChanged(nameof(StateSource));
            }
        }

        private string _stateName;
        public string StateName
        {
            get => _stateName;
            set
            {
                _stateName = value;
                OnPropertyChanged(nameof(StateName));
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
            }
        }

        private void getStates(object obj)
        {
            string text = obj as string;
            StateSource = _context.StateOrders
                                   .Where(s => s.Name.ToLower().Contains(text.ToLower()))
                                   .Take(5)
                                   .ToList();
        }

        private async Task addState()
        {
            if (string.IsNullOrEmpty(StateName))
            {
                MessageBox.Show("Укажите название состояния.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedState is null) await createState();
            if (StateOrders.Contains(SelectedState)) MessageBox.Show($"Состояние {SelectedState.Name} уже числится за фильтром {FilterName}.", "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                StateOrders.Add(SelectedState);
                _stateFilter.StateOrders.Add(SelectedState);
            }
            SelectedState = null;
        }

        private async void addStateByKeyboard(object obj)
        {
            if ((Key)obj == Key.Enter) await addState();
        }

        private async Task createState()
        {
            SelectedState = StateSource.SingleOrDefault(d => d.Name.ToLower() == StateName.ToLower());
            if (SelectedState is null) SelectedState = await _context.StateOrders.SingleOrDefaultAsync(d => d.Name.ToLower() == StateName.ToLower());
            if (SelectedState is null) SelectedState = new StateOrder { Name = StateName };
        }

        private void deleteEntity(object obj)
        {
            StateOrders.Remove(obj as StateOrder);
        }

        protected override async Task<bool> dataIsCorrect()
        {
            if (mode == Mode.Additing)
            {
                if (string.IsNullOrEmpty(FilterName))
                {
                    MessageBox.Show("Укажите корректное наименование фильтра.", "Неправильно заполнены данные", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }

        protected override void setCommands()
        {
            GetStatesCommand = new DelegateCommand(getStates);
            AddStateAsyncCommand = new AsyncCommand(async (obj) => await addState());
            AddStateByKeyboardCommand = new DelegateCommand(addStateByKeyboard);
            DeleteCommand = new DelegateCommand(deleteEntity);
        }

        protected override async Task addEntity()
        {
            var filter = await _context.StateFilter.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Name == _stateFilter.Name && t.SoftDeleted);
            if (filter != null) 
            {
                _stateFilter.StateFilterId = filter.StateFilterId;
                filter.SetFields(_stateFilter);
            } 
            else await _context.AddAsync(_stateFilter);
        }

        protected override async Task updateEntity()
        {
            var filter = await _context.StateFilter.FindAsync(_stateFilter.StateFilterId);
            filter.SetFields(_stateFilter);
        }

        public override async Task<IEntity> GetEntity() => await _context.StateFilter.FindAsync(_stateFilter.StateFilterId);
    }
}

