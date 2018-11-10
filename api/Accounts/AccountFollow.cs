namespace Api.Accounts
{
  public class AccountFollow
  {
    public int AccountId { get; set; }
    public int FollowingId { get; set; }

    public Account Account { get; set; }
    public Account Following { get; set; }
  }
}