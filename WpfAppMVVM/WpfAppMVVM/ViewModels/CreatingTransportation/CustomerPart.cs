using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.OtherViewModels;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        private List<Customer> _customerSource;
        public List<Customer> CustomerSource 
        {
            get => _customerSource;
            set 
            {
                _customerSource = value;
                OnPropertyChanged(nameof(CustomerSource));
            }
        }

        public DelegateCommand GetCustomers { get; set; }

        public Customer Customer
        {
            get => Transportation.Customer;
            set
            {
                Transportation.Customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        private void getCustomers(object e)
        {
            string text = e as string;
            CustomerSource = _context.Customers
                            .Where(c => c.Name.ToLower().Contains(text.ToLower()))
                            .OrderBy(c => c.Name)
                            .Take(5)
                            .ToList();
        }
    }
}
