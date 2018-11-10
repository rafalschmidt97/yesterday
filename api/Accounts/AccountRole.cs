using Api.Core.Security.Roles;

namespace Api.Accounts
{
  public class AccountRole
  {
    public int AccountId { get; set; }
    public int RoleId { get; set; }

    public Account Account { get; set; }
    public Role Role { get; set; }
  }
}