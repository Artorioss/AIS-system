using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Customer: IEntity
    {
        public int CustomerId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public Customer()
        {
            Transportations = new ObservableCollection<Transportation>();
        }

        public Customer(Customer customer)
        {
            SetFields(customer);
        }

        public void SetFields(IEntity customer) 
        {
            Customer customerEnity = customer as Customer;
            CustomerId = customerEnity.CustomerId;
            Name = customerEnity.Name;
            Transportations = new ObservableCollection<Transportation>();
            if (customerEnity.Transportations != null && customerEnity.Transportations.Count > 0) foreach(var transportation in customerEnity.Transportations) Transportations.Add(transportation);
            SoftDeleted = customerEnity.SoftDeleted;
        }

        public object Clone()
        {
            return new Customer(this);
        }
    }
}
