using System.ComponentModel.DataAnnotations;

public record RegistrationModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z\d_]{4,20}$")]
    public string? Username { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])[a-zA-Z\d_]{8,40}$")]
    public string? Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }

    [Required]
    [EmailAddress]
    public string? EMail { get; set; }

    // [Required]
    [Range(typeof(DateTime), "1900-01-01T00:00:00.000Z", "2030-01-01T00:00:00.000Z")]
    public DateTime BirthDate { get; set; }
}
