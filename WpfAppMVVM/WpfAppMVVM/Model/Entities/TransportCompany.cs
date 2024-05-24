using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class TransportCompany
    {
        public int TransportCompanyId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }
}
