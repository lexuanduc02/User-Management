using App.Common.Models.User.Dtos;

namespace App.Common.Models.User.Result
{
    public class LoginResult
    {
        public UserViewModel UserInfor {  get; set; }
        public Token Token { get; set; }
    }

    public class Token
    {
        public string AccessToken { get; set; }
        public string ExpireIn { get; set; }
    }
}
