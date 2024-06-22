using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.Models.QueryObjects
{
    public static class TransportationDTOSelect
    {
        public static IQueryable<TransportationDTO> TransportationToDTO(this IQueryable<Transportation> transportations)
        {
            return transportations.Select(transportation => new TransportationDTO
            {
                TransportationId = transportation.TransportationId,
                DateLoading = transportation.DateLoading.Value.ToShortDateString(),
                CustomerName = transportation.Customer.Name,
                TransportCompanyName = transportation.Driver.TransportCompany.Name,
                DriverName = transportation.Driver.Name,
                Price = transportation.Price,
                PaymentToDriver = transportation.PaymentToDriver,
                Delta = transportation.Price - transportation.PaymentToDriver,
                RouteName = transportation.RouteName,
                State = transportation.StateOrder.StateOrderId
            });
        }
    }
}
