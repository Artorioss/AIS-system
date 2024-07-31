using Microsoft.EntityFrameworkCore;
using Moq;
using System.Windows;
using WpfApp;
using WpfAppMVVM;
using WpfAppMVVM.Model;
using WpfAppMVVM.Model.EfCode;
using WpfAppMVVM.Model.EfCode.Entities;
using WpfAppMVVM.ViewModels.CreatingTransportation;
using WpfAppMVVM.ViewModels.OtherViewModels;
using WpfAppMVVM.Views.OtherViews;
using WpfAppMVVM.Views;
using WpfAppMVVM.ViewModels;
using NuGet.Frameworks;

namespace Test
{
    public class CreatingTransportationViewModelTests
    {
        IDisplayRootRegistry RootRegistry = new DisplayRootRegistryForTesting();
        public CreatingTransportationViewModelTests()
        {
            registrationWindows(RootRegistry);
        }

        private void registrationWindows(IDisplayRootRegistry DisplayRootRegistry)
        {
            DisplayRootRegistry.RegisterWindowType<CreatingTransportationViewModel, CreatingTransportationWindow>();
        }

        //Создание viewModel с использованием конструктора по умолчанию
        [Fact]
        public async Task CreatingViewModel_WhenNotArguments_shouldContinueWork()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var viewModel = new CreatingTransportationViewModel(Context, RootRegistry);

            //Act
            await viewModel.ShowDialog();

            // Assert
            Assert.NotNull(viewModel.Transportation);
            Assert.Equal("Создание заявки", viewModel.WindowName);
            Assert.Equal(Mode.Additing, viewModel.mode);
        }

        //Создание viewModel с передачей в конструктор объекта для редактирования с непрогруженными связанными данными.
        [Fact]
        public async Task CreatingTransportationViewModel_WhenReferenceDataNotLoaded_ShouldLoadedReferencesData()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var viewModel = new CreatingTransportationViewModel(Context.Transportations.First(), Context, RootRegistry);

            // Act
            await viewModel.ShowDialog();

            // Assert
            Assert.NotNull(viewModel.Transportation.RouteName);
            Assert.NotNull(viewModel.PaymentMethodsSource);
            Assert.NotNull(viewModel.Car);
            Assert.True(viewModel.CarSource.Count > 0);
            Assert.NotNull(viewModel.CarBrand);
            Assert.True(viewModel.CarBrandSource.Count > 0);
            Assert.NotNull(viewModel.Trailler);
            Assert.True(viewModel.TraillerSource.Count > 0);
            Assert.NotNull(viewModel.TraillerBrand);
            Assert.True(viewModel.TraillerBrandSource.Count > 0);
            Assert.NotNull(viewModel.TransportCompany);
            Assert.True(viewModel.CompaniesSource.Count > 0);
            Assert.NotNull(viewModel.Driver);
            Assert.True(viewModel.DriversSource.Count > 0);
            Assert.NotNull(viewModel.Transportation.Route);
            Assert.NotNull(viewModel.Customer);
            Assert.True(viewModel.CustomerSource.Count > 0);
            Assert.NotNull(viewModel.Transportation.StateOrder);

