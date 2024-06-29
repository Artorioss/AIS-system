namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class TraillerBrand: ICloneable
    {
        public int TraillerBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public ICollection<Trailler> Traillers { get; set; }

        public TraillerBrand() 
        {
            Traillers = new List<Trailler>();
        }

        public TraillerBrand(TraillerBrand traillerBrand)
        {
            SetFields(traillerBrand);
        }

        public void SetFields(TraillerBrand traillerBrand) 
        {
            TraillerBrandId = traillerBrand.TraillerBrandId;
            Name = traillerBrand.Name;
            RussianName = traillerBrand.RussianName;
            Traillers = traillerBrand.Traillers;
        }

        public object Clone()
        {
            return new TraillerBrand(this);
        }
    }
}
