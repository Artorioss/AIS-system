using System.ComponentModel.DataAnnotations;

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
