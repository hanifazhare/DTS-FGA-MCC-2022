using System;

namespace InventoryManagementAPI.Models.Item {
    class Item {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public int itemQuantity { get; set; }
        public string itemNote { get; set; }

        public void printItemsData() {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Item Code: " + itemCode);
            Console.WriteLine("Item Name: " + itemName);
            Console.WriteLine("Item Quantity: " + itemQuantity);
            Console.WriteLine("Item Note: " + itemNote);
            Console.WriteLine("---------------------------------------------");
        }
    }
}