namespace WarehouseApi.Models;

public class StockMovement
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int ValueBefore { get; set; }
    public int ValueAfter { get; set; }
    public string Reason { get; set; }
    public DateTime MovedAt { get; set; } = DateTime.UtcNow;
    public Item Item { get; set; } = null!;
}