using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugstore_test
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Drugstore Drugstore { get; set; }
    }

    public static class WarehouseExtensions
    {
        public static bool Add(this Warehouse warehouse, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO pharmacy.dbo.Warehouses (Name, DrugstoreId) VALUES ('{warehouse.Name}', '{warehouse.Drugstore.Id}')", connection);
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

        public static bool Delete(this Warehouse warehouse, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM pharmacy.dbo.Warehouses WHERE id = '{warehouse.Id}'", connection);
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
