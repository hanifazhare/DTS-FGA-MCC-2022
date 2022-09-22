namespace InventoryManagement.Models.Transaction {
    class Transaction {
        public int id { get; set; }
        public int transactionTypeId { get; set; }
        public int itemId { get; set; }
        public int userId { get; set; }
        public string code { get; set; }
        public string quantity { get; set; }
        public string note { get; set; }
    }
}