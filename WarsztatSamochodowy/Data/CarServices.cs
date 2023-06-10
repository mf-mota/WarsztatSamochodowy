using System.Data.SqlClient;
using WarsztatSamochodowy.Models;

namespace WarsztatSamochodowy.Data
{
    public class CarServices : ICarService
    {
        public string ConStr { get; set; }
        public IConfiguration _configuration { get; set; }
        public SqlConnection con;
        public CarServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConStr = _configuration.GetConnectionString("DBConnection");
        }
        public List<Car> GetCars()
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    //var cmd = new SqlCommand("GetCars", con);
                    var cmd = new SqlCommand();
                    //here i added the query

                    cmd.Connection = con;
                    cmd.CommandText = "Select * FROM Cars";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Car> cars = new List<Car>();

                    while (reader.Read())
                    {
                        Car car = new Car();
                        car.CarId = Convert.ToInt32(reader["CarId"]);
                        car.CarMake = reader["CarMake"].ToString();
                        car.CarModel = reader["CarModel"].ToString();
                        car.Arrival = Convert.ToDateTime(reader["Arrival"]);
                        car.DateDone = Convert.ToDateTime(reader["DateDone"]);
                        car.Owner = GetCarOwner(Convert.ToInt32(reader["Owner"]));
                        car.Registration = reader["Registration"].ToString();
                        cars.Add(car);
                    }
                    return cars;
                }

            }
            catch (Exception)
            {
                throw;

            }
        }
        //thanks chat gpt for the great help
        private CarOwner GetCarOwner(int ownerId)
        {
            using (var con = new SqlConnection(ConStr))
            {
                con.Open();
                var cmd = new SqlCommand("SELECT * FROM CarOwners WHERE Id = @OwnerId", con);
                cmd.Parameters.AddWithValue("@OwnerId", ownerId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CarOwner owner = new CarOwner();
                    owner.Id = Convert.ToInt32(reader["Id"]);
                    owner.Name = reader["Name"].ToString();
                    owner.LastName = reader["LastName"].ToString();
                    owner.PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                    return owner;
                }
            }

            return null;

        }

        public Car DisplayCar(int carId)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    //var cmd = new SqlCommand("GetCars", con);
                    var cmd = new SqlCommand();
                    //here i added the query

                    cmd.Connection = con;
                    cmd.CommandText = $"Select * FROM Cars WHERE CarId={carId}";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Car car = new Car();
                        car.CarId = Convert.ToInt32(reader["CarId"]);
                        car.CarMake = reader["CarMake"].ToString();
                        car.CarModel = reader["CarModel"].ToString();
                        car.Arrival = Convert.ToDateTime(reader["Arrival"]);
                        car.DateDone = Convert.ToDateTime(reader["DateDone"]);
                        car.Owner = GetCarOwner(Convert.ToInt32(reader["Owner"]));
                        car.Registration = reader["Registration"].ToString();
                        return car;
                    }
                    else
                        return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteCar(int carId)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();
                    try
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.Transaction = tran;
                        cmd.CommandText = $"DELETE FROM Cars WHERE CarId={carId}";
                        int rowsDeleted = cmd.ExecuteNonQuery();
                        tran.Commit();
                        return rowsDeleted > 0;
                    }

                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool EditCar(int carId, string carMake, string carModel, int ownerId, string registration)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlTransaction tranEdit = con.BeginTransaction();

                    try
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.Transaction = tranEdit;
                        cmd.CommandText = $"Update Cars " +
                            $"SET CarMake='{carMake}', CarModel='{carModel}', " +
                            $"Owner={ownerId}, Registration='{registration}' " +
                            $"WHERE CarId={carId}";
                        int rowChanged = cmd.ExecuteNonQuery();
                        tranEdit.Commit();
                        return rowChanged > 0;

                    }
                    catch (Exception)
                    {
                        tranEdit.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool CreateCar(string carMake, string carModel, string arrival, string dateDone, int ownerId, string registration)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        var cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.Transaction = transaction;
                        cmd.CommandText = $"INSERT INTO Cars " +
                            $"VALUES ('{carMake}', '{carModel}', " +
                            $"{arrival}, {dateDone}, {ownerId}, '{registration}')";

                        int rowsAdded = cmd.ExecuteNonQuery();

                        transaction.Commit();

                        return rowsAdded > 0;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}




public interface ICarService
{
    public List<Car> GetCars();
    public bool DeleteCar(int carId);
    public Car DisplayCar(int carId);
    public bool EditCar(int carId, string carMake, string carModel, int ownerId, string registration);
    public bool CreateCar(string carMake, string carModel, string arrival, string dateDone, int ownerId, string registration);
}


