namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class TraillerBrand: IEntity
    {
        public int TraillerBrandId { get; set; }
        public string Name { get; set; }
        public string? RussianName { get; set; }
        public bool SoftDeleted { get; set; }
        public ICollection<Trailler> Traillers { get; set; }

        public TraillerBrand() 
        {
            Traillers = new List<Trailler>();
        }

        public TraillerBrand(TraillerBrand traillerBrand)
        {
            SetFields(traillerBrand);
        }

        public void SetFields(IEntity traillerBrand) 
        {
            TraillerBrand traillerBrandEntity = traillerBrand as TraillerBrand;
            TraillerBrandId = traillerBrandEntity.TraillerBrandId;
            Name = traillerBrandEntity.Name;
            RussianName = traillerBrandEntity.RussianName;
            Traillers = new List<Trailler>();
            if (traillerBrandEntity.Traillers != null && traillerBrandEntity.Traillers.Count > 0) (Traillers as List<Trailler>).AddRange(traillerBrandEntity.Traillers);
            SoftDeleted = traillerBrandEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new TraillerBrand(this);
        }
    }
}
