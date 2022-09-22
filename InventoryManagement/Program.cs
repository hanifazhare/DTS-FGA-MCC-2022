using System;
using System.Collections.Generic;
using InventoryManagement.Models.Item;
using InventoryManagement.Models.User;
using InventoryManagement.Models.Transaction;
using InventoryManagement.Utils.TablePrinter;
using InventoryManagement.API;

namespace InventoryManagement {
    class Program {
        static void Main() {
            int userChoice;
            int userRoleId = 1;
            string inputFirstName, inputLastName, inputUsername, inputEmail, inputUsernameEmail, inputPhoneNumber, inputPassword, inputNewPassword;

            var inventoryItemList = new List<Item>();
            var itemData = new Item();
            var userData = new User();
            var api = new Api();

            do {
                WelcomeMenu:
                helperMenu();
                Console.WriteLine("Welcome to Inventory Management Program");
                Console.WriteLine();
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Change Password");
                Console.WriteLine("4. Forgot Password");
                helperMenu(false, true, false);

                Console.Write("Your choice: ");
                userChoice = userChoiceIntValidation();

                switch (userChoice) {
                    case 1:
                        Console.WriteLine();
                        helperMenu();
                        Console.Write("Username/Email: ");
                        inputUsernameEmail = Console.ReadLine();
                        Console.Write("Password: ");
                        inputPassword = Console.ReadLine();

                        if (api.loginByUsernameEmail(inputUsernameEmail, inputPassword)) {
                            userData = api.getUserByUsernameEmail(inputUsernameEmail);
                            Console.WriteLine("Successfully logged in");
                            Console.WriteLine();

                            MainMenu:
                            helperMenu();
                            Console.Write("Hello, ");
                            Console.Write(userData.roleId == 1 ? userData.firstName : userData.firstName + " " + userData.lastName);
                            Console.WriteLine("! Welcome to Main Menu");
                            Console.WriteLine();
                            Console.WriteLine("1. Items List");
                            Console.WriteLine("2. Items Transaction");
                            Console.WriteLine("3. Add Item");
                            Console.WriteLine("---------------------------------------------");
                            Console.WriteLine("9. Detail Account");
                            helperMenu(false, false, true);
                            
                            Console.Write("Your choice: ");
                            userChoice = userChoiceIntValidation();
                            
                            switch (userChoice) {
                                case 1:
                                    ListItemMenu:
                                    Console.WriteLine();
                                    helperMenu();
                                    inventoryItemList = getInventoryItemList(api, inventoryItemList);
                                    helperMenu(false, false, false);
                                    
                                    Console.Write("Your choice: ");
                                    userChoice = userChoiceIntValidation();

                                    switch (userChoice) {
                                        case 0:
                                            Console.WriteLine();
                                            goto MainMenu;
                                        default:
                                            Console.WriteLine("Wrong choice, choose again!");
                                            goto ListItemMenu;
                                    }
                                case 2:
                                    TransactionMenu:
                                    Console.WriteLine();
                                    helperMenu();
                                    Console.WriteLine("1. Item Transaction List");

                                    if (userData.roleId == 1) {
                                        userRoleId = userData.roleId;
                                        Console.WriteLine("2. Item In");
                                        Console.WriteLine("3. Item Out");
                                    } else if (userData.roleId == 2) {
                                        userRoleId = userData.roleId;
                                        Console.WriteLine("2. Item In");
                                    } else {
                                        userRoleId = userData.roleId;
                                        Console.WriteLine("2. Item Out");
                                    }
                                    
                                    helperMenu(false, false, false);

                                    Console.Write("Your choice: ");
                                    userChoice = userChoiceIntValidation();

                                    switch (userChoice) {
                                        case 1:
                                            api.getAllTransaction();
                                            goto TransactionMenu;
                                        case 2:
                                            if (userRoleId == 3) {
                                                processTransaction(api, inventoryItemList, itemData, userData, false);
                                            } else {
                                                processTransaction(api, inventoryItemList, itemData, userData,true);
                                            }
                                            goto TransactionMenu;
                                        case 3:
                                            if (userRoleId == 1) {
                                                processTransaction(api, inventoryItemList, itemData, userData, false);
                                                goto TransactionMenu;
                                            } else {
                                                goto default;
                                            }
                                        case 0:
                                            Console.WriteLine();
                                            goto MainMenu;
                                        default:
                                            Console.WriteLine("Wrong choice, choose again!");
                                            goto TransactionMenu;
                                    }
                                case 3:
                                    Console.WriteLine();
                                    helperMenu();
                                    addInventoryItemList(api);
                                    AddItemMenu:
                                    Console.WriteLine();
                                    Console.WriteLine("1. Add Item Again");
                                    helperMenu(false, false, false);

                                    Console.Write("Your choice: ");
                                    userChoice = userChoiceIntValidation();

                                    switch (userChoice) {
                                        case 1:
                                            goto AddItemMenu;
                                        case 0:
                                            Console.WriteLine();
                                            goto MainMenu;
                                        default:
                                            Console.WriteLine("Wrong choice, choose again!");
                                            goto AddItemMenu;
                                    }
                                case 0:
                                    Console.WriteLine("Logging out and back to welcome menu...");
                                    Console.WriteLine();
                                    goto WelcomeMenu;
                                default:
                                    Console.WriteLine("Wrong choice, choose again!");
                                    Console.WriteLine();
                                    goto MainMenu;
                            }
                        } else {
                            LoginMenu:
                            Console.WriteLine();
                            helperMenu(false, false, false);

                            Console.Write("Your choice: ");
                            userChoice = userChoiceIntValidation();

                            switch (userChoice) {
                                case 0:
                                    Console.WriteLine();
                                    goto WelcomeMenu;
                                default:
                                    Console.WriteLine("Wrong choice, choose again!");
                                    goto LoginMenu;
                            }
                        }
                    case 2:
                        Console.WriteLine();
                        helperMenu();
                        Console.WriteLine("Register new account");
                        Console.WriteLine();
                        Console.Write("First Name: ");
                        inputFirstName = Console.ReadLine();
                        Console.Write("Last Name: ");
                        inputLastName = Console.ReadLine();
                        Console.Write("Username: ");
                        inputUsername = Console.ReadLine();
                        Console.Write("Email: ");
                        inputEmail = Console.ReadLine();
                        Console.Write("Phone Number: ");
                        inputPhoneNumber = Console.ReadLine();
                        Console.Write("Password: ");
                        inputPassword = Console.ReadLine();

                        api.registerUser(3, inputFirstName, inputLastName, inputUsername, inputEmail, inputPhoneNumber, inputPassword);

                        RegisterMenu:
                        Console.WriteLine();
                        helperMenu(false, false, false);

                        Console.Write("Your choice: ");
                        userChoice = userChoiceIntValidation();

                        switch (userChoice) {
                            case 0:
                                Console.WriteLine();
                                goto WelcomeMenu;
                            default:
                                Console.WriteLine("Wrong choice, choose again!");
                                goto RegisterMenu;
                        }
                    case 3:
                        Console.WriteLine();
                        helperMenu();
                        Console.WriteLine("Please input your username or email to change your password");
                        Console.WriteLine();
                        Console.Write("Username/Email: ");
                        inputUsernameEmail = Console.ReadLine();

                        Console.WriteLine("Checking account...");

                        if (api.checkUserByUsernameEmail(inputUsernameEmail)) {
                            Console.WriteLine();
                            Console.Write("Success! Now enter your old password: ");
                            inputPassword = Console.ReadLine();

                            Console.WriteLine("Matching account and password...");

                            if (api.loginByUsernameEmail(inputUsernameEmail, inputPassword)) {
                                Console.WriteLine();
                                Console.Write("Success! Now enter your new password: ");
                                inputNewPassword = Console.ReadLine();

                                api.changeUserPassword(inputUsernameEmail, inputPassword, inputNewPassword);
                            }
                        }

                        ChangePassMenu:
                        Console.WriteLine();
                        helperMenu(false, false, false);

                        Console.Write("Your choice: ");
                        userChoice = userChoiceIntValidation();

                        switch (userChoice) {
                            case 0:
                                Console.WriteLine();
                                goto WelcomeMenu;
                            default:
                                Console.WriteLine("Wrong choice, choose again!");
                                goto ChangePassMenu;
                        }
                    case 4:
                        ForgotPassMenu:
                        Console.WriteLine();
                        helperMenu();
                        Console.WriteLine("Please contact Administrator if you forgot your password");
                        helperMenu(false, false, false);

                        Console.Write("Your choice: ");
                        userChoice = userChoiceIntValidation();

                        switch (userChoice) {
                            case 0:
                                Console.WriteLine();
                                goto WelcomeMenu;
                            default:
                                Console.WriteLine("Wrong choice, choose again!");
                                goto ForgotPassMenu;
                        }
                    case 0:
                        Console.WriteLine("Closing program...");
                        break;
                    default:
                        Console.WriteLine("Wrong choice, choose again!");
                        Console.WriteLine();
                        break;                        
                }
            } while (userChoice != 0);
        }
        
