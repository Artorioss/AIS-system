using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Command;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.ViewModels.CreatingTransportation
{
    internal partial class CreatingTransportationViewModel : BaseViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }
        public DelegateCommand SetCustomer { get; set; }
        private bool freezComboBox = false;

        Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                if (freezComboBox)
                {
                    var cust = Customers.SingleOrDefault(x => x.CustomerId == value.CustomerId);
                    if (cust != null)
                    {
                        Customers.Remove(cust);
                        Customers.Add(value);
                    }
                    else
                    {
                        Customers.Clear();
                        Customers.Add(value);
                    }
                }
                OnPropertyChanged(nameof(Customer));
            }
        }

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                OnPropertyChanged(nameof(CustomerName));
                if (!freezComboBox) getCustomers();
            }
        }
        private bool _isDropDownOpenCustomers;
        public bool IsDropDownOpenCustomers
        {
            get => _isDropDownOpenCustomers;
            set
            {
                _isDropDownOpenCustomers = value;
                OnPropertyChanged(nameof(IsDropDownOpenCustomers));
            }
        }
        private void getCustomers()
        {
            if (string.IsNullOrEmpty(CustomerName))
            {
                IsDropDownOpenCustomers = false;
                Customer = null;
            }

            if (Customer == null)
            {
                var list = _context.Customers.AsNoTracking()
                                .Where(c => c.Name.ToLower().Contains(CustomerName.ToLower()))
                                .OrderBy(c => c.Name)
                                .Take(5);
                Customers.Clear();
                foreach (var item in list)
                {
                    Customers.Add(item);
                }
            }

            IsDropDownOpenCustomers = Customers.Count > 0;

        }

        private void setCustomer()
        {
            if (_customer == null && !string.IsNullOrEmpty(CustomerName))
            {
                freezComboBox = true;
                Customer cust = _context.Customers
                        .FirstOrDefault(s => s.Name.ToLower() == CustomerName.ToLower());

                if (cust is null)
                    Customer = new Customer { Name = CustomerName };
                else
                {
                    Customer = cust;
                }
                freezComboBox = false;
            }
        }
    }
}
