using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class StateOrder: IEntity
    {
        public int StateOrderId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public StateOrder() 
        {
            Transportations = new ObservableCollection<Transportation>();
        }

        public StateOrder(StateOrder stateOrder)
        {
            SetFields(stateOrder);
        }

        public void SetFields(IEntity stateOrder) 
        {
            StateOrder stateOrderEntity = stateOrder as StateOrder;
            StateOrderId = stateOrderEntity.StateOrderId;
            Name = stateOrderEntity.Name;
            Transportations = new ObservableCollection<Transportation>();
            if (stateOrderEntity.Transportations != null && stateOrderEntity.Transportations.Count > 0) foreach(var transportation in stateOrderEntity.Transportations) Transportations.Add(transportation);
            SoftDeleted = stateOrderEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new StateOrder(this);
        }
    }
}