        static void helperMenu(bool isTopMenu = true, bool isBottomWelcomeMenu = true, bool isBottomMainMenu = true) {
            if (isTopMenu) {
                Console.WriteLine("=============================================");
                Console.WriteLine("=========== Inventory Management ============");
                Console.WriteLine("=============================================");
            } else {
                Console.WriteLine("---------------------------------------------");
                if (isBottomWelcomeMenu) {
                    Console.WriteLine("0. Exit");
                } else if (isBottomMainMenu) {
                    Console.WriteLine("0. Logout");
                } else {
                    Console.WriteLine("0. Back to Previous Menu");
                }
                Console.WriteLine("=============================================");
            }
        }
        
        static int userChoiceIntValidation() {
            int userChoice;
            string inputUserChoice;
            
            inputUserChoice = Console.ReadLine();
            
            while (!int.TryParse(inputUserChoice, out userChoice)) {
                Console.WriteLine("Error, must input number!");
                Console.WriteLine();

                Console.Write("Please enter your input again: ");
                inputUserChoice = Console.ReadLine();
            }
            
            return userChoice;
        }

        static void addInventoryItemList(Api api) {
            string itemCode;
            string itemName;
            int itemQuantity;
            string itemNote;
            
            Console.WriteLine();
            Console.WriteLine("Adding item...");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Item Code: ");
            itemCode = Console.ReadLine();
            Console.Write("Item Name: ");
            itemName = Console.ReadLine();
            Console.Write("Item Quantity: ");
            itemQuantity = userChoiceIntValidation();
            Console.Write("Item Note: ");
            itemNote = Console.ReadLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Done!");

            var itemData = new Item() {
                code = itemCode,
                name = itemName,
                availableQuantity = itemQuantity,
                note = itemNote
            };

            api.insertItem(itemData);
        }

