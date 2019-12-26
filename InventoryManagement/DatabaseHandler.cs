using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace InventoryManagement
{
    public class DatabaseHandler
    {
        private string _databasePATH = "database.db";

        private int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;

            using (var con = new SQLiteConnection($"Data Source = {this._databasePATH}"))
            {
                con.Open();

                using (var command = new SQLiteCommand(query, con))
                {
                    foreach (var pair in args)
                    {
                        command.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    numberOfRowsAffected = command.ExecuteNonQuery();
                }
            }

            return numberOfRowsAffected;
        }

        private DataTable ExecuteRead(string query, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(query.Trim()))
                return null;

            using (var con = new SQLiteConnection($"Data Source = {this._databasePATH}"))
            {
                con.Open();

                using (var command = new SQLiteCommand(query, con))
                {
                    foreach (var pair in args)
                    {
                        command.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    var dataAdapter = new SQLiteDataAdapter(command);
                    var dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);
                    dataAdapter.Dispose();

                    return dataTable;
                }
            }
        }

        public int Create (Product product)
        {
            string query = "INSERT INTO ProductTest (code, description, quantity, price) " +
                "VALUES (@code, @description, @quantity, @price)";

            var args = new Dictionary<string, object>()
            {
                { "@code", product.Code },
                { "@description", product.Description },
                { "@quantity", product.Quantity },
                { "@price", product.Price },
            };

            return ExecuteWrite(query, args);
        }

        public int Delete (Product product)
        {
            string query = "DELETE FROM ProductTest WHERE idProduct = @id";

            var args = new Dictionary<string, object>()
            {
                { "@id", product.Id }
            };

            return ExecuteWrite(query, args);
        }
         
        public int Update (Product product, string description, int quantity, double price)
        {
            string query = "UPDATE productTest " +
                "SET description = @description, quantity = @quantity, price = @price " +
                "WHERE idProduct = @idProduct;";

            var args = new Dictionary<string, object>()
            {
                { "@idProduct", product.Id },
                { "@description", description },
                { "@quantity", quantity },
                { "@price", price },
            };

            return ExecuteWrite(query, args);
        }

        public DataTable GetProducts()
        {
            string query = "SELECT * FROM ProductTest";

            using (var con = new SQLiteConnection($"Data Source = {this._databasePATH}"))
            {
                con.Open();

                using (var command = new SQLiteCommand(query, con))
                {
                    var dataAdapter = new SQLiteDataAdapter(command);
                    var dataTable = new DataTable();

                    dataAdapter.Fill(dataTable);
                    dataAdapter.Dispose();

                    return dataTable;
                }
            }
        }

        public Product GetProductByCode(string code)
        {
            string query = "SELECT * FROM ProductTest WHERE code = @code";

            var args = new Dictionary<string, object>()
            {
                { "@code", code }
            };

            DataTable dataTable = ExecuteRead(query, args);

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                return null;
            }

            Product product = new Product
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["idProduct"]),
                Code = Convert.ToString(dataTable.Rows[0]["code"]),
                Description = Convert.ToString(dataTable.Rows[0]["description"]),
                Quantity = Convert.ToInt32(dataTable.Rows[0]["quantity"]),
                Price = Convert.ToDouble(dataTable.Rows[0]["price"])
            };

            return product;
        }
    }
}
