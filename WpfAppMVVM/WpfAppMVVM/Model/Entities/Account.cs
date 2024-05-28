using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.Entities
{
    internal class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public ICollection<IssuedInvoices> IssuedInvoices { get; set; }
    }
}
