using System.ComponentModel.DataAnnotations;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class StateOrder: ICloneable
    {
        public int StateOrderId { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        public ICollection<Transportation> Transportations { get; set; }

        public StateOrder() 
        {
            Transportations = new HashSet<Transportation>();
        }

        public StateOrder(StateOrder stateOrder)
        {
            SetFields(stateOrder);
        }

        public void SetFields(StateOrder stateOrder) 
        {
            StateOrderId = stateOrder.StateOrderId;
            Name = stateOrder.Name;
            Transportations = stateOrder.Transportations;
        }

        public object Clone()
        {
            return new StateOrder(this);
        }
    }
}
