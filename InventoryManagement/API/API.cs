using System;
using Microsoft.Data.SqlClient;
using InventoryManagement.Models.Item;

namespace InventoryManagement.API {
    public class Api {
        SqlConnection sqlConnection;
        string connectionString = "Data Source=DESKTOP-59AKV1V;Initial Catalog=DTS_MCC_4;" + 
            "User ID=admin;Password=12345;TrustServerCertificate=True;Connect Timeout=30;";

        //Get Login by username or email
        public bool getLoginByUsernameEmail(string usernameEmail, string password) {
            bool status = false;
            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spLogin " +
                    "@username = @inputUsername, " +
                    "@email = @inputEmail, " +
                    "@passwordPlainText = @inputPassword, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";

            SqlParameter usernameParam = new SqlParameter();
            usernameParam.ParameterName = "@inputUsername";
            usernameParam.Value = usernameEmail;

            SqlParameter emailParam = new SqlParameter();
            emailParam.ParameterName = "@inputEmail";
            emailParam.Value = usernameEmail;

            SqlParameter passwordParam = new SqlParameter();
            passwordParam.ParameterName = "@inputPassword";
            passwordParam.Value = password;

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(usernameParam);
            sqlCommand.Parameters.Add(emailParam);
            sqlCommand.Parameters.Add(passwordParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            if (sqlDataReader[0].ToString() == "User successfully logged in") {
                                status = true;
                            } else {
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No Data Rows");
                        status = false;
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return status;
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