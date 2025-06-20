namespace WarehouseApi.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; // e.g. "Aisle 1"
    public string? Description { get; set; } // optional details
    public ICollection<Item> Items { get; set; } = new List<Item>();
}