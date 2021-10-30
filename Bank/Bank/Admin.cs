using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Admin
    {
        public Admin()
        {

        }

        public Admin(string username, SecureString password, string fullName)
        {
            this.username = username;
            this.password = password;
            this.fullName = fullName;
        }
        public string username { get; set; }

        public SecureString password { get; set; }

        public string fullName { get; set; }
    }
}

