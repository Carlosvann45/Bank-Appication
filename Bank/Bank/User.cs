using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class User
    {
        public User()
        {
        }

        public User(string username, SecureString password, string fullName, int accountNumber, decimal balance, int overdraftLimit, bool accountFreeze)
        {
            this.username = username;
            this.password = password;
            this.fullName = fullName;
            this.accountNumber = accountNumber;
            this.balance = balance;
            this.overdraftLimit = overdraftLimit;
            this.accountFreeze = accountFreeze;
        }
        public string username { get; set; }

        public SecureString password { get; set; }

        public string fullName { get; set; }

        public int accountNumber { get; set; }

        public decimal balance { get; set; }

        public int overdraftLimit { get; set; }

        public bool accountFreeze { get; set; }
    }
} 
