using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugstore_test
{
    public class Drugstore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public static class DrugstoreExtensions
    {
        public static bool Add(this Drugstore drugstore, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO pharmacy.dbo.Drugstores (Name, Address, Phone) VALUES ('{drugstore.Name}', '{drugstore.Address}', '{drugstore.Phone}')", connection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                Console.WriteLine("Execution Error!");
                return false;
            }
        }

        public static bool Delete(this Drugstore drugstore, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM pharmacy.dbo.Drugstores WHERE id = '{drugstore.Id}'", connection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                Console.WriteLine("Execution Error!");
                return false;
            }
        }
    }
}
