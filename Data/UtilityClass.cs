using PLM_System.Models;
using System.Data;
using System.Data.SqlClient;

namespace PLM_System.Data
{
    public static class UtilityClass
    {
        public static List<Car> LoadDataFromDatabase()
        {
            string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Plm_System;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Cars";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                connection.Open();
                Console.WriteLine("Connection successful");
                adapter.Fill(dataTable);

                List<Car> cars = new List<Car>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Car car = new Car
                    {
                        SpotId = Convert.ToInt32(row["SpotId"]),
                        TagNumber = row["TagNumber"].ToString(),
                        CheckInTime = Convert.ToDateTime(row["CheckInTime"]),
                        CheckOutTime = row.IsNull("CheckOutTime") ? null : (DateTime?)Convert.ToDateTime(row["CheckOutTime"])
                    };
                    cars.Add(car);
                }

                return cars;
            }
        }


    }
}
