using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugstore_test
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public static class ItemExtensions
    {
        public static bool Add(this Item item, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO pharmacy.dbo.Items (Name) VALUES ('{item.Name}')", connection);
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

        public static bool Delete(this Item item, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM pharmacy.dbo.Items WHERE id = '{item.Id}'", connection);
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
