﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Customer: ICloneable
    {
        public int CustomerId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public Customer()
        {
            Transportations = new List<Transportation>();
        }

        public Customer(Customer customer)
        {
            SetFields(customer);
        }

        public void SetFields(Customer customer) 
        {
            CustomerId = customer.CustomerId;
            Name = customer.Name;
            Transportations = customer.Transportations;
        }

        public object Clone()
        {
            return new Customer(this);
        }
    }
}
