using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Auth0.Services.Models
{
    public class User
    {
        public string user_id;
        public string email;
        public bool email_verified;
        public bool verify_email;
        public string password;
        public string connection;
        public object app_metadata;

        public User(string user_id, string email, string password, string role)
        {
            this.user_id = user_id;
            this.email = email;
            this.password = password;
            this.email_verified = false;
            this.verify_email = true;
            this.connection = "PharmaChain-DB";
            this.app_metadata = new { roles = role };
        }
    }
}
