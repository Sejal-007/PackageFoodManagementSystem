namespace PackageFoodManagementSystem.Repository.Models
{
    public class Inventory
    {
        public int Id { get; set; } // Primary Key
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public string WarehouseLocation { get; set; } = string.Empty;
    }
}