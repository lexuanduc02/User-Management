namespace App.Common.Models.User.Request
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }   
    }
}
