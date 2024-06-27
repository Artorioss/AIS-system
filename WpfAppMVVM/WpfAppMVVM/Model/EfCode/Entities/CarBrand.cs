namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class CarBrand: ICloneable
    {
        public int CarBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public ICollection<Car> Cars { get; set; }

        public CarBrand()
        {
            Cars = new List<Car>();
        }

        public CarBrand(CarBrand carBrand)
        {
            SetFields(carBrand);
        }

        public void SetFields(CarBrand carBrand) 
        {
            CarBrandId = carBrand.CarBrandId;
            Name = carBrand.Name;
            RussianName = carBrand.RussianName;
            Cars = carBrand.Cars;
        }

        public object Clone()
        {
            return new CarBrand(this);
        }
    }
}
