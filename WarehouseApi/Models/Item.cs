namespace WarehouseApi.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public int Quantity { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}