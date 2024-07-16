using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class StateOrder: IEntity
    {
        public int StateOrderId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public StateOrder() 
        {
            Transportations = new List<Transportation>();
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
            Transportations = new List<Transportation>();
            if (stateOrderEntity.Transportations != null && stateOrderEntity.Transportations.Count > 0) (Transportations as List<Transportation>).AddRange(stateOrderEntity.Transportations);
            SoftDeleted = stateOrderEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new StateOrder(this);
        }
    }
}
