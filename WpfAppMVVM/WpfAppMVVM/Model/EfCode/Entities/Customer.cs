using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Transportation> Transportations { get; set; }
    }
}
