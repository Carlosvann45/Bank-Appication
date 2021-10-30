using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class AdminPage
    {
        //Loggs in admin if authorized and prints users page 
        public static bool LoginAdmin(string username, SecureString password, List<Admin> admins)
        {
            bool loginAnwser = false;
            if (Security.AuthorizeAdmin(username, password, admins))
            {
                Console.Clear();
                PrintAccountInfo();
                loginAnwser = true;
            }
            return loginAnwser;
        }

        //will logout admins and overwrite data file with changed information
        public static void UpdateAdmins(List<Admin> admins)
        {
            Admin currentUser = Security.GetAdmin();
            admins.Add(currentUser);

            List<string> updatedAdmins = new List<string>();

            Random rand = new Random();

            foreach (Admin admin in admins)
            {
                string newLine = "";
                int offset = rand.Next(1, 9);
                newLine += admin.username;
                newLine += ",";
                string tempPass = Security.Encode(admin.password, offset);
                newLine += tempPass;
                newLine += ",";
                newLine += admin.fullName;
                newLine += ",";
                newLine += offset;


                updatedAdmins.Add(newLine);
            }

            //overwrites file with path directory and list of updated users
            try
            {
                File.WriteAllLines(Environment.CurrentDirectory + @"\admin.txt", updatedAdmins);
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.Clear();
                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n" + "                              ERROR: " + exception.GetType());
                Console.WriteLine("\n" + "                                           Error Message: ");
                Console.WriteLine("\n" + exception.Message);
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n" + "      If you are seeing this message the data was not able to be saved and no changes were made.");
                Console.WriteLine("\n" + "     This is probably because this program does not have the proper authorization from you system.");
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                Console.ReadKey();
                Console.Clear();
            }

            //resets user so no one can access the information
            Admin resetAdmin = new Admin();
            Security.SetAdmin(resetAdmin);
        }

        public static List<Admin> GenerateAdmins()
        {
            //file path of all user information
            string filePath = Environment.CurrentDirectory + @"\admin.txt";

            List<string> lines = new List<string>();
            List<Admin> admins = new List<Admin>();

            //reads all the lines from file and compiles into list of lines 
            lines = File.ReadAllLines(filePath).ToList();

            //adds each line into an array then creates a user profile and stores it in the data structure.
            foreach (string line in lines)
            {
                string[] items = line.Split(',');

                 int offset = int.Parse(items[3]);

                //decodes and secures password while in application
                SecureString securePass = Security.Decode(items[1], offset);

                Admin a = new Admin(items[0], securePass, items[2]);
                admins.Add(a);
            }
            return admins;
        }

        public static void AdminCommand(List<User> users, User user ,int accountNumber ,  string command)
        {
            switch (command)
            {
                case "create":

                    bool userExist = false;

                    foreach (User person in users)
                    {
                        if (person.username == user.username)
                        {
                            userExist = true;
                        }
                    }

                    if (userExist)
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                             Username exsist. Press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        users.Add(user);

                        UpdateUsers(users);

                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                     " + user.username + " has been added. Press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
                case "delete":
                    bool deleteUser = false;
                    foreach (User person in users)
                    {
                        if (person.accountNumber == accountNumber)
                        {
                            deleteUser = true;
                            user = person;
                        }
                    }
                    if (deleteUser)
                    {
                        users.Remove(user);

                        UpdateUsers(users);

                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                             User has been deleted. Press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                             User does not exsist. Press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    break;
            }
        }
        public static string CreateAccountNumber(List<User> users)
        {
            string newAccountNumber = "";

            Random rand = new Random();

            bool numberFound = false;

            while (!numberFound)
            {
                bool accountNumberMatches = false;

                for (int ndx = 1; ndx <= 6; ndx++)
                {
                    newAccountNumber += rand.Next(1, 9).ToString();
                }
                foreach (User user in users)
                {
                    if (user.accountNumber == int.Parse(newAccountNumber))
                    {
                        accountNumberMatches = true;
                    }
                }
                if (!accountNumberMatches)
                {
                    numberFound = true;
                }
                else
                {
                    newAccountNumber = "";
                }
            }
            return newAccountNumber;
        }

        //prints admins information to the console
        public static void PrintAccountInfo()
        {
            Admin admin = Security.GetAdmin();

            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                        Welcome " + admin.fullName + "! Here is a list of Commands!");
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
        }

        public static void UpdateUsers(List<User> users)
        {
            List<string> updatedUsers = new List<string>();

            Random rand = new Random();

            foreach (User user in users)
            {
                string newLine = "";
                int offset = rand.Next(1, 9);

                newLine += user.username;
                newLine += ",";
                string tempPass = Security.Encode(user.password, offset);
                newLine += tempPass;
                newLine += ",";
                newLine += user.fullName;
                newLine += ",";
                newLine += user.accountNumber;
                newLine += ",";
                newLine += user.balance;
                newLine += ",";
                newLine += user.overdraftLimit;
                newLine += ",";
                newLine += offset;

                updatedUsers.Add(newLine);
            }

            //overwrites file with path directory and list of updated users
            try
            {
                File.WriteAllLines(Environment.CurrentDirectory + @"\user.txt", updatedUsers);
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.Clear();
                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n" + "                              ERROR: " + exception.GetType());
                Console.WriteLine("\n" + "                                           Error Message: ");
                Console.WriteLine("\n" + exception.Message);
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                Console.WriteLine("\n" + "      If you are seeing this message the data was not able to be saved and no changes were made.");
                Console.WriteLine("\n" + "     This is probably because this program does not have the proper authorization from you system.");
                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                Console.ReadKey();
                Console.Clear();
            }
        }

    }
}
