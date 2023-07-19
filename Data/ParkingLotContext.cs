using Microsoft.Extensions.Configuration;
using PLM_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PLM_System.Data
{
    public  class ParkingLotContext
    {
        public string _connectionString;
        public  List<Car> cars = new List<Car>();

        public  void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public  List<Car> LoadDataFromDatabase()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Cars";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Car car = new Car
                    {
                        SpotId = Convert.ToInt32(reader["SpotId"]),
                        TagNumber = reader["TagNumber"].ToString(),
                        CheckInTime = Convert.ToDateTime(reader["CheckInTime"]),
                        CheckOutTime = reader.IsDBNull(reader.GetOrdinal("CheckOutTime")) ? null : (DateTime?)reader["CheckOutTime"]
                    };
                    cars.Add(car);
                }
                connection.Close();
            }

            return cars;
        }


        public void SaveCarToDatabase(Car car)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string MQuery = "INSERT INTO Cars (SpotId, TagNumber, CheckInTime) VALUES (@SpotId, @TagNumber, @CheckInTime)";
                    using (SqlCommand updateCommand = new SqlCommand(MQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@SpotId", car.SpotId);
                        if (car.CheckOutTime == null)
                        {
                            updateCommand.Parameters.AddWithValue("@CheckInTime", car.CheckInTime);
                        }
                        updateCommand.Parameters.AddWithValue("@CheckOutTime", (object)car.CheckOutTime ?? DBNull.Value);
                        updateCommand.Parameters.AddWithValue("@TagNumber", car.TagNumber);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the car to the database: {ex.Message}");
            }
        }
        public void DeleteCarFromDatabase(Car car)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string deleteQuery = "DELETE FROM Cars WHERE TagNumber = @TagNumber";

                    connection.Open();
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@TagNumber", car.TagNumber);
                        deleteCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the car from the database: {ex.Message}");
            }
        }

        public int CalculateAverageCarsPerDay(List<Car> cars)
        {
            var groupedCars = cars.GroupBy(c => c.CheckInTime.Date);
            var totalDays = groupedCars.Count();
            var totalCars = groupedCars.Sum(group => group.Count());

            return totalDays > 0 ? totalCars / totalDays : 0;
        }
    }
}
