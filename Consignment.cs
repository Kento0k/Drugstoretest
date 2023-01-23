using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugstore_test
{
    public class Consignment
    {
        public Guid Id { get; set; }
        public Item Item { get; set; }
        public Warehouse Warehouse { get; set; }
        public long Quantity { get; set; }
    }

    public static class ConsignmentExtensions
    {
        public static bool Add(this Consignment consignment, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO pharmacy.dbo.Consignments (Quantity, WarehouseId, ItemId) VALUES ('{consignment.Quantity}', '{consignment.Warehouse.Id}', '{consignment.Item.Id}')", connection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            } catch (SqlException)
            {
                Console.WriteLine("Execution Error!");
                return false;
            }
        }

        public static bool Delete(this Consignment consignment, SqlConnection connection)
        {
            SqlCommand sqlCommand = new SqlCommand($"DELETE FROM pharmacy.dbo.Consignments WHERE id = '{consignment.Id}'", connection);
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
