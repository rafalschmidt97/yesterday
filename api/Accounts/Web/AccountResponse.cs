using Api.Accounts.Profiles;

namespace Api.Accounts.Web
{
  public class AccountResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public AccountProfile Profile { get; set; }
    public int Following { get; set; }
    public int Followers { get; set; }
  }
  
  public class AccountProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}