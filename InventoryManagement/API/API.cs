using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using InventoryManagement.Models.Item;
using InventoryManagement.Models.User;
using InventoryManagement.Models.Transaction;

namespace InventoryManagement.API {
    class Api {
        SqlConnection sqlConnection;
        string connectionString = "Data Source = DESKTOP-59AKV1V; Initial Catalog = DTS_MCC_4;" +
            "Integrated Security = True; TrustServerCertificate = True; Connect Timeout = 30;";
        
        //All User Param
        public (SqlParameter roleIdParam, SqlParameter firstNameParam, SqlParameter lastNameParam, SqlParameter usernameParam, SqlParameter emailParam, SqlParameter phoneNumberParam, SqlParameter passwordParam, SqlParameter newPasswordParam) allUserParam(
                int roleId, string firstName, string lastName, string username, string email, string phoneNumber, string password, string newPassword) {
            SqlParameter roleIdParam = new SqlParameter();
            roleIdParam.ParameterName = "@inputRoleId";
            roleIdParam.Value = roleId;

            SqlParameter firstNameParam = new SqlParameter();
            firstNameParam.ParameterName = "@inputFirstName";
            firstNameParam.Value = firstName;

            SqlParameter lastNameParam = new SqlParameter();
            lastNameParam.ParameterName = "@inputLastName";
            lastNameParam.Value = lastName;

            SqlParameter usernameParam = new SqlParameter();
            usernameParam.ParameterName = "@inputUsername";
            usernameParam.Value = username;

            SqlParameter emailParam = new SqlParameter();
            emailParam.ParameterName = "@inputEmail";
            emailParam.Value = email;

            SqlParameter phoneNumberParam = new SqlParameter();
            phoneNumberParam.ParameterName = "@inputPhoneNumber";
            phoneNumberParam.Value = phoneNumber;

            SqlParameter passwordParam = new SqlParameter();
            passwordParam.ParameterName = "@inputPassword";
            passwordParam.Value = password;

            SqlParameter newPasswordParam = new SqlParameter();
            newPasswordParam.ParameterName = "@inputNewPassword";
            newPasswordParam.Value = newPassword;

            return(roleIdParam, firstNameParam, lastNameParam, usernameParam, emailParam, phoneNumberParam, passwordParam, newPasswordParam);
        }

        //All Item Param
        public (SqlParameter itemCodeParam, SqlParameter itemNameParam, SqlParameter itemQuantityParam, SqlParameter itemNoteParam) allItemParam(
                string itemCode, string itemName, double itemQuantity, string itemNote) {
            SqlParameter itemCodeParam = new SqlParameter();
            itemCodeParam.ParameterName = "@inputItemCode";
            itemCodeParam.Value = itemCode;

            SqlParameter itemNameParam = new SqlParameter();
            itemNameParam.ParameterName = "@inputItemName";
            itemNameParam.Value = itemName;

            SqlParameter itemQuantityParam = new SqlParameter();
            itemQuantityParam.ParameterName = "@inputItemQuantity";
            itemQuantityParam.Value = itemQuantity;

            SqlParameter itemNoteParam = new SqlParameter();
            itemNoteParam.ParameterName = "@inputItemNote";
            itemNoteParam.Value = itemNote;

            return(itemCodeParam, itemNameParam, itemQuantityParam, itemNoteParam);
        }

        //All Transaction Param
        public (SqlParameter transactionTypeIdParam, SqlParameter itemIdParam, SqlParameter userIdParam, SqlParameter transactionQuantityParam, SqlParameter transactionNoteParam) allTransactionParam(
                int transactionTypeId, int itemId, int userId, double transactionQuantity, string transactionNote) {
            SqlParameter transactionTypeIdParam = new SqlParameter();
            transactionTypeIdParam.ParameterName = "@inputTransactionTypeId";
            transactionTypeIdParam.Value = transactionTypeId;

            SqlParameter itemIdParam = new SqlParameter();
            itemIdParam.ParameterName = "@inputItemId";
            itemIdParam.Value = itemId;

            SqlParameter userIdParam = new SqlParameter();
            userIdParam.ParameterName = "@inputUserId";
            userIdParam.Value = userId;

            SqlParameter transactionQuantityParam = new SqlParameter();
            transactionQuantityParam.ParameterName = "@inputTransactionQuantity";
            transactionQuantityParam.Value = transactionQuantity;

            SqlParameter transactionNoteParam = new SqlParameter();
            transactionNoteParam.ParameterName = "@inputTransactionNote";
            transactionNoteParam.Value = transactionNote;

            return(transactionTypeIdParam, itemIdParam, userIdParam, transactionQuantityParam, transactionNoteParam);
        }

