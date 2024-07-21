using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WpfAppMVVM.Model.EfCode.Entities
{
    public class Trailler : IEntity
    {
        [Key, Required, MaxLength(8)]
        public string Number { get; set; }
        public int? BrandId { get; set; }
        public bool SoftDeleted { get; set; }
        public TraillerBrand Brand { get; set; }
        public ObservableCollection<Driver> Drivers { get; set; }
        public ObservableCollection<Transportation> Transportations { get; set; }

        public Trailler() 
        {
            Drivers = new ObservableCollection<Driver>();
            Transportations = new ObservableCollection<Transportation>();
        }

        public Trailler(Trailler trailler)
        {
            SetFields(trailler);
        }

        public void SetFields(IEntity trailler) 
        {
            Trailler traiilerEntity = trailler as Trailler;
            Number = traiilerEntity.Number;
            BrandId = traiilerEntity.BrandId;
            Brand = traiilerEntity.Brand;
            Drivers = new ObservableCollection<Driver>();
            Transportations = new ObservableCollection<Transportation>();
            if (traiilerEntity.Drivers != null && traiilerEntity.Drivers.Count > 0) foreach(var driver in traiilerEntity.Drivers) Drivers.Add(driver);
            if (traiilerEntity.Transportations != null && traiilerEntity.Transportations.Count > 0) foreach(var transportation in traiilerEntity.Transportations) Transportations.Add(transportation);
            SoftDeleted = traiilerEntity.SoftDeleted;
        }

        public object Clone()
        {
            return new Trailler(this);
        }
    }
}
