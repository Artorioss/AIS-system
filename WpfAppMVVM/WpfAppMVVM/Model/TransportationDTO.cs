using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Models
{
    public class TransportationDTO
    {
        public int TransportationId { get; set; }
        public string? DateLoading { get; set; }
        public string CustomerName { get; set; }
        public string? DriverName { get; set; }
        public string TransportCompanyName { get; set; }
        public decimal? Price { get; set; }
        public decimal? PaymentToDriver { get; set; }
        public decimal? Delta { get; set; }
        public string? RouteName { get; set; }
        public int State { get; set; }
    }
}
