using System.ComponentModel.DataAnnotations;

namespace DevForABuck.API;

public class AuthOptions
{
    [Required]
    public string? TenantId { get; init; }

    [Required]
    public string? ClientId { get; init; }

    [Required]
    public string? Authority { get; init; }
}

