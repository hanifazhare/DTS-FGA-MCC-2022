using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using InventoryManagementAPI.Models.Item;

namespace InventoryManagementAPI {
    class Program {
        SqlConnection sqlConnection;
        string connectionString = "Data Source=DESKTOP-59AKV1V;Initial Catalog=DTS_MCC_3;" + 
            "User ID=admin;Password=12345;TrustServerCertificate=True;Connect Timeout=30;";

        static void Main() {
            Program program = new Program();

            Item item = new Item() {
                itemCode = "PC_2",
                itemName = "PC",
                itemQuantity = 200,
                itemNote = "A PC"
            };

            //Select All
            Console.WriteLine("Select All in Table Items");
            Console.WriteLine("Done! Table Detail:");
            program.getAll();
            Console.WriteLine();

            ////Select by Id
            Console.WriteLine("Select by ID in Table Items || Where ID = 1");
            Console.WriteLine("Done! Table Detail:");
            program.getById(1);
            Console.WriteLine();

            //Insert
            Console.WriteLine("Insert in Table Items");
            program.insert(item);
            Console.WriteLine("Insert Done! Table Detail:");
            program.getAll();

            //Update
            Console.WriteLine("Update Data in Table Items || Where ID = 5");
            program.update(5, "PC", 500, "A Good PC");
            Console.WriteLine("Update Done! Table Detail:");
            program.getAll();

            //Delete
            Console.WriteLine("Delete Data in Table Items || Where ID = 5");
            program.delete(5);
            Console.WriteLine("Delete Done! Table Detail:");
            program.getAll();
        }

        //Select All
        void getAll() {
            string query = "SELECT * FROM Items";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            Console.WriteLine(
                                sqlDataReader[0] + " - " +
                                sqlDataReader[1] + " - " +
                                sqlDataReader[2] + " - " +
                                sqlDataReader[3] + " - " +
                                sqlDataReader[4]);
                        }
                    } else {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        //Select by Id
        void getById(int id) {
            string query = "SELECT * FROM Items WHERE id = @id";

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = id;

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add(sqlParameter);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                        }
                    } else {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        //Insert
        void insert(Item item) {
            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                //ItemCode
                SqlParameter itemCodeParam = new SqlParameter();
                itemCodeParam.ParameterName = "@code";
                itemCodeParam.Value = item.itemCode;

                //ItemName
                SqlParameter itemNameParam = new SqlParameter();
                itemNameParam.ParameterName = "@name";
                itemNameParam.Value = item.itemName;

                //ItemQuantity
                SqlParameter itemQuantityParam = new SqlParameter();
                itemQuantityParam.ParameterName = "@available_quantity";
                itemQuantityParam.Value = item.itemQuantity;

                //ItemNotes
                SqlParameter itemNoteParam = new SqlParameter();
                itemNoteParam.ParameterName = "@notes";
                itemNoteParam.Value = item.itemNote;

                sqlCommand.Parameters.Add(itemCodeParam);
                sqlCommand.Parameters.Add(itemNameParam);
                sqlCommand.Parameters.Add(itemQuantityParam);
                sqlCommand.Parameters.Add(itemNoteParam);

                try {
                    sqlCommand.CommandText =
                        "INSERT INTO Items (code, name, available_quantity, notes) " +
                        "VALUES (@code, @name, @available_quantity, @notes)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Update
        void update(int id, string itemName, int itemQuantity, string itemNote) {
            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                //ItemId
                SqlParameter itemIdParam = new SqlParameter();
                itemIdParam.ParameterName = "@id";
                itemIdParam.Value = id;

                //ItemName
                SqlParameter itemNameParam = new SqlParameter();
                itemNameParam.ParameterName = "@name";
                itemNameParam.Value = itemName;

                //ItemQuantity
                SqlParameter itemQuantityParam = new SqlParameter();
                itemQuantityParam.ParameterName = "@available_quantity";
                itemQuantityParam.Value = itemQuantity;

                //ItemNotes
                SqlParameter itemNoteParam = new SqlParameter();
                itemNoteParam.ParameterName = "@notes";
                itemNoteParam.Value = itemNote;

                sqlCommand.Parameters.Add(itemIdParam);
                sqlCommand.Parameters.Add(itemNameParam);
                sqlCommand.Parameters.Add(itemQuantityParam);
                sqlCommand.Parameters.Add(itemNoteParam);

                try {
                    sqlCommand.CommandText =
                        "UPDATE Items " +
                        "SET name = @name, available_quantity = @available_quantity, notes = @notes " +
                        "WHERE id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Delete
        void delete(int id) {
            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                //ItemId
                SqlParameter itemIdParam = new SqlParameter();
                itemIdParam.ParameterName = "@id";
                itemIdParam.Value = id;

                sqlCommand.Parameters.Add(itemIdParam);

                try {
                    sqlCommand.CommandText =
                        "DELETE FROM Items " +
                        "WHERE id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}