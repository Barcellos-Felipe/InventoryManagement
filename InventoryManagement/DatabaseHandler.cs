using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace InventoryManagement
{
    /// <summary>
    /// The DatabaseHandler class contains all the CRUD methods.
    /// </summary>
    public class DatabaseHandler
    {
        private string _databasePATH = @"..\..\data\database.db";

        /// <summary>
        /// Handle all write methods within the database (Create, Update and Delete).
        /// </summary>
        /// <param name="query">The sql query that will be executed whithin the database.</param>
        /// <param name="args">The arguments that will be passed to the query.</param>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
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

        /// <summary>
        /// Handle all read methods within the database (GetProducts, GetProductByCode).
        /// </summary>
        /// <param name="query">The sql query that will be executed whithin the database.</param>
        /// <param name="args">The arguments that will be passed to the query.</param>
        /// <returns>
        /// The data table retrieved by the query.
        /// </returns>
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

        /// <summary>
        /// Creates a new product inside the database.
        /// </summary>
        /// <param name="product">A product object.</param>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
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

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="product">A product object.</param>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
        public int Delete (Product product)
        {
            string query = "DELETE FROM ProductTest WHERE idProduct = @id";

            var args = new Dictionary<string, object>()
            {
                { "@id", product.Id }
            };

            return ExecuteWrite(query, args);
        }

        /// <summary>
        /// Updates a product from the database.
        /// </summary>
        /// <param name="product">A product object.</param>
        /// <param name="description">The new description</param>
        /// <param name="quantity">The new quantity</param>
        /// <param name="price">The new price</param>
        /// <returns>
        /// The number of rows affected by the query.
        /// </returns>
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

        /// <summary>
        /// Retrieves all products from the database
        /// </summary>
        /// <returns>
        /// A data table with all products from the database.
        /// </returns>
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

        /// <summary>
        /// Retrieves a product from the database.
        /// </summary>
        /// <param name="code">The product's code</param>
        /// <returns>
        /// The product object retrieved by the query.
        /// </returns>
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
