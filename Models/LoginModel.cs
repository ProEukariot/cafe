using System.ComponentModel.DataAnnotations;

public record LoginModel
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}