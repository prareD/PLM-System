using Microsoft.AspNetCore.Mvc;
using PLM_System.Data;
using PLM_System.Models;

namespace PLM_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParkingLotContext _parkingLotContext;
        private readonly int _totalSpots;
        private readonly decimal _hourlyFee;

        public HomeController(ParkingLotContext parkingLotContext, IConfiguration configuration)
        {
            _parkingLotContext = parkingLotContext;
            _totalSpots = configuration.GetValue<int>("AppSettings:TotalSpots");
            _hourlyFee = configuration.GetValue<decimal>("AppSettings:HourlyFee");

            _parkingLotContext.Initialize(configuration);
        }

        // GET: Home/Index
        // Pass data to the view
        public IActionResult Index()
        {
            var cars = _parkingLotContext.LoadDataFromDatabase();

            ViewBag.Cars = cars;
            ViewBag.HourlyFee = _hourlyFee;
            ViewBag.TotalSpots = _totalSpots;
            ViewBag.ElapsedTime = '-';

            return View(cars);
        }

        // POST: Home/CheckIn
        //Triggers on CheckInBtn in View clicked
        [HttpPost]
        public IActionResult CheckIn(string tagNumber)
        {
            var cars = _parkingLotContext.LoadDataFromDatabase();

            if (cars.Count(c => c.CheckOutTime == null) >= _totalSpots)
            {
                return Json(new { errorMessage = "No spots available." });
            }

            var car = cars.FirstOrDefault(c => c.TagNumber == tagNumber && c.CheckOutTime == null);
            if (car != null)
            {
                return Json(new { errorMessage = "Car is already in the parking lot." });
            }

            var random = new Random();
            var spotId = random.Next(1, _totalSpots + 1);

            var newCar = new Car
            {
                TagNumber = tagNumber,
                SpotId = spotId,
                CheckInTime = DateTime.Now
            };

            cars.Add(newCar);
            _parkingLotContext.SaveCarToDatabase(newCar);

            return Json(new { cars });
        }

        // POST: Home/CheckOut
        //Triggers on CheckOutBtn in View clicked
        [HttpPost]
        public IActionResult CheckOut(string tagNumber)
        {
            var cars = _parkingLotContext.LoadDataFromDatabase();

            var car = cars.FirstOrDefault(c => c.TagNumber == tagNumber);
            if (car == null)
            {
                return Json(new { errorMessage = "Car is not registered in the parking lot." });
            }

            car.CheckOutTime = DateTime.Now;
            var totalHours = car.CheckOutTime.HasValue ? (decimal)(car.CheckOutTime.Value - car.CheckInTime).TotalHours : 0;
            var totalAmount = Math.Ceiling(totalHours) * _hourlyFee;

            _parkingLotContext.DeleteCarFromDatabase(car);

            return Json(new { cars, totalAmount = totalAmount.ToString("C") });
        }

        // GET: Home/Stats
        //Triggers on statsbtn in View clicked
        [HttpGet]
        public IActionResult Stats()
        {
            var cars = _parkingLotContext.LoadDataFromDatabase();

            var stats = new ParkingSpots
            {
                AvailableSpots = _totalSpots - cars.Count,
                Revenue = cars.Sum(c => (decimal)((c.CheckOutTime - c.CheckInTime)?.TotalHours ?? 0) * _hourlyFee) / 30,
                AverageCarsPerDay = _parkingLotContext.CalculateAverageCarsPerDay(cars)
            };

            return Json(new { cars, stats });
        }
    }
}
