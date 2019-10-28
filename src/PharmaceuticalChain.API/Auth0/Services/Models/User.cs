using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Auth0.Services.Models
{
    public class User
    {
        public string UserId;
        public string Email;
        public bool IsEmailVerified;
        public bool IsVerifyingEmail;
        public string Password;
        public string Connection;
        public object AppMetadata;

        public User(string userId, string email, string password, string role)
        {
            this.UserId = userId;
            this.Email = email;
            this.Password = password;
            this.IsEmailVerified = false;
            this.IsVerifyingEmail = true;
            this.Connection = "PharmaChain-DB";
            this.AppMetadata = new { roles = role };
        }
    }
}
