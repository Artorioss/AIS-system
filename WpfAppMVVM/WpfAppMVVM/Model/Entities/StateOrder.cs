using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class StateOrder
    {
        public int StateOrderId { get; set; }
        public string Name { get; set; }
        public Transportation Transportation { get; set; }
    }
}
