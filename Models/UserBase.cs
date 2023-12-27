using System.ComponentModel.DataAnnotations;

public class UserBase
{
    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])[a-zA-Z\d_]{8,40}$")]
    public string? Password { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Range(typeof(DateTime), "1900-01-01T00:00:00.000Z", "2030-01-01T00:00:00.000Z")]
    public DateTime BirthDate { get; set; }

    //picture
}