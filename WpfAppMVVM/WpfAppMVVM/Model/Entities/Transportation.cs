using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Models.Entities
{
    public class Transportation
    {
        public int TransportationId { get; set; }
        public string? DateLoading { get; set; }
        public int CustomerId { get; set; }
        public int? DriverId { get; set; }
        public int? TransportCompanyId { get; set; }
        public decimal? Price { get; set; }
        public decimal? PaymentToDriver { get; set; }
        public int? AccountNumber { get; set; }
        public string? AccountDate { get; set; }
        public string? Address { get; set; }
        public int StateOrderId { get; set; }
        public Customer Customer { get; set; }
        public Driver? Driver { get; set; }
        public TransportCompany TransportCompany { get; set; }
        public StateOrder StateOrder { get; set; }

    }
}
