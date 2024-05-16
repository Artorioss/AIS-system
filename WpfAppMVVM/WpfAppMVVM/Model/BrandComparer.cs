using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppMVVM.Model.Entities;

namespace WpfAppMVVM.Model
{
    internal class BrandComparer : IEqualityComparer<Brand>
    {
        public bool Equals(Brand? x, Brand? y)
        {
            return x.BrandId == y.BrandId;
        }

        public int GetHashCode([DisallowNull] Brand obj)
        {
            return obj.BrandId.GetHashCode();
        }
    }
}
