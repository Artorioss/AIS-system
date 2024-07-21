using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class CarBrand: IEntity
    {
        public int CarBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public bool SoftDeleted { get; set; }
        public ObservableCollection<Car> Cars { get; set; }

        public CarBrand()
        {
            Cars = new ObservableCollection<Car>();
        }

        public CarBrand(CarBrand carBrand)
        {
            SetFields(carBrand);
        }

        public void SetFields(IEntity carBrand) 
        {
            CarBrand carBrandEntity = carBrand as CarBrand;
            CarBrandId = carBrandEntity.CarBrandId;
            Name = carBrandEntity.Name;
            RussianName = carBrandEntity.RussianName;
            Cars = new ObservableCollection<Car>();
            if (carBrandEntity != null && carBrandEntity.Cars.Count > 0) foreach(var car in carBrandEntity.Cars) Cars.Add(car);
            SoftDeleted = carBrandEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new CarBrand(this);
        }
    }
}
