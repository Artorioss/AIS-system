using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class PaymentMethod: IEntity
    {
        public int PaymentMethodId { get; set; }
        [MaxLength(16)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public PaymentMethod() 
        {
            Transportations = new ObservableCollection<Transportation>(); 
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
            Transportations = new ObservableCollection<Transportation>();
            if (paymentMethodEntity.Transportations != null && paymentMethodEntity.Transportations.Count > 0) foreach(var transportation in paymentMethodEntity.Transportations) Transportations.Add(transportation);
            SoftDeleted = paymentMethodEntity.SoftDeleted;
        }
    }
}
