namespace InventoryManagement.Models.Item {
    class Item {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public double availableQuantity { get; set; }
        public string note { get; set; }
    }
}