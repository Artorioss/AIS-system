namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void GetCars_ReturnsCars()
        {
            // Arrange
            var cars = new List<Car>
        {
            new Car { Id = 1, Name = "Car1" },
            new Car { Id = 2, Name = "Car2" }
        }.AsQueryable();

            var mockSet = new Mock<DbSet<Car>>();
            mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.Provider);
            mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.Expression);
            mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.ElementType);
            mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.GetEnumerator());

            var mockContext = new Mock<TransportationEntities>();
            mockContext.Setup(c => c.Cars).Returns(mockSet.Object);

            // Act
            var service = new CarService(mockContext.Object);
            var result = service.GetCars();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Car1", result.First().Name);
        }
    }
}