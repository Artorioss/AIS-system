using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net;

namespace WpfAppMVVM.Model
{
    internal interface IObserver
    {
        public void Update();
    }
}
