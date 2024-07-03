using System.Diagnostics.CodeAnalysis;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.Model
{
    internal class CarBrandComparer : IEqualityComparer<CarBrand>
    {
        public bool Equals(CarBrand? x, CarBrand? y)
        {
            return x.CarBrandId == y.CarBrandId;
        }

        public int GetHashCode([DisallowNull] CarBrand obj)
        {
            return obj.CarBrandId.GetHashCode();
        }
    }
}
