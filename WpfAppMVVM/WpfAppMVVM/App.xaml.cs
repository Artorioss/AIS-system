using System.Configuration;
using System.Data;
using System.Windows;
using WpfAppMVVM.Models;

namespace WpfAppMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TransportationEntities _context { get; set; }
        public App() 
        {
            _context = new TransportationEntities();
        }
    }

}
