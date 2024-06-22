using System.Diagnostics.CodeAnalysis;
using WpfAppMVVM.Model.EfCode.Entities;

namespace WpfAppMVVM.Model
{
    internal class TraillerBrandComparer : IEqualityComparer<TraillerBrand>
    {
        public bool Equals(TraillerBrand? x, TraillerBrand? y)
        {
            return x.TraillerBrandId == y.TraillerBrandId;
        }

        public int GetHashCode([DisallowNull] TraillerBrand obj)
        {
            return obj.TraillerBrandId.GetHashCode();
        }
    }
}
