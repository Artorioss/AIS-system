namespace WpfAppMVVM.Model.EfCode.Entities
{
    public interface IEntity: ICloneable
    {
        public bool SoftDeleted { get; set; }
        public void SetFields(IEntity entity);
    }
}
