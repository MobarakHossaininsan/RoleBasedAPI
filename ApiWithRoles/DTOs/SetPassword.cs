using System.ComponentModel.DataAnnotations;

namespace ApiWithRoles.DTOs;

public class SetPassword
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}