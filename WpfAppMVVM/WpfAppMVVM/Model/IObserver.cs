using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Net;

namespace WpfAppMVVM.Model
{
    public interface IObserver
    {
        public void Update();
    }
}
