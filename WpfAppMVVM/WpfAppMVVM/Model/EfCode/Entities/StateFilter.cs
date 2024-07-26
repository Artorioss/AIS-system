using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class StateFilter : IEntity
    {
        public int StateFilterId { get; set; }
        public string Name { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<StateOrder> StateOrders { get; set; }

        public StateFilter() 
        {
            StateOrders = new ObservableCollection<StateOrder>();
        }

        public StateFilter(StateFilter stateFilter) 
        {
            SetFields(stateFilter);
        }

        public object Clone()
        {
            return new StateFilter(this);
        }

        public void SetFields(IEntity entity)
        {
            StateFilter stateFilter = entity as StateFilter;
            StateFilterId = stateFilter.StateFilterId;
            Name = stateFilter.Name;
            SoftDeleted = stateFilter.SoftDeleted;
            StateOrders = new ObservableCollection<StateOrder>();
            if(stateFilter.StateOrders != null && stateFilter.StateOrders.Count > 0) foreach(var state in stateFilter.StateOrders) StateOrders.Add(state);   
        }
    }
}