        static List<Item> getInventoryItemList(Api api, List<Item> itemList) {
            itemList = api.getAllItem();

            Console.WriteLine();
            Console.WriteLine("Loading items list...");
            if (itemList.Count == 0) {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Nothing, please add item in main menu first!");
                Console.WriteLine("---------------------------------------------");
            } else {
                var printAllItem = new TablePrinter("ID", "Code", "Name", "Available Quantity", "Note");
                foreach (var item in itemList) {
                    printAllItem.AddRow(item.id, item.code, item.name, $"{item.availableQuantity:0.00}", item.note);
                }
                printAllItem.Print();
            }
            Console.WriteLine("Done!");
            Console.WriteLine();

            return itemList;
        }

        static void processTransaction(Api api, List<Item> itemList, Item itemData, User userData, bool isItemIn) {
            string userChoiceItemCode, userChoiceTransactionNote;
            int userChoiceItemQuantity;
            bool isCheckingItemsList = true, isAvailableQuantityNegative = false;
            var printItemData = new TablePrinter("ID", "Code", "Name", "Available Quantity", "Note");
            var printNewItemData = new TablePrinter("ID", "Code", "Name", "Available Quantity", "Note");

            Console.WriteLine();
            helperMenu();
            itemList = getInventoryItemList(api, itemList);

            if (itemList.Count == 0) {
                Console.WriteLine("Error, can't continue transaction!");
                Console.WriteLine("Please add item in main menu first!");
            } else {
                while (isCheckingItemsList) {
                    Console.Write("Please choose item code: ");
                    userChoiceItemCode = Console.ReadLine();

                    Console.WriteLine("Checking from items list...");
                    
                    foreach (var item in itemList) {
                        if (userChoiceItemCode == item.code) {
                            Console.WriteLine();
                            Console.WriteLine("Item detail:");
                            printItemData.AddRow(item.id, item.code, item.name, $"{item.availableQuantity:0.00}", item.note);
                            printItemData.Print();

                            Console.Write("Please input item quantity: ");
                            userChoiceItemQuantity = userChoiceIntValidation();
                            Console.Write("Please input transaction note: ");
                            userChoiceTransactionNote = Console.ReadLine();

                            if (isItemIn) {
                                api.insertItemTransaction(1, item.id, userData.id, userChoiceItemQuantity, userChoiceTransactionNote);
                            } else {
                                if (!api.insertItemTransaction(2, item.id, userData.id, userChoiceItemQuantity, userChoiceTransactionNote)) {
                                    isAvailableQuantityNegative = true;
                                }
                            }
                            
                            if (!isAvailableQuantityNegative) {
                                Console.WriteLine();
                                Console.WriteLine("Item Detail:");
                                itemData = api.getItemByItemCode(userChoiceItemCode);
                                printNewItemData.AddRow(itemData.id, itemData.code, itemData.name, $"{itemData.availableQuantity:0.00}", itemData.note);
                                printNewItemData.Print();
                                Console.WriteLine("Done updating item data!");
                            }

                            isCheckingItemsList = false;
                            isAvailableQuantityNegative = false;
                        }
                    }

                    if (isCheckingItemsList) {
                        Console.WriteLine("Nothing match, please choose again!");
                        Console.WriteLine();
                    }
                }
            }
        }

        static void getTransactionList(Api api, List<Transaction> transactionList) {
            // transactionList = api.getAllTransaction();

            Console.WriteLine();
            Console.WriteLine("Loading transactions list...");
            if (transactionList.Count == 0) {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Nothing, please do transaction first!");
                Console.WriteLine("---------------------------------------------");
            } else {
                var printAllTransaction = new TablePrinter("ID", "Code", "Name", "Available Quantity", "Note");
                // foreach (var transaction in transactionList) {
                    // printAllTransaction.AddRow(transaction.id, transaction.code, transaction.name, $"{transaction:0.00}", transaction.note);
                // }
                printAllTransaction.Print();
            }
            Console.WriteLine("Done!");
            Console.WriteLine();
        }
    }
}