namespace MunchWebAPI
{
  public partial class User
  {
    public int UserId { get; set; } = 0;
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Password { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }

  }
}
