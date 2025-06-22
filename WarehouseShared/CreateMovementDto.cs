using System.ComponentModel.DataAnnotations;

namespace WarehouseShared;

public record CreateMovementDto(
    [Required(ErrorMessage = "New value is required")]
    int Change,
    [Required(ErrorMessage = "Reason is required.")]
    [StringLength(512, MinimumLength = 3,
        ErrorMessage = "Reason must be between 3 and 512 characters.")]
    string Reason
);