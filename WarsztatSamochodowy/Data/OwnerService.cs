using System.Data.SqlClient;
using System.Numerics;
using WarsztatSamochodowy.Models;

namespace WarsztatSamochodowy.Data
{
    public class OwnerServices : IOwnerService
    {
        public string ConStr { get; set; }
        public IConfiguration _configuration { get; set; }
        public SqlConnection con;
        public OwnerServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConStr = _configuration.GetConnectionString("DBConnection");
        }
        public List<CarOwner> GetOwners()
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    var cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "Select * FROM CarOwners";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<CarOwner> owners = new List<CarOwner>();

                    while (reader.Read())
                    {
                        CarOwner owner = new CarOwner();
                        owner.Id = Convert.ToInt32(reader["Id"]);
                        owner.Name = reader["Name"].ToString();
                        owner.LastName = reader["LastName"].ToString();
                        owner.PhoneNumber = Convert.ToInt32(reader["PhoneNumber"]);
                        owners.Add(owner);
                    }
                    con.Close();
                    return owners;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool OwnerCreate(string firstName, string lastName, int phoneNumber)
        {
            try
            {
                using (con = new SqlConnection(ConStr))
                {
                    con.Open();
                    var cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"INSERT INTO CarOwners " +
                        $"VALUES ('{firstName}', '{lastName}', " +
                        $"{phoneNumber}) ";
                    int rowAdded = cmd.ExecuteNonQuery();
                    if (rowAdded > 0)
                        return true;
                    else
                        return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }



}

    public interface IOwnerService
    {
        public List<CarOwner> GetOwners();
        public bool OwnerCreate(string firstName, string lastName, int phoneNumber);
    }


