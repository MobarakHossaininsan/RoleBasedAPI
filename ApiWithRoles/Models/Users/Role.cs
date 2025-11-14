using System.ComponentModel.DataAnnotations;

namespace ApiWithRoles.Models.Users;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string RoleType { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
}