using System.ComponentModel.DataAnnotations;

namespace ApiWithRoles.DTOs;

public class Verify
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required] 
    public string Code { get; set; } = string.Empty;
}