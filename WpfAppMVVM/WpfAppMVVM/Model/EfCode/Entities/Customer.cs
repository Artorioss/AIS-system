using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Customer: IEntity
    {
        public int CustomerId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public Customer()
        {
            Transportations = new List<Transportation>();
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
            Transportations = new List<Transportation>();
            if (customerEnity.Transportations != null && customerEnity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(customerEnity.Transportations);
            SoftDeleted = customerEnity.SoftDeleted;
        }

        public object Clone()
        {
            return new Customer(this);
        }
    }
}
