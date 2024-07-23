using System.Windows;
using WpfApp;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.ViewModels;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.ViewModels.OtherViewModels;
using WpfAppMVVM.Views;
using WpfAppMVVM.Views.OtherViews;

namespace WpfAppMVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TransportationEntities _context { get; set; }
        public DisplayRootRegistry DisplayRootRegistry { get; set; }
        public App()
        {
            _context = new TransportationEntities();
            DisplayRootRegistry = new DisplayRootRegistry();
            registrationWindows();
        }

        private void registrationWindows() 
        {
            DisplayRootRegistry.RegisterWindowType<CreatingTransportationViewModel, CreatingTransportationWindow>();
            DisplayRootRegistry.RegisterWindowType<ReferenceBookViewModel, WindowReferencesBook>();
            DisplayRootRegistry.RegisterWindowType<CarBrandViewModel, CarBrandWindow>();
            DisplayRootRegistry.RegisterWindowType<CarViewModel, CarWindow>();
            DisplayRootRegistry.RegisterWindowType<CustomerViewModel, CustomerWindow>();
            DisplayRootRegistry.RegisterWindowType<DriverViewModel, DriverWindow>();
            DisplayRootRegistry.RegisterWindowType<RouteViewModel, RouteWindow>();
            DisplayRootRegistry.RegisterWindowType<RoutePointViewModel, RoutePointWindow>();
            DisplayRootRegistry.RegisterWindowType<StateOrderViewModel, StateWindow>();
            DisplayRootRegistry.RegisterWindowType<TraillerBrandViewModel, TraillerBrandWindow>();
            DisplayRootRegistry.RegisterWindowType<TraillerViewModel, TraillerWindow>();
            DisplayRootRegistry.RegisterWindowType<TransportCompanyViewModel, TransportCompanyWindow>();
        }
    }

}