            Assert.Equal("Редактирование заявки", viewModel.WindowName);
            Assert.Equal(Mode.Editing, viewModel.mode);
        }

        //Создание viewModel с передачей в конструктор объекта для редактирования у которого нет связанных данных.
        [Fact]
        public async Task CreatingTransportationViewModel_WhenNotExistReferenceData_ShouldContinueWorkWithoutReferenceData()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var viewModel = new CreatingTransportationViewModel(Context.Transportations.Last(), Context, RootRegistry);

            // Act
            await viewModel.ShowDialog();

            // Assert
            Assert.NotNull(viewModel.Transportation.RouteName);
            Assert.NotNull(viewModel.Customer);
            Assert.True(viewModel.CustomerSource.Count > 0);
            Assert.NotNull(viewModel.Transportation.StateOrder);

            Assert.Equal("Редактирование заявки", viewModel.WindowName);
            Assert.Equal(Mode.Editing, viewModel.mode);

        }

        [Fact]
        public async Task UpdateEntity_UpdateCarNumberInTransportationOnAnotherExistsingCarNumberInDb_ShouldUpdateSuccessfully()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var viewModel = new CreatingTransportationViewModel(Context.Transportations.Single(t => t.TransportationId == 2), Context, RootRegistry);

            //Act
            await viewModel.ShowDialog();
            viewModel.Car = Context.Cars.Single(c => c.Number == "П148АВ77");
            viewModel.AcceptСhangesCommand.Execute(null);

            //Assert
            var tr = Context.Transportations.Single(t => t.TransportationId == 2);
            Assert.True(tr.CarNumber == "П148АВ77");
            Assert.True(tr.Car.Number == "П148АВ77");
            Assert.True(Context.Cars.Single(c => c.Number == "К123СР77").Transportations.FirstOrDefault(t => t.TransportationId == 2) is null);
            Assert.True(Context.Cars.Single(c => c.Number == "П148АВ77").Transportations.FirstOrDefault(t => t.TransportationId == 2) != null);
        }

        [Fact]
        public async Task updateEntity_UpdateCarNumberInTransportationOnAnotherExistsingCarNumberInDb_ShouldAdditingNewCarInDb()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var viewModel = new CreatingTransportationViewModel(Context.Transportations.Single(t => t.TransportationId == 3), Context, RootRegistry);

            //Act
            await viewModel.ShowDialog();
            viewModel.Car = new Car() { Number = "A111AA11" };
            viewModel.AcceptСhangesCommand.Execute(null);

            //Assert
            var tr = Context.Transportations.Single(t => t.TransportationId == 3);
            Assert.True(tr.CarNumber == "A111AA11");
            Assert.True(Context.Cars.FirstOrDefault(c => c.Number == "A111AA11") != null);
        }

        [Fact]
        public async Task addEntity_AddTransportationWithNewReferencesInDb_ShouldAdditingNewTransportationInDbAndAdditingAllReferencesInDb()
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var vieModel = new CreatingTransportationViewModel(Context, RootRegistry);

            //Act
            await vieModel.ShowDialog();
            vieModel.Car = new Car()
            {
                Number = "B111BB11",
                Brand = new CarBrand()
                {
                    Name = "TESTCARBRAND",
                    RussianName = "ТЕСТБРЕНД"
                }
            };

            vieModel.Trailler = new Trailler()
            {
                Number = "QW1234",
                Brand = new TraillerBrand()
                {
                    Name = "TESTBRAND",
                    RussianName = "TESTTRAILLERBRAND"
                }
            };

            vieModel.GeneralRoute = "TESTROUTEPOINTNAME1 - TESTROUTEPOINTNAME2";
            vieModel.Customer = new Customer() { Name = "TESTCUSTOMER" };
            vieModel.Driver = new Driver() { Name = "TESTDRIVER" };
            vieModel.PaymentMethod = new PaymentMethod() { Name = "TESTPAYMENTMETHOD" };
            vieModel.AcceptСhangesCommand.Execute(null);
            var tr = Context.Transportations.FirstOrDefault(t => t.Route.RouteName == "TESTROUTEPOINTNAME1 - TESTROUTEPOINTNAME2");

            //Assert
            Assert.NotNull(tr);
            var car = Context.Cars.FirstOrDefault(c => c.Number == "B111BB11");
            Assert.NotNull(car);
            Assert.NotNull(car.Brand);
            Assert.NotNull(Context.CarBrands.FirstOrDefault(cb => cb.Name == "TESTCARBRAND"));
            var trailler = Context.Traillers.FirstOrDefault(t => t.Number == "QW1234");
            Assert.NotNull(trailler);
            Assert.NotNull(trailler.Brand);
            Assert.NotNull(Context.TraillerBrands.FirstOrDefault(cb => cb.Name == "TESTBRAND"));
            Assert.NotNull(Context.RoutePoints.FirstOrDefault(rp => rp.Name == "TESTROUTEPOINTNAME1"));
            Assert.NotNull(Context.RoutePoints.FirstOrDefault(rp => rp.Name == "TESTROUTEPOINTNAME2"));
            Assert.NotNull(Context.Customers.FirstOrDefault(rp => rp.Name == "TESTCUSTOMER"));
            Assert.NotNull(Context.Customers.FirstOrDefault(rp => rp.Name == "TESTCUSTOMER"));
            Assert.NotNull(Context.Drivers.FirstOrDefault(rp => rp.Name == "TESTDRIVER"));
            Assert.NotNull(Context.PaymentMethods.FirstOrDefault(rp => rp.Name == "TESTPAYMENTMETHOD"));
        }

        [Fact]
        public async Task addEntity_AddTransportationWithReferencesFromDb_ShouldAdditingNewTransportationInDb() 
        {
            //Arrange
            TransportationEntities Context = InMemoryDbContextFactory.GetDbContext();
            var vieModel = new CreatingTransportationViewModel(Context, RootRegistry);

            //Act
            await vieModel.ShowDialog();
            vieModel.Car = Context.Cars.Single(c => c.Number == "П148АВ77");
            vieModel.Trailler = Context.Traillers.Single(t => t.Number == "TR1234");
            vieModel.GeneralRoute = "TESTPOINT1 - TESTPOINT2";
            vieModel.Customer = Context.Customers.Single(c => c.Name == "Customer One");
            vieModel.Driver = Context.Drivers.Single(d => d.Name == "John Doe");
            vieModel.PaymentMethod = Context.PaymentMethods.Single(p => p.Name == "Credit Card");
            vieModel.AcceptСhangesCommand.Execute(null);
            var tr = Context.Transportations.FirstOrDefault(t => t.Route.RouteName == "TESTPOINT1 - TESTPOINT2");

            //Assert
            Assert.NotNull(tr);
            Assert.True(tr.CarNumber == "П148АВ77");
            Assert.True(tr.TraillerNumber == "TR1234");
            Assert.True(tr.CustomerId == 1);
            Assert.True(tr.PaymentMethodId == 1);
            Assert.True(tr.DriverId == 1);
        }
    }
}
