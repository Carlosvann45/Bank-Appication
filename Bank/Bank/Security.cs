using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Security
    {
        private static User user;

        private static Admin admin;

        public static User GetUser()
        {
            return user;
        }

        public static void SetUser(User newUser)
        {
            user = newUser;
        }

        public static Admin GetAdmin()
        {
            return admin;
        }

        public static void SetAdmin(Admin newAdmin)
        {
            admin = newAdmin;
        }

        //checks to make sure a user data is verified then gives access to users information 
        public static bool AuthorizeUser(string username, SecureString password, List<User> users)
        {
            foreach (User person in users)
            {
                SecureString actualPassword = person.password;

                //trims whitespace and sets username and password from users
                string actualUsername = person.username.Trim();

                username = username.Trim();

                if (actualUsername == username)
                {
                  
                    if (ComparePasswords(actualPassword, password))
                    {
                        user = person;
                        users.Remove(person);
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                               Access has been denied! Incorrect password");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        return false;
                    }
                        
                }
            }
            Console.Clear();
            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                               Access has been denied! Incorrect username");
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
            return false;
        }


        //checks to make sure a admin data is verified then gives access to admins permissions
        public static bool AuthorizeAdmin(string username, SecureString password, List<Admin> admins)
        {
            foreach (Admin person in admins)
            {
                SecureString actualPassword = person.password;

                //trims whitespace and sets username and password from admins
                string actualUsername = person.username.Trim();

                username = username.Trim();

                if (actualUsername == username)
                {
                    if (ComparePasswords(actualPassword, password))
                    {
                        admin = person;
                        admins.Remove(person);
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                               Access has been denied! Incorrect password");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        return false;
                    }

                }
            }
            Console.Clear();
            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                               Access has been denied! Incorrect username");
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
            return false;
        }

        public static bool ComparePasswords(SecureString firstPass, SecureString secondPass)
        {
            bool samePass = false;

            //converts password to string and tests it 
            IntPtr valuePass1 = IntPtr.Zero;
            IntPtr valuePass2 = IntPtr.Zero;

            valuePass1 = Marshal.SecureStringToGlobalAllocUnicode(firstPass);
            valuePass2 = Marshal.SecureStringToGlobalAllocUnicode(secondPass);

            if (Marshal.PtrToStringUni(valuePass1).Trim() == Marshal.PtrToStringUni(valuePass2).Trim())
            {
                samePass = true;
            }
                return samePass;
        }

        public static string Encode(SecureString password, int offset)
        {
            IntPtr valuePass = IntPtr.Zero;
            valuePass = Marshal.SecureStringToGlobalAllocUnicode(password);

            StringBuilder sb = new StringBuilder(Marshal.PtrToStringUni(valuePass).Trim());

            for (int ndx = 0; ndx < sb.Length; ndx++)
            {
                sb[ndx] = (char)(sb[ndx] + offset);
            }

            return sb.ToString();
        }

        public static SecureString Decode(string password, int offset)
        {
            StringBuilder sb = new StringBuilder(password.Trim());
            
            SecureString securePass = new SecureString();

            for (int ndx = 0; ndx < sb.Length; ndx++)
            {
                securePass.AppendChar((char)(sb[ndx] - offset));
            }
            
            return securePass;
        }
    }
}
       