using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataAccess.Models
{
    public class User : ModelBase
    {
        public User(string username, string password, string rePassword)
            : base()
        {
            Username = username;
            Password = password;
            RePassword = rePassword;
        }
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
