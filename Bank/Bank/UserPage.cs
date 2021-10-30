using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class UserPage
    {

        //Loggs in user if authorized and prints users page 
        public static bool LoginUser(string username, SecureString password, List<User> users)
        {
            if (Security.AuthorizeUser(username, password, users))
            {
                Console.Clear();
                PrintAccountInfo();
                return true;
            }
            return false;
        }

        //Generates a list of users 
        public static List<User> GenerateUsers()
        {
            //file path of all user information
            string filePath = Environment.CurrentDirectory + @"\user.txt";

            List<string> lines = new List<string>();
            List<User> users = new List<User>();

            //reads all the lines from file and compiles into list of lines 
            lines = File.ReadAllLines(filePath).ToList();

            //adds each line into an array then creates a user profile and stores it in the data structure.
            foreach(string line in lines)
            {
                string[] items = line.Split(',');

                int offset = int.Parse(items[7]);
                
                //decodes and secures password while in application
                SecureString securePass = Security.Decode(items[1], offset);

                User u = new User(items[0], securePass, items[2], int.Parse(items[3]), decimal.Parse(items[4]), int.Parse(items[5]), bool.Parse(items[6]));
                users.Add(u);
;            }
            return users;
        }

        
        //initiates a transaction on users account with a transaction type and amount 
        public static void TransactionFromAccount(string transactionType, int amount, int accountNumber, List<User> users)
        {
            User user = Security.GetUser();

            switch (transactionType)
            {
                //withdraws money from account if within overdraft limit 
                case "withdraw":
                    if ((user.balance - amount) >= 0)
                    {
                        user.balance -= amount;

                        UpdateUsers(users);

                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "               Withdrawed " + amount.ToString("C") + " from your account. press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();

                        PrintAccountInfo();
                    }
                    else if ((user.balance - amount) < 0 && (user.balance - amount) >= (user.overdraftLimit * -1))
                    {
                        bool choosing = true;

                        while (choosing)
                        {
                            Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                                          WARNING AHEAD!");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.Write("\n" + "You are about to go into overdraft! are you sure you want to make a withdrawal?(yes or no) ");

                        string anwser = Console.ReadLine().ToLower().Trim();
                        
                            switch (anwser)
                            {
                                case "yes":
                                    choosing = false;
                                    user.balance -= amount;

                                    UpdateUsers(users);

                                    Console.Clear();
                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                    Console.WriteLine("\n" + "               Withdrawed " + amount.ToString("C") + " from your account. press enter to continue.");
                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                    Console.ReadKey();
                                    Console.Clear();

                                    PrintAccountInfo();
                                    break;
                                case "no":
                                    choosing = false;
                                    Console.Clear();
                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                    Console.WriteLine("\n" + "                         Aborted transaction. press enter to continue.");
                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                    Console.ReadKey();
                                    Console.Clear();

                                    PrintAccountInfo();
                                    break;
                                default:
                                    Console.Clear();
                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                    Console.WriteLine("\n" + "                               Error: Invalid anwser try again. press enter to continue.");
                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                            }
                        }                       
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                                 Insufficient funds. press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();

                        PrintAccountInfo();
                    }
                    break;
                //deposits money into a users account
                case "deposit":
                        user.balance += amount;


                    UpdateUsers(users);

                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "             Deposited " + amount.ToString("C") + " into your account. press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();

                    PrintAccountInfo();
                    break;
                //will transfer only if a user is matched and if it meets tranfer amount requirments
                case "transfer":
                    bool userPresent = false;

                    User transferToUser = null;

                    foreach(User person in users)
                    {
                        if(person.accountNumber == accountNumber)
                        {
                            userPresent = true;
                            transferToUser = person;
                        }
                    }

                    if (userPresent)
                    {
                        if ((user.balance - amount) >= 0)
                        {
                            user.balance -= amount;
                            transferToUser.balance += amount;

                            UpdateUsers(users);

                            Console.Clear();
                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                            Console.WriteLine("\n" + "          Transferd " + amount.ToString("C") + " from your account to " + transferToUser.fullName + ". Press enter to continue.");
                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                            Console.ReadKey();
                            Console.Clear();

                            PrintAccountInfo();
                        }
                        else if ((user.balance - amount) < 0 && (user.balance - amount) >= (user.overdraftLimit * -1))
                        {
                            bool choosing = true;

                            while (choosing)
                            {
                                Console.Clear();
                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("\n" + "                                          WARNING AHEAD!");
                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                Console.Write("\n" + "You are about to go into overdraft! are you sure you want to make a withdrawal?(yes or no) ");

                                string anwser = Console.ReadLine().ToLower().Trim();

                                switch (anwser)
                                {
                                    case "yes": 
                                        choosing = false;
                                        user.balance -= amount;
                                        transferToUser.balance += amount;

                                        UpdateUsers(users);

                                        Console.Clear();
                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                        Console.WriteLine("\n" + "          Transferd " + amount.ToString("C") + " from your account to " + transferToUser.fullName + ". Press enter to continue.");
                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                        Console.ReadKey();
                                        Console.Clear();

                                        PrintAccountInfo();
                                        break;
                                    case "no":
                                        choosing = false;

                                        Console.Clear();
                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                        Console.WriteLine("\n" + "                         Aborted transaction. press enter to continue.");
                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                        Console.ReadKey();
                                        Console.Clear();

                                        PrintAccountInfo();
                                        break;
                                    default:
                                        Console.Clear();
                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                        Console.WriteLine("\n" + "                        Error: Invalid anwser try again. press enter to continue.");
                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                        Console.ReadKey();
                                        Console.Clear();
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                            Console.WriteLine("\n" + "                               Insufficient funds. press enter to continue.");
                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                            Console.ReadKey();
                            Console.Clear();

                            PrintAccountInfo();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                        Console.WriteLine("\n" + "                     Account number you gave does not exsist. press enter to continue.");
                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                        Console.ReadKey();
                        Console.Clear();

                        PrintAccountInfo();
                    }
                    break;
            } 
        }

        //prints users information to the console
        public static void PrintAccountInfo()
        {
            User user = Security.GetUser();

            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                         Heres " + user.username + " account information.");
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                        Account Holder is: " + user.fullName);
            Console.WriteLine("\n" + "                        Account Number is: " + user.accountNumber);
            Console.WriteLine("\n" + "                        Account Balance is: " + user.balance.ToString("C"));
            Console.WriteLine("\n" + "                        Account Overdraft Limit is: " + user.overdraftLimit);
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
        }

        public static void UpdateUsers(List<User> users)
        {
            User currentUser = Security.GetUser();
            users.Add(currentUser);

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
                newLine += user.accountFreeze;
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