        //Select User by Username or Email
        public User getUserByUsernameEmail(string usernameEmail) {
            var userData = new User();
            var allUserParamList = allUserParam(0, "", "", usernameEmail, usernameEmail, "", "", "");

            string query = "SELECT * FROM Users WHERE username = @inputUsername OR email = @inputEmail";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(allUserParamList.usernameParam);
            sqlCommand.Parameters.Add(allUserParamList.emailParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            userData = new User() {
                                id = Convert.ToInt32(sqlDataReader[0]),
                                roleId = Convert.ToInt32(sqlDataReader[1]),
                                firstName = sqlDataReader[2].ToString(),
                                lastName = sqlDataReader[3].ToString(),
                                username = sqlDataReader[4].ToString(),
                                email = sqlDataReader[5].ToString(),
                                phoneNumber = sqlDataReader[6].ToString()
                            };
                        }
                    } else {
                        Console.WriteLine("No data row");
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return userData;
        }
        
        //Register User
        public bool registerUser(int roleId, string firstName, string lastName, string username,
                string email, string phoneNumber, string password) {
            bool status = false;
            var allUserParamList = allUserParam(roleId, firstName, lastName, username, email, phoneNumber, password, "");

            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spRegister " +
                    "@roleId = @inputRoleId, " +
                    "@firstName = @inputFirstName, " +
                    "@lastName = @inputLastName, " +
                    "@username = @inputUsername, " +
                    "@email = @inputEmail, " +
                    "@phoneNumber = @inputPhoneNumber, " +
                    "@passwordPlainText = @inputPassword, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";
            
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(allUserParamList.roleIdParam);
            sqlCommand.Parameters.Add(allUserParamList.firstNameParam);
            sqlCommand.Parameters.Add(allUserParamList.lastNameParam);
            sqlCommand.Parameters.Add(allUserParamList.usernameParam);
            sqlCommand.Parameters.Add(allUserParamList.emailParam);
            sqlCommand.Parameters.Add(allUserParamList.phoneNumberParam);
            sqlCommand.Parameters.Add(allUserParamList.passwordParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            if (sqlDataReader[0].ToString() == "Successfully register") {
                                status = true;
                            } else {
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No data rows");
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

        //Login by Username or Email
        public bool loginByUsernameEmail(string usernameEmail, string password) {
            bool status = false;
            var allUserParamList = allUserParam(0, "", "", usernameEmail, usernameEmail, "", password, "");

            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spLogin " +
                    "@username = @inputUsername, " +
                    "@email = @inputEmail, " +
                    "@passwordPlainText = @inputPassword, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";
            
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(allUserParamList.usernameParam);
            sqlCommand.Parameters.Add(allUserParamList.emailParam);
            sqlCommand.Parameters.Add(allUserParamList.passwordParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            if (sqlDataReader[0].ToString() == "Successfully logged in") {
                                status = true;
                            } else {
                                Console.WriteLine(sqlDataReader[0]);
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No data rows");
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

        //Check User by Username or Email
        public bool checkUserByUsernameEmail(string usernameEmail) {
            bool status = false;
            var allUserParamList = allUserParam(0, "", "", usernameEmail, usernameEmail, "", "", "");

            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spCheckUserByUsernameEmail " +
                    "@username = @inputUsername, " +
                    "@email = @inputEmail, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";
            
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            
            sqlCommand.Parameters.Add(allUserParamList.usernameParam);
            sqlCommand.Parameters.Add(allUserParamList.emailParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            if (sqlDataReader[0].ToString() == "Success") {
                                status = true;
                            } else {
                                Console.WriteLine(sqlDataReader[0]);
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No data rows");
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

        //Change User Password
        public bool changeUserPassword(string usernameEmail, string password, string newPassword) {
            bool status = false;
            var allUserParamList = allUserParam(0, "", "", usernameEmail, usernameEmail, "", password, newPassword);

            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spChangeUserPassword " +
                    "@username = @inputUsername, " +
                    "@email = @inputEmail, " +
                    "@passwordPlainText = @inputPassword, " +
                    "@newPasswordPlainText = @inputNewPassword, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";
            
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            
            sqlCommand.Parameters.Add(allUserParamList.usernameParam);
            sqlCommand.Parameters.Add(allUserParamList.emailParam);
            sqlCommand.Parameters.Add(allUserParamList.passwordParam);
            sqlCommand.Parameters.Add(allUserParamList.newPasswordParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            if (sqlDataReader[0].ToString() == "Successfully changed password") {
                                Console.WriteLine(sqlDataReader[0]);
                                status = true;
                            } else {
                                Console.WriteLine(sqlDataReader[0]);
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No data rows");
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

        //Select All Item Data
        public List<Item> getAllItem() {
            var itemList = new List<Item>();
            string query = "SELECT * FROM Items";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            itemList.Add(
                                new Item {
                                    id = Convert.ToInt32(sqlDataReader[0]),
                                    code = sqlDataReader[1].ToString(),
                                    name = sqlDataReader[2].ToString(),
                                    availableQuantity = Convert.ToDouble(sqlDataReader[3]),
                                    note = sqlDataReader[4].ToString()
                                }
                            );
                        }
                    } else {
                        Console.WriteLine("No data rows");
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return itemList;
        }

        //Select Item Data by Item Code
        public Item getItemByItemCode(string itemCode) {
            var itemData = new Item();
            var allItemParamList = allItemParam(itemCode, "", 0, "");

            string query = "SELECT * FROM Items WHERE code = @inputItemCode";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(allItemParamList.itemCodeParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            itemData = new Item() {
                                id = Convert.ToInt32(sqlDataReader[0]),
                                code = sqlDataReader[1].ToString(),
                                name = sqlDataReader[2].ToString(),
                                availableQuantity = Convert.ToDouble(sqlDataReader[3]),
                                note = sqlDataReader[4].ToString()
                            };
                        }
                    } else {
                        Console.WriteLine("No data rows");
                    }
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return itemData;
        }

        //Insert Item Data
        public bool insertItem(Item item) {
            bool status = false;

            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();
                var allItemParamList = allItemParam(item.code, item.name, item.availableQuantity, item.note);

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.Parameters.Add(allItemParamList.itemCodeParam);
                sqlCommand.Parameters.Add(allItemParamList.itemNameParam);
                sqlCommand.Parameters.Add(allItemParamList.itemQuantityParam);
                sqlCommand.Parameters.Add(allItemParamList.itemNoteParam);

                try {
                    sqlCommand.CommandText =
                        "INSERT INTO Items (code, name, available_quantity, notes) " +
                        "VALUES (@inputItemCode, @inputItemName, @inputItemQuantity, @inputItemNote)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();

                    status = true;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    status = false;
                }
            }

            return status;
        }

        //Update Item Data by Item Code
        public bool updateItemByItemCode(string itemCode, string itemName, int itemQuantity, string itemNote) {
            bool status = false;

            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();
                var allItemParamList = allItemParam(itemCode, itemName, itemQuantity, itemNote);

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.Parameters.Add(allItemParamList.itemCodeParam);
                sqlCommand.Parameters.Add(allItemParamList.itemNameParam);
                sqlCommand.Parameters.Add(allItemParamList.itemQuantityParam);
                sqlCommand.Parameters.Add(allItemParamList.itemNoteParam);

                try {
                    sqlCommand.CommandText =
                        "UPDATE Items " +
                        "SET name = @inputItemName, available_quantity = @inputItemQuantity, notes = @inputItemNote " +
                        "WHERE code = @inputItemCode";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();

                    status = true;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    status = false;
                }
            }

            return status;
        }

        //Delete Item Data by Item Code
        public bool deleteItemByItemCode(string itemCode) {
            bool status = false;

            using(SqlConnection sqlConnection = new SqlConnection(connectionString)) {
                sqlConnection.Open();
                var allItemParamList = allItemParam(itemCode, "", 0, "");

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.Parameters.Add(allItemParamList.itemCodeParam);

                try {
                    sqlCommand.CommandText =
                        "DELETE FROM Items " +
                        "WHERE code = @inputItemCode";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();

                    status = true;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    status = false;
                }
            }

            return status;
        }

        //Select All Transaction Data
        public bool getAllTransaction() {
            bool status = false;
            var transactionList = new List<Transaction>();
            string query = "SELECT * FROM TransactionDetails";

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
                                sqlDataReader[4] + " - " +
                                sqlDataReader[5] + " - " +
                                sqlDataReader[6] + " - " +
                                sqlDataReader[7]);
                        }
                        status = true;
                    } else {
                        Console.WriteLine("No data rows");
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

        //Insert Item Transaction
        public bool insertItemTransaction(int transactionTypeId, int itemId, int userId, double transactionQuantity, string transactionNote) {
            bool status = false;
            var allTransactionParamList = allTransactionParam(transactionTypeId, itemId, userId, transactionQuantity, transactionNote);

            string query =
                "DECLARE @responseMessage varchar(250) " +
                "EXEC spItemTransaction " +
                    "@transactionTypeId = @inputTransactionTypeId, " +
                    "@itemId = @inputItemId, " +
                    "@userId = @inputUserId, " +
                    "@quantity = @inputTransactionQuantity, " +
                    "@notes = @inputTransactionNote, " +
                    "@responseMessage = @responseMessage OUTPUT " +
                "SELECT	@responseMessage as responseMessage";
            
            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            sqlCommand.Parameters.Add(allTransactionParamList.transactionTypeIdParam);
            sqlCommand.Parameters.Add(allTransactionParamList.itemIdParam);
            sqlCommand.Parameters.Add(allTransactionParamList.userIdParam);
            sqlCommand.Parameters.Add(allTransactionParamList.transactionQuantityParam);
            sqlCommand.Parameters.Add(allTransactionParamList.transactionNoteParam);

            try {
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()) {
                    if (sqlDataReader.HasRows) {
                        while (sqlDataReader.Read()) {
                            Console.WriteLine(sqlDataReader[0].ToString());
                            if (sqlDataReader[0].ToString() == "Transaction success") {
                                status = true;
                            } else if(sqlDataReader[0].ToString() == "Transaction error! Available Quantity in table Items < 0") {
                                status = false;
                            } else {
                                status = false;
                            }
                        }
                    } else {
                        Console.WriteLine("No data rows");
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
    }
}