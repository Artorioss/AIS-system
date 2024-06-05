using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class StateOrder
    {
        public int StateOrderId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Transportation> Transportation { get; set; }
    }
}
