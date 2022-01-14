namespace Mango.Services.Email.Models;

public class EmailLog
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Log { get; set; }
    public DateTime EmailSent { get; set; }
}