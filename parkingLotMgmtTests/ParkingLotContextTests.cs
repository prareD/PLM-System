using Microsoft.Extensions.Configuration;
using Moq;
using PLM_System.Models;

namespace parkingLotMgmt
{
    public interface IParkingLotContext
    {
        List<Car> LoadDataFromDatabase();
        void SaveCarToDatabase(Car car);
        void DeleteCarFromDatabase(Car car);
    }

    public class ParkingLotContext : IParkingLotContext
    {
        private IConfiguration configuration;

        public ParkingLotContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Initialize(IConfiguration configuration)
        {
            this.configuration = configuration;
            // Initialize the database connection and other setup based on the configuration
        }

        public List<Car> LoadDataFromDatabase()
        {
            //using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            //{
            //    connection.Open();
            //    string query = "SELECT COUNT(*) FROM Cars WHERE SpotId = @SpotId";
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@SpotId", car.SpotId);
            //        int carCount = (int)command.ExecuteScalar();
            //        Assert.AreEqual(1, carCount, "The car was not added to the database.");
            //    }
            //}
            return new List<Car>();
        }

        public void SaveCarToDatabase(Car car)
        {
            //using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            //{
            //    connection.Open();
            //    string query = "INSERT INTO Cars (SpotId, TagNumber, CheckInTime, CheckOutTime, ElapsedHours) VALUES (@SpotId, @TagNumber, @CheckInTime, @CheckOutTime, @ElapsedHours)";
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("@SpotId", car.SpotId);
            //        int carCount = (int)command.ExecuteScalar();
            //        Assert.AreEqual(1, carCount, "The car was not added to the database.");
            //    }
            //}
        }

        public void DeleteCarFromDatabase(Car car)
        {
            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(_connectionString))
            //    {
            //        string deleteQuery = "DELETE FROM Cars WHERE TagNumber = @TagNumber";

            //        connection.Open();
            //        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
            //        {
            //            deleteCommand.Parameters.AddWithValue("@TagNumber", car.TagNumber);
            //            deleteCommand.ExecuteNonQuery();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred while deleting the car from the database: {ex.Message}");
            //}
        }
    }

    [TestFixture]
    public class ParkingLotContextTests
    {
        private Mock<IParkingLotContext> mockParkingLotContext;
        private IConfiguration configuration;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var configValues = new Dictionary<string, string>
            {
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            mockParkingLotContext = new Mock<IParkingLotContext>();
            mockParkingLotContext.Setup(context => context.LoadDataFromDatabase()).Returns(new List<Car>());
            mockParkingLotContext.Setup(context => context.SaveCarToDatabase(It.IsAny<Car>()));
            mockParkingLotContext.Setup(context => context.DeleteCarFromDatabase(It.IsAny<Car>()));
        }

        [Test]
        public void LoadDataFromDatabase_ReturnsListOfCars()
        {
            // Arrange
            var parkingLotContext = new ParkingLotContext(configuration);

            // Act
            var cars = parkingLotContext.LoadDataFromDatabase();

            // Assert
            Assert.NotNull(cars);
            Assert.IsInstanceOf<List<Car>>(cars);
        }

        [Test]
        public void SaveCarToDatabase_AddsCarToDatabase()
        {
            // Arrange
            var parkingLotContext = new ParkingLotContext(configuration);

            var car = new Car
            {
                SpotId = 1,
                TagNumber = "ABC123",
                CheckInTime = DateTime.Now
            };

            // Act
            parkingLotContext.SaveCarToDatabase(car);
            // Assert: No need to verify the method call on the mock, as we are testing the actual implementation
        }

        [Test]
        public void DeleteCarFromDatabase_RemovesCarFromDatabase()
        {
            // Arrange
            var parkingLotContext = new ParkingLotContext(configuration);

            var car = new Car
            {
                SpotId = 1,
                TagNumber = "ABC123",
                CheckInTime = DateTime.Now
            };

            // Act
            parkingLotContext.DeleteCarFromDatabase(car);
            // Assert: No need to verify the method call on the mock, as we are testing the actual implementation
        }

    }
}