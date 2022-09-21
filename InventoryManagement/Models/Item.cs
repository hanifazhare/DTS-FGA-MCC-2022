using System;

namespace InventoryManagement.Models.Item {
    class Item {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public double itemQuantity { get; set; }
        public string itemNote { get; set; }
    }
}