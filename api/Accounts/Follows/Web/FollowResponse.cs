using Api.Accounts.Profiles;

namespace Api.Accounts.Follows.Web
{
  public class FollowResponse
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public FollowProfile Profile { get; set; }
  }
  
  public class FollowProfile
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Gender Gender { get; set; }
    public string Photo { get; set; }
    public string Description { get; set; }
  }
}