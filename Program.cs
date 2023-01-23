using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drugstore_test
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string showItems = "SELECT * FROM pharmacy.dbo.Items";
            string showDrugstores = "SELECT * FROM pharmacy.dbo.Drugstores";
            string showWarehouses = "SELECT * FROM pharmacy.dbo.Warehouses";
            string showConsignments = "SELECT * FROM pharmacy.dbo.Consignments";
            SqlCommand command;
            SqlDataReader reader;
            var items = new List<Item>();
            var drugstores = new List<Drugstore>();
            var warehouses = new List<Warehouse>();
            var consignments = new List<Consignment>();
            int option;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    while (true)
                    {
                        // Заполнение текущих списков товаров, аптек, складов и партий товаров для дальнейшей обработки
                        items.Clear();
                        drugstores.Clear();
                        warehouses.Clear();
                        consignments.Clear();
                        // Заполнение списка товаров
                        command = new SqlCommand(showItems, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            items.Add(new Item { Id = reader.GetGuid(0), Name = reader.GetString(1) });
                        }
                        reader.Close();

                        // Заполнение списка аптек

                        command = new SqlCommand(showDrugstores, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            drugstores.Add(new Drugstore { Id = reader.GetGuid(0), Name = reader.GetString(1), Address = reader.GetString(2), Phone = reader.GetString(3) });
                        }
                        reader.Close();

                        // Заполнение списка складов

                        command = new SqlCommand(showWarehouses, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            warehouses.Add(new Warehouse { Id = reader.GetGuid(0), Name = reader.GetString(1), Drugstore = drugstores.Find(drugstore => drugstore.Id == reader.GetGuid(2))});
                        }
                        reader.Close();

                        // Заполнение списка партий

                        command = new SqlCommand(showConsignments, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            consignments.Add(new Consignment { Id = reader.GetGuid(0), Quantity = reader.GetInt64(1), Warehouse = warehouses.Find(warehouse => warehouse.Id == reader.GetGuid(2)), Item = items.Find(item => item.Id == reader.GetGuid(3)) });
                        }
                        reader.Close();

                        Console.WriteLine();
                        Console.WriteLine("Choose Cathegory");
                        Console.WriteLine("1. Items");
                        Console.WriteLine("2. Drugstores");
                        Console.WriteLine("3. Warehouses");
                        Console.WriteLine("4. Consignments");
                        Console.WriteLine("5. Item quantity in selected drugstore");
                        Console.WriteLine();

                        try
                        {
                            option = Convert.ToInt32(Console.ReadLine());
                            switch (option)
                            {
                                // ОПЕРАЦИИ С ТОВАРОМ
                                case 1:
                                    Console.WriteLine();
                                    Console.WriteLine("1. Add");
                                    Console.WriteLine("2. Delete");
                                    Console.WriteLine("3. Print");
                                    Console.WriteLine();
                                    try
                                    {
                                        option = Convert.ToInt32(Console.ReadLine());
                                        switch (option)
                                        {
                                            // Добавить товар
                                            case 1:
                                                Console.WriteLine();
                                                Console.WriteLine("Insert item name:");
                                                Console.WriteLine();
                                                var item = new Item { Name = Console.ReadLine().ToString() };
                                                item.Add(connection);
                                                break;
                                            // Удалить товар
                                            case 2:
                                                Console.WriteLine();
                                                Console.WriteLine("Choose item:");
                                                Console.WriteLine();
                                                
                                                foreach (Item i in items) {
                                                    Console.WriteLine($"{items.IndexOf(i)}. {i.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    items[Convert.ToInt32(Console.ReadLine())].Delete(connection);
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                }
                                                break;
                                            // Вывод списка товаров
                                            case 3:
                                                Console.WriteLine();
                                                Console.WriteLine("                 Id                  | Name");
                                                Console.WriteLine("_____________________________________|________________");
                                                foreach (Item i in items)
                                                {
                                                    Console.WriteLine($"{i.Id} | {i.Name}");
                                                }
                                                Console.WriteLine("");
                                                break;
                                            default:
                                                Console.WriteLine("Wrong Input!");
                                                break;
                                        }
                                    } catch
                                    {
                                        Console.WriteLine("Wrong Input!");
                                    }
                                    break;
                                // ОПЕРАЦИИ С АПТЕКАМИ
                                case 2:
                                    Console.WriteLine();
                                    Console.WriteLine("1. Add");
                                    Console.WriteLine("2. Delete");
                                    Console.WriteLine("3. Print");
                                    Console.WriteLine();
                                    try
                                    {
                                        option = Convert.ToInt32(Console.ReadLine());
                                        switch (option)
                                        {
                                            // Добавить аптеку
                                            case 1:
                                                Console.WriteLine();
                                                Console.WriteLine("Insert drugstore name:");
                                                Console.WriteLine();
                                                var drugstoreName = Console.ReadLine().ToString();
                                                Console.WriteLine();
                                                Console.WriteLine("Insert drugstore address:");
                                                Console.WriteLine();
                                                var drugstoreAddress = Console.ReadLine().ToString();
                                                Console.WriteLine();
                                                Console.WriteLine("Insert drugstore phone:");
                                                Console.WriteLine();
                                                var drugstorePhone = Console.ReadLine().ToString();
                                                var drugstore = new Drugstore { Name = drugstoreName, Address = drugstoreAddress, Phone = drugstorePhone };
                                                drugstore.Add(connection);
                                                break;
                                            // Удалить аптеку
                                            case 2:
                                                Console.WriteLine();
                                                Console.WriteLine("Choose Drugstore:");
                                                Console.WriteLine();
                                                foreach (Drugstore i in drugstores)
                                                {
                                                    Console.WriteLine($"{drugstores.IndexOf(i)}. {i.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    drugstores[Convert.ToInt32(Console.ReadLine())].Delete(connection);
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                }
                                                break;
                                            // Вывод списка аптек
                                            case 3:
                                                Console.WriteLine();
                                                Console.WriteLine("                 Id                  |                 Name                |               Address               |              Phone               ");
                                                Console.WriteLine("_____________________________________|_____________________________________|_____________________________________|_____________________________________");
                                                foreach (Drugstore i in drugstores)
                                                {
                                                    Console.WriteLine($"{i.Id} | {i.Name} | {i.Address} | {i.Phone}");
                                                }
                                                Console.WriteLine("");
                                                break;
                                            default:
                                                Console.WriteLine("Wrong Input!");
                                                break;
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Wrong Input!");
                                    }
                                    break;
                                // ОПЕРАЦИИ СО СКЛАДАМИ
                                case 3:
                                    Console.WriteLine();
                                    Console.WriteLine("1. Add");
                                    Console.WriteLine("2. Delete");
                                    Console.WriteLine("3. Print");
                                    Console.WriteLine();
                                    try
                                    {
                                        option = Convert.ToInt32(Console.ReadLine());
                                        switch (option)
                                        {
                                            // Добавить склад
                                            case 1:
                                                Console.WriteLine();
                                                Console.WriteLine("Insert warehouse name:");
                                                Console.WriteLine();
                                                var warehouseName = Console.ReadLine().ToString();
                                                Console.WriteLine();
                                                Console.WriteLine("Choose drugstore:");
                                                Console.WriteLine();
                                                var drugstore = new Drugstore();
                                                foreach (Drugstore i in drugstores)
                                                {
                                                    Console.WriteLine($"{drugstores.IndexOf(i)}. {i.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    drugstore = drugstores[Convert.ToInt32(Console.ReadLine())];
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                    break;
                                                }
                                                var warehouse = new Warehouse { Name = warehouseName, Drugstore = drugstore };
                                                warehouse.Add(connection);
                                                break;
                                            // Удалить склад
                                            case 2:
                                                Console.WriteLine();
                                                Console.WriteLine("Choose Warehouse:");
                                                Console.WriteLine();
                                                foreach (Warehouse i in warehouses)
                                                {
                                                    Console.WriteLine($"{warehouses.IndexOf(i)}. {i.Name} in drugstore {i.Drugstore.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    warehouses[Convert.ToInt32(Console.ReadLine())].Delete(connection);
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                }
                                                break;
                                            case 3:
                                                
                                                Console.WriteLine();
                                                Console.WriteLine("                 Id                  |                 Name                |               Drugstore               ");
                                                Console.WriteLine("_____________________________________|_____________________________________|_______________________________________");
                                                foreach (Warehouse i in warehouses)
                                                {
                                                    Console.WriteLine($"{i.Id} | {i.Name} | {i.Drugstore.Name}");
                                                }
                                                Console.WriteLine("");
                                                break;
                                            default:
                                                Console.WriteLine("Wrong Input!");
                                                break;
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Wrong Input!");
                                    }
                                    break;
                                // ОПЕРАЦИИ С ПАРТИЯМИ
                                case 4:
                                    Console.WriteLine();
                                    Console.WriteLine("1. Add");
                                    Console.WriteLine("2. Delete");
                                    Console.WriteLine("3. Print");
                                    Console.WriteLine();
                                    try
                                    {
                                        option = Convert.ToInt32(Console.ReadLine());
                                        switch (option)
                                        {
                                            // Добавить партию
                                            case 1:
                                                int quantity;
                                                Console.WriteLine();
                                                Console.WriteLine("Insert item quantity:");
                                                Console.WriteLine();
                                                try
                                                {
                                                    quantity = Convert.ToInt32(Console.ReadLine());
                                                } catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                    break;
                                                }

                                                Console.WriteLine();
                                                Console.WriteLine("Choose item:");
                                                Console.WriteLine();
                                                var item = new Item();
                                                foreach (Item i in items)
                                                {
                                                    Console.WriteLine($"{items.IndexOf(i)}. {i.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    item = items[Convert.ToInt32(Console.ReadLine())];
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                    break;
                                                }

                                                Console.WriteLine();
                                                Console.WriteLine("Choose warehouse:");
                                                Console.WriteLine();
                                                var warehouse = new Warehouse();
                                                foreach (Warehouse i in warehouses)
                                                {
                                                    Console.WriteLine($"{warehouses.IndexOf(i)}. {i.Name} in drugstore {i.Drugstore.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    warehouse = warehouses[Convert.ToInt32(Console.ReadLine())];
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                    break;
                                                }
                                                var consignment = new Consignment {Quantity = quantity, Item = item, Warehouse = warehouse};
                                                consignment.Add(connection);
                                                break;
                                            // Удалить партию
                                            case 2:
                                                Console.WriteLine();
                                                Console.WriteLine("Choose consignment:");
                                                Console.WriteLine();
                                                foreach (Consignment i in consignments)
                                                {
                                                    Console.WriteLine($"{consignments.IndexOf(i)}. {i.Quantity} pts. of {i.Item.Name} in warehouse {i.Warehouse.Name} in drugstore {i.Warehouse.Drugstore.Name}");
                                                }
                                                Console.WriteLine("");
                                                try
                                                {
                                                    consignments[Convert.ToInt32(Console.ReadLine())].Delete(connection);
                                                }
                                                catch
                                                {
                                                    Console.WriteLine("Wrong Input!");
                                                }
                                                break;
                                            // Вывод на экран
                                            case 3:
                                                Console.WriteLine();
                                                Console.WriteLine("                 Id                  |                 Item                |               Quantity               |               Warehouse               |               Drugstore              ");
                                                Console.WriteLine("_____________________________________|_____________________________________|______________________________________|_______________________________________|______________________________________");
                                                foreach (Consignment i in consignments)
                                                {
                                                    Console.WriteLine($"{i.Id} | {i.Item.Name} | {i.Quantity} | {i.Warehouse.Name} | {i.Warehouse.Drugstore.Name}");
                                                }
                                                Console.WriteLine("");
                                                break;
                                            default:
                                                Console.WriteLine("Wrong Input!");
                                                break;
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Wrong Input!");
                                    }
                                    break;
                                // Вывести на экран весь список товаров и его количество в выбранной аптеке (количество товара во всех складах аптеки)
                                case 5:
                                    Console.WriteLine();
                                    Console.WriteLine("Select drugstore:");
                                    Console.WriteLine();
                                    foreach (Drugstore i in drugstores)
                                    {
                                        Console.WriteLine($"{drugstores.IndexOf(i)}. {i.Name} located by {i.Address}");
                                    }
                                    Console.WriteLine();
                                    try
                                    {
                                        var drugstore = drugstores[Convert.ToInt32(Console.ReadLine())];
                                        SqlCommand getItemsInSelectedDrugstore = new SqlCommand($"SELECT pharmacy.dbo.Items.Name, SUM(pharmacy.dbo.Consignments.Quantity) AS Quantity FROM pharmacy.dbo.Items " +
                                                                                                $"JOIN pharmacy.dbo.Consignments ON pharmacy.dbo.Items.Id = pharmacy.dbo.Consignments.ItemId " +
                                                                                                $"JOIN pharmacy.dbo.Warehouses ON pharmacy.dbo.Warehouses.Id = pharmacy.dbo.Consignments.WarehouseId " +
                                                                                                $"JOIN pharmacy.dbo.Drugstores ON pharmacy.dbo.Drugstores.Id = pharmacy.dbo.Warehouses.DrugstoreId " +
                                                                                                $"WHERE DrugstoreId = '{drugstore.Id}' " +
                                                                                                $"GROUP BY pharmacy.dbo.Items.Name", connection);
                                        try
                                        {
                                            reader = getItemsInSelectedDrugstore.ExecuteReader();
                                            while (reader.Read())
                                            {
                                                Console.WriteLine($"{reader.GetString(0)}, {reader.GetInt64(1)} pts. left");
                                            }
                                            reader.Close();
                                        } catch (SqlException)
                                        {
                                            Console.WriteLine("SQL ERROR!");
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Wrong Input!");
                                        break;
                                    }

                                    break;
                                default:
                                    Console.WriteLine("Wrong Input!");
                                    break;
                            }
                        } catch
                        {
                            Console.WriteLine("Wrong Input!");
                        }

                    }
                } catch (SqlException)
                {
                    Console.WriteLine("Ошибка подключения");
                }
            Console.WriteLine("Подключение закрыто...");

                Console.Read();
            }
        }
    }
}
