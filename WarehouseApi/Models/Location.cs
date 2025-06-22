namespace WarehouseApi.Models;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
}