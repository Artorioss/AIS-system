using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Models.Entities;

namespace WpfAppMVVM.Models.QueryObjects
{
    public static class TransportationDTOSelect
    {
        public static IQueryable<TransportationDTO> TransportationToDTO(this IQueryable<Transportation> transportations)
        {
            return transportations.Select(transportation => new TransportationDTO
            {
                TransportationId = transportation.TransportationId,
                DateLoading = transportation.DateLoading,
                CustomerName = transportation.Customer.Name,
                DriverName = transportation.Driver.Name,
                TransportCompanyName = transportation.TransportCompany.Name,
                Price = transportation.Price,
                PaymentToDriver = transportation.PaymentToDriver,
                Delta = transportation.Price - transportation.PaymentToDriver,
                Address = transportation.Route.RouteName
            });
        }
    }
}
