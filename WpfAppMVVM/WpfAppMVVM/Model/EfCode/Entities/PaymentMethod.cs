using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        public ICollection<Transportation> Transportations { get; set; }
    }
}
