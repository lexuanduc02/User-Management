﻿namespace App.Common.Models.User.Dtos
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }    
        public string Email { get; set; }
    }
}
