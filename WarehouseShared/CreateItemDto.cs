using System.ComponentModel.DataAnnotations;

namespace WarehouseShared;

public record CreateItemDto(
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(256, MinimumLength = 2,
        ErrorMessage = "Name must be between 2 and 256 characters.")]
    string Name,
    [Required(ErrorMessage = "SKU is required.")]
    [StringLength(12, MinimumLength = 8,
        ErrorMessage = "SKU must be between 8 and 12 characters.")]
    string Sku,
    [Required(ErrorMessage = "Location is required")]
    int LocationId
);