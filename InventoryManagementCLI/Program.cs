using System;
using System.Collections.Generic;
using InventoryManagementCLI.Models.Item;

namespace InventoryManagementCLI {
    class Program {
        static void Main() {
            int userChoice;
            var inventoryItemList = new List<Item>();
            
            do {
                MainMenu:
                helperMenu();
                Console.WriteLine("1. Items List");
                Console.WriteLine("2. Items Transaction");
                Console.WriteLine("3. Add Item");
                helperMenu(false);
                
                Console.Write("Your choice: ");
                userChoice = userChoiceIntValidation();
                
                switch (userChoice) {
                    case 1:
                        ListItemMenu:
                        Console.WriteLine();
                        helperMenu();
                        getInventoryItemList(inventoryItemList);
                        helperMenu(false, false);
                        
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
                        Console.WriteLine("1. Item In");
                        Console.WriteLine("2. Item Out");
                        helperMenu(false, false);

                        Console.Write("Your choice: ");
                        userChoice = userChoiceIntValidation();

                        switch (userChoice) {
                            case 1:
                                processTransaction(true, inventoryItemList);
                                goto TransactionMenu;
                            case 2:
                                processTransaction(false, inventoryItemList);
                                goto TransactionMenu;
                            case 0:
                                Console.WriteLine();
                                goto MainMenu;
                            default:
                                Console.WriteLine("Wrong choice, choose again!");
                                goto TransactionMenu;
                        }
                    case 3:
                        AddItemMenu:
                        Console.WriteLine();
                        helperMenu();
                        inventoryItemList = addInventoryItemList(inventoryItemList);
                        Console.WriteLine("1. Add Item Again");
                        helperMenu(false, false);

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
                        Console.WriteLine("Closing program...");
                        break;
                    default:
                        Console.WriteLine("Wrong choice, choose again!");
                        Console.WriteLine();
                        break;
                }
            } while (userChoice != 0);
        }
        
        static void helperMenu(bool isTopMenu = true, bool isBottomMainMenu = true) {
            if (isTopMenu) {
                Console.WriteLine("=============================================");
                Console.WriteLine("=========== Inventory Management ============");
                Console.WriteLine("=============================================");
            } else {
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine(isBottomMainMenu ? "0. Exit" : "0. Back to Previous Menu");
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
                    item.printItemsData();
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
                            item.printItemsData();

                            Console.Write("Please input item quantity: ");
                            userChoiceItemQuantity = userChoiceIntValidation();

                            if (isItemIn) {
                                item.itemQuantity += userChoiceItemQuantity;
                            } else {
                                item.itemQuantity -= userChoiceItemQuantity;
                            }
                            
                            Console.WriteLine();
                            Console.WriteLine("Item Detail:");
                            item.printItemsData();
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