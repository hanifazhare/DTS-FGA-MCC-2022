using System;
using System.Collections.Generic;
using InventoryManagement.Models.Item;
using InventoryManagement.API;

namespace InventoryManagement {
    class Program {
        static void Main() {
            int userChoice;
            string inputFirstName, inputLastName, inputUsername, inputEmail, inputUsernameEmail, inputPhoneNumber, inputPassword, inputNewPassword;

            var inventoryItemList = new List<Item>();
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
                            Console.WriteLine("Oke main");
                            // MainMenu:
                            // helperMenu();
                            // Console.WriteLine("1. Items List");
                            // Console.WriteLine("2. Items Transaction");
                            // Console.WriteLine("3. Add Item");
                            // helperMenu(false, false, true);
                            
                            // Console.Write("Your choice: ");
                            // userChoice = userChoiceIntValidation();
                            
                            // switch (userChoice) {
                            //     case 1:
                            //         ListItemMenu:
                            //         Console.WriteLine();
                            //         helperMenu();
                            //         getInventoryItemList(inventoryItemList);
                            //         helperMenu(false, false, false);
                                    
                            //         Console.Write("Your choice: ");
                            //         userChoice = userChoiceIntValidation();

                            //         switch (userChoice) {
                            //             case 0:
                            //                 Console.WriteLine();
                            //                 goto MainMenu;
                            //             default:
                            //                 Console.WriteLine("Wrong choice, choose again!");
                            //                 goto ListItemMenu;
                            //         }
                            //     case 2:
                            //         TransactionMenu:
                            //         Console.WriteLine();
                            //         helperMenu();
                            //         Console.WriteLine("1. Item In");
                            //         Console.WriteLine("2. Item Out");
                            //         helperMenu(false, false, false);

                            //         Console.Write("Your choice: ");
                            //         userChoice = userChoiceIntValidation();

                            //         switch (userChoice) {
                            //             case 1:
                            //                 processTransaction(true, inventoryItemList);
                            //                 goto TransactionMenu;
                            //             case 2:
                            //                 processTransaction(false, inventoryItemList);
                            //                 goto TransactionMenu;
                            //             case 0:
                            //                 Console.WriteLine();
                            //                 goto MainMenu;
                            //             default:
                            //                 Console.WriteLine("Wrong choice, choose again!");
                            //                 goto TransactionMenu;
                            //         }
                            //     case 3:
                            //         AddItemMenu:
                            //         Console.WriteLine();
                            //         helperMenu();
                            //         inventoryItemList = addInventoryItemList(inventoryItemList);
                            //         Console.WriteLine("1. Add Item Again");
                            //         helperMenu(false, false, false);

                            //         Console.Write("Your choice: ");
                            //         userChoice = userChoiceIntValidation();

                            //         switch (userChoice) {
                            //             case 1:
                            //                 goto AddItemMenu;
                            //             case 0:
                            //                 Console.WriteLine();
                            //                 goto MainMenu;
                            //             default:
                            //                 Console.WriteLine("Wrong choice, choose again!");
                            //                 goto AddItemMenu;
                            //         }
                            //     case 0:
                            //         Console.WriteLine("Closing program...");
                            //         break;
                            //     default:
                            //         Console.WriteLine("Wrong choice, choose again!");
                            //         Console.WriteLine();
                            //         break;
                            // }
                        } 
                        // else {
                        //     LoginMenu:
                        //     Console.WriteLine();
                        //     helperMenu(false, false, false);

                        //     Console.Write("Your choice: ");
                        //     userChoice = userChoiceIntValidation();

                        //     switch (userChoice) {
                        //         case 0:
                        //             Console.WriteLine();
                        //             goto WelcomeMenu;
                        //         default:
                        //             Console.WriteLine("Wrong choice, choose again!");
                        //             goto LoginMenu;
                        //     }
                        // }
                        break;
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
                        Console.WriteLine("Please contact Administrator if you forgot your password!");
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

        static List<Item> addInventoryItemList(List<Item> itemList) {
            string _itemCode;
            string _itemName;
            int _itemQuantity;
            string _itemNote;
            
            Console.WriteLine();
            Console.WriteLine("Adding Item...");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Item Code: ");
            _itemCode = Console.ReadLine();
            Console.Write("Item Name: ");
            _itemName = Console.ReadLine();
            Console.Write("Item Quantity: ");
            _itemQuantity = userChoiceIntValidation();
            Console.Write("Item Note: ");
            _itemNote = Console.ReadLine();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Done!");
            Console.WriteLine();

            itemList.Add(
                new Item {
                    itemCode = _itemCode,
                    itemName = _itemName,
                    itemQuantity = _itemQuantity,
                    itemNote = _itemNote
                }
            );

            return itemList;
        }

        static void getInventoryItemList(List<Item> itemList) {
            Console.WriteLine();
            Console.WriteLine("---------------- List Items -----------------");
            Console.WriteLine();
            if (itemList.Count == 0) {
                Console.WriteLine("Nothing, please add item in main menu first!");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------");
            } else {
                foreach (var item in itemList) {
                    // item.printItemsData();
                    Console.WriteLine();
                }
            }
        }

        static void processTransaction(bool isItemIn, List<Item> itemList) {
            string userChoiceItemCode;
            int userChoiceItemQuantity;
            bool isCheckingItemsList = true;

            Console.WriteLine();
            helperMenu();
            getInventoryItemList(itemList);

            if (itemList.Count == 0) {
                Console.WriteLine();
                Console.WriteLine("Error, can't continue transaction!");
                Console.WriteLine("Please add item in main menu first!");
            } else {
                while (isCheckingItemsList) {
                    Console.WriteLine();
                    Console.Write("Please choose item code: ");
                    userChoiceItemCode = Console.ReadLine();
                    
                    foreach (var item in itemList) {
                        if (userChoiceItemCode == item.itemCode) {
                            Console.WriteLine();
                            Console.WriteLine("Item Detail:");
                            // item.printItemsData();

                            Console.Write("Please input item quantity: ");
                            userChoiceItemQuantity = userChoiceIntValidation();

                            if (isItemIn) {
                                item.itemQuantity += userChoiceItemQuantity;
                            } else {
                                item.itemQuantity -= userChoiceItemQuantity;
                            }
                            
                            Console.WriteLine();
                            Console.WriteLine("Item Detail:");
                            // item.printItemsData();
                            Console.WriteLine();
                            Console.WriteLine("Done updating data!");
                            isCheckingItemsList = false;
                        }
                    }

                    if (isCheckingItemsList) {
                        Console.WriteLine("Nothing match, please choose again!");
                    }
                }
            }
        }
    }
}