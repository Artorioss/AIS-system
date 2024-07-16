using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class PaymentMethod: IEntity
    {
        public int PaymentMethodId { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public PaymentMethod() 
        {
            Transportations = new HashSet<Transportation>(); 
        }

        public PaymentMethod(PaymentMethod paymentMethod)
        {

            SetFields(paymentMethod);
        }

        public object Clone()
        {
            return new PaymentMethod(this);
        }

        public void SetFields(IEntity entity)
        {
            PaymentMethod paymentMethodEntity = entity as PaymentMethod;
            PaymentMethodId = paymentMethodEntity.PaymentMethodId;
            Name = paymentMethodEntity.Name;
            SoftDeleted = paymentMethodEntity.SoftDeleted;
            Transportations = new List<Transportation>();
            if (paymentMethodEntity.Transportations != null && paymentMethodEntity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(paymentMethodEntity.Transportations);
            SoftDeleted = paymentMethodEntity.SoftDeleted;
        }
    }
}
