namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class CarBrand: IEntity
    {
        public int CarBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Car> Cars { get; set; }

        public CarBrand()
        {
            Cars = new List<Car>();
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
            Cars = new List<Car>();
            if (carBrandEntity != null && carBrandEntity.Cars.Count > 0) (Cars as List<Car>).AddRange(carBrandEntity.Cars);
            SoftDeleted = carBrandEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new CarBrand(this);
        }
    }
}
