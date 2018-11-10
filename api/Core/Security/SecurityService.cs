using Api.Accounts;
using Api.Core.AutomaticDI;
using static BCrypt.Net.BCrypt;

namespace Api.Core.Security
{
  public class SecurityService : ISingleton
  {
    private readonly TokenUtilities tokenUtilities;
    private readonly AccountService accountService;

    public SecurityService(TokenUtilities tokenUtilities, AccountService accountService)
    {
      this.tokenUtilities = tokenUtilities;
      this.accountService = accountService;
    }

    public bool Authenticate(string username, string password)
    {
      var user = accountService.GetByUsername(username);
      return user != null && Verify(password, user.Password);
    }

    public string GenerateToken(string username)
    {
      var account = accountService.GetByUsernameWithRoles(username);
      return tokenUtilities.Generate(account);
    }
  }
}