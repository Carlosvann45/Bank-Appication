using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    class Program
    {
        static void Main(string[] args)
        {
            bool closeApp = false;

            //runs untill you exit the app
            while (!closeApp)
            {
                Console.WriteLine("Welcome to the Programers Choice Bank of the World!");
                Console.Write("\n" + "To login tell us if you are a user, admin, or just say exit: ");

                string adminAnwser = Console.ReadLine().ToLower().Trim();

                //swithes roles depending on input
                switch (adminAnwser)
                {
                    case "admin":
                        List<Admin> admins = AdminPage.GenerateAdmins();

                        List<User> listOfUsers = UserPage.GenerateUsers();

                        bool adminLoggedIn = true;

                        //will run through login sequence untill user logsout or exits before logging in
                        while (adminLoggedIn)
                        {
                            Console.WriteLine("\n" + " ----------------");
                            Console.WriteLine("/  Login Admin  /");
                            Console.WriteLine("----------------");
                            Console.Write("\n" + "Username: ");

                            string username = Console.ReadLine().ToLower().Trim();

                            Console.Write("\n" + "Password: ");

                            SecureString password = new SecureString();

                            ConsoleKeyInfo keyEntry;

                            //replaces password with * after saving character
                            do
                            {
                                keyEntry = Console.ReadKey(true);

                                if (keyEntry.Key != ConsoleKey.Backspace)
                                {
                                    password.AppendChar(keyEntry.KeyChar);

                                    Console.Write("*");
                                }
                                else
                                {
                                    //checks if string is null or empty
                                    if (!string.IsNullOrEmpty(password.ToString()))
                                    {
                                        //remove last character
                                        password.RemoveAt(password.Length - 1);

                                        int pos = Console.CursorLeft;
                                        //moves cursor to the left
                                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                        //replace star character
                                        Console.Write(" ");
                                        //move cursor to the left
                                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                    }
                                }
                            }
                            while (keyEntry.Key != ConsoleKey.Enter);

                            if (AdminPage.LoginAdmin(username, password, admins))
                            {
                                bool adminCommandInProgress = true;

                                while (adminCommandInProgress)
                                {
                                    Console.WriteLine("\n" + "You can create, delete, freeze,and view users, or change your password and logout.");
                                    Console.Write("commands: create, delete, freeze, view, change password, or logout. ");

                                    string command = Console.ReadLine().Trim();

                                    switch (command)
                                    {
                                        case "create":
                                            string newUsername = "";

                                            bool choosingUsername = true;

                                            while (choosingUsername)
                                            {
                                                Console.Write("\n" + "Enter a username: ");
                                                string checkNewUsername = Console.ReadLine();

                                                if (checkNewUsername.Length >= 6)
                                                {
                                                    newUsername = checkNewUsername;
                                                    choosingUsername = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                    Username must have atleast 6 characters. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }
                                            }

                                            SecureString newPassword = new SecureString();

                                            bool choosingPassword = true;

                                            while (choosingPassword)
                                            {
                                                Console.Write("\n" + "Enter a password: ");

                                                SecureString firstCheckPassword = new SecureString();

                                                ConsoleKeyInfo newKeyEntry;

                                                //replaces password with * after saving character
                                                do
                                                {
                                                    newKeyEntry = Console.ReadKey(true);

                                                    if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                    {
                                                        firstCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                        Console.Write("*");
                                                    }
                                                    else
                                                    {
                                                        //checks if string is null or empty
                                                        if (!string.IsNullOrEmpty(firstCheckPassword.ToString()))
                                                        {
                                                            //remove last character
                                                            firstCheckPassword.RemoveAt(firstCheckPassword.Length - 1);

                                                            int pos = Console.CursorLeft;
                                                            //moves cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            //replace star character
                                                            Console.Write(" ");
                                                            //move cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                        }
                                                    }
                                                }
                                                while (newKeyEntry.Key != ConsoleKey.Enter);

                                                Console.Write("\n\n" + "Retype your password: ");

                                                SecureString secondCheckPassword = new SecureString();

                                                //replaces password with * after saving character
                                                do
                                                {
                                                    newKeyEntry = Console.ReadKey(true);

                                                    if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                    {
                                                        secondCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                        Console.Write("*");
                                                    }
                                                    else
                                                    {
                                                        //checks if string is null or empty
                                                        if (!string.IsNullOrEmpty(secondCheckPassword.ToString()))
                                                        {
                                                            //remove last character
                                                            firstCheckPassword.RemoveAt(secondCheckPassword.Length - 1);

                                                            int pos = Console.CursorLeft;
                                                            //moves cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            //replace star character
                                                            Console.Write(" ");
                                                            //move cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                        }
                                                    }
                                                }
                                                while (newKeyEntry.Key != ConsoleKey.Enter);

                                                if(Security.ComparePasswords(firstCheckPassword, secondCheckPassword))
                                                {
                                                    newPassword = firstCheckPassword;
                                                    choosingPassword = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                              Password must match. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }
                                            }

                                            string newFullName = "";

                                            bool choosingFullName = true;

                                            while (choosingFullName)
                                            {
                                                Console.Write("\n\n" + "Enter the full name: ");
                                                string checkFullName = Console.ReadLine();

                                                //matches param for a full name
                                                Regex rgx = new Regex(@"[a-zA-z]+[\s][a-zA-Z]+");

                                                if (rgx.IsMatch(checkFullName))
                                                {
                                                    newFullName = checkFullName;
                                                    choosingFullName = false;
                                                }
                                                else
                                                {

                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "           Minimum of first and last name with one space inbetween. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }
                                            }

                                            string newAccountNumber = AdminPage.CreateAccountNumber(listOfUsers);

                                            decimal newAccountBalance = 0;
                                            bool pickingNumber = true;

                                            while (pickingNumber)
                                            {
                                                Console.Write("\n" + "Enter a starting balance: ");
                                                string checkNumber = Console.ReadLine();

                                                if (decimal.TryParse(checkNumber, out decimal depositAmount))
                                                {
                                                    if(depositAmount > 0)
                                                    {
                                                        newAccountBalance = depositAmount;
                                                        pickingNumber = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                           Number must be greater than 0. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                               Invalid Number. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }

                                            }

                                            int newOverdraftLimit = 0;
                                            bool pickingDraftNumber = true;

                                            while (pickingDraftNumber)
                                            {
                                                Console.Write("\n" + "Enter a overdraft limit less than 500: ");
                                                string checkNumber = Console.ReadLine();

                                                if (int.TryParse(checkNumber, out int overDraftAmount))
                                                {
                                                    if(overDraftAmount >= 0 && overDraftAmount <= 500)
                                                    {
                                                        newOverdraftLimit = overDraftAmount;
                                                        pickingDraftNumber = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                            Number must be between 0 and 500. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                               Invalid Number. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }

                                            }


                                            bool newAccountFreeze = false;

                                            User newUser = new User(newUsername, newPassword, newFullName, int.Parse(newAccountNumber), newAccountBalance, newOverdraftLimit, newAccountFreeze);

                                            AdminPage.AdminCommand(listOfUsers, newUser, 0, "create");

                                            AdminPage.PrintAccountInfo();
                                            break;
                                        case "delete":
                                            bool checkingAcctNum = true;

                                            while (checkingAcctNum)
                                            {
                                                Console.Write("\n" + "Enter 6 digit account number you would like to delete: ");
                                                string checkCorrectAcct = Console.ReadLine();

                                                if (int.TryParse(checkCorrectAcct, out int deleteAcctNum))
                                                {
                                                    if (deleteAcctNum >= 100000 && deleteAcctNum <= 999999)
                                                    {
                                                        AdminPage.AdminCommand(listOfUsers, null, deleteAcctNum, "delete");
                                                        checkingAcctNum = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                  Number must be exactly 6 digits and not negative. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                         Must be a number. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }
                                            }

                                            AdminPage.PrintAccountInfo();
                                            break;
                                        case "freeze":
                                            bool choosingFreeze = true;

                                            while (choosingFreeze)
                                            {
                                                Console.Write("\n" + "Would you like to freeze or unfreeze? ");
                                                string freezeAnwser = Console.ReadLine();

                                                switch (freezeAnwser)
                                                {
                                                    case "freeze":
                                                        int acctNumber = 0;

                                                        bool choosing = true;

                                                        while (choosing)
                                                        {
                                                            bool choosingAcc = true;

                                                            while (choosingAcc)
                                                            {
                                                                Console.Write("\n" + "Enter 6 digit account number you would like to freeze: ");
                                                                string checkCorrectAcct = Console.ReadLine();

                                                                if (int.TryParse(checkCorrectAcct, out int acctNum))
                                                                {
                                                                    if (acctNum >= 100000 && acctNum <= 999999)
                                                                    {
                                                                        acctNumber = acctNum;
                                                                        choosingAcc = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                        Console.WriteLine("\n" + "                  Number must be exactly 6 digits and not negative. Please try again.");
                                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                         Must be a number. Please try again.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                }
                                                            }

                                                            bool foundUser = false;

                                                            bool userIsAlreadyFrozen = false;

                                                            foreach (User person in listOfUsers)
                                                            {
                                                                if (person.accountNumber == acctNumber)
                                                                {
                                                                    if (person.accountFreeze != true)
                                                                    {
                                                                        Security.SetUser(person);
                                                                        User updateUser = Security.GetUser();
                                                                        updateUser.accountFreeze = true;
                                                                        foundUser = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        userIsAlreadyFrozen = true;
                                                                    }
                                                                }
                                                            }

                                                            if (userIsAlreadyFrozen)
                                                            {
                                                                choosingFreeze = false;
                                                                choosing = false;

                                                                Console.Clear();
                                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                Console.WriteLine("\n" + "                       Account is already frozen. Press enter to continue.");
                                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                Console.ReadKey();
                                                                Console.Clear();
                                                            }
                                                            else
                                                            {
                                                                if (foundUser)
                                                                {
                                                                    choosingFreeze = false;
                                                                    choosing = false;

                                                                    User resetPerson = new User();
                                                                    Security.SetUser(resetPerson);

                                                                    Console.Clear();
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                          User frozen. Press enter to continue.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.ReadKey();
                                                                    Console.Clear();
                                                                }
                                                                else
                                                                {
                                                                    choosingFreeze = false;
                                                                    choosing = false;

                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                          User doesnt exist. Press enter to continue.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.ReadKey();
                                                                    Console.Clear();
                                                                }
                                                            }
                                                        }

                                                        AdminPage.PrintAccountInfo();
                                                        break;
                                                    case "unfreeze":
                                                        int acctToUnfreeze = 0;

                                                        bool choosing2 = true;

                                                        while (choosing2)
                                                        {
                                                            bool choosingAcc = true;

                                                            while (choosingAcc)
                                                            {
                                                                Console.Write("\n" + "Enter 6 digit account number you would like to freeze: ");
                                                                string checkCorrectAcct = Console.ReadLine();

                                                                if (int.TryParse(checkCorrectAcct, out int acctNum))
                                                                {
                                                                    if (acctNum >= 100000 && acctNum <= 999999)
                                                                    {
                                                                        acctToUnfreeze = acctNum;
                                                                        choosingAcc = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                        Console.WriteLine("\n" + "                  Number must be exactly 6 digits and not negative. Please try again.");
                                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                         Must be a number. Please try again.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                }
                                                            }

                                                            bool foundUser = false;

                                                            bool userIsAlreadyUnfrozen = false;

                                                            foreach (User person in listOfUsers)
                                                            {
                                                                if (person.accountNumber == acctToUnfreeze)
                                                                {
                                                                    if (person.accountFreeze != false)
                                                                    {
                                                                        Security.SetUser(person);
                                                                        User updateUser = Security.GetUser();
                                                                        updateUser.accountFreeze = false;
                                                                        foundUser = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        userIsAlreadyUnfrozen = true;
                                                                    }
                                                                }
                                                            }

                                                            if (userIsAlreadyUnfrozen)
                                                            {
                                                                choosingFreeze = false;
                                                                choosing2 = false;

                                                                Console.Clear();
                                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                Console.WriteLine("\n" + "                       Account is already unfrozen. Press enter to continue.");
                                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                Console.ReadKey();
                                                                Console.Clear();
                                                            }
                                                            else
                                                            {
                                                                if (foundUser)
                                                                {
                                                                    choosingFreeze = false;
                                                                    choosing2 = false;

                                                                    User resetPerson = new User();
                                                                    Security.SetUser(resetPerson);

                                                                    Console.Clear();
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                         User is now unfrozen. Press enter to continue.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.ReadKey();
                                                                    Console.Clear();
                                                                }
                                                                else
                                                                {
                                                                    choosingFreeze = false;
                                                                    choosing2 = false;

                                                                    Console.Clear();
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                          User doesnt exist. Press enter to continue.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.ReadKey();
                                                                    Console.Clear();
                                                                }
                                                            }
                                                        }

                                                        AdminPage.PrintAccountInfo();
                                                        break;
                                                    default:
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                          Invalid anwser. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        break;
                                                }
                                            }
                                            
                                            break;
                                        case "view":
                                            bool deciding = true;

                                            while(deciding) {
                                                Console.Write("\n" + "Would you like to view all or one? ");
                                                string decision = Console.ReadLine();

                                                switch (decision)
                                                {
                                                    case "one":

                                                        int accountNumber = 0;

                                                        bool checkAcctNum = true;

                                                        while (checkAcctNum)
                                                        {
                                                            Console.Write("\n" + "Enter 6 digit account number you would like to view: ");
                                                            string checkCorrectAcct = Console.ReadLine();

                                                            if (int.TryParse(checkCorrectAcct, out int acctNum))
                                                            {
                                                                if (accountNumber >= 100000 && accountNumber <= 999999)
                                                                {
                                                                        accountNumber = acctNum;
                                                                        checkAcctNum = false;
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                    Console.WriteLine("\n" + "                  Number must be exactly 6 digits and not negative. Please try again.");
                                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                                Console.WriteLine("\n" + "                         Must be a number. Please try again.");
                                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                            }
                                                        }

                                                        bool foundUser = false;

                                                        foreach (User person in listOfUsers)
                                                        {
                                                            if (person.accountNumber == accountNumber)
                                                            {
                                                                Security.SetUser(person);
                                                                UserPage.PrintAccountInfo();
                                                                foundUser = true;
                                                            }
                                                        }

                                                        if (foundUser)
                                                        {
                                                            deciding = false;

                                                            User resetPerson = new User();
                                                            Security.SetUser(resetPerson);

                                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                            Console.WriteLine("\n" + "                          Press enter to continue.");
                                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                            Console.ReadKey();
                                                            Console.Clear();
                                                        }
                                                        else
                                                        {
                                                            deciding = false;

                                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                            Console.WriteLine("\n" + "                          User doesnt exist. Press enter to continue.");
                                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                            Console.ReadKey();
                                                            Console.Clear();
                                                        }


                                                        AdminPage.PrintAccountInfo();
                                                        break;
                                                    case "all":
                                                        Console.Clear();

                                                        foreach (User user in listOfUsers)
                                                        {
                                                            Security.SetUser(user);
                                                            UserPage.PrintAccountInfo();
                                                        }

                                                        User resetUser = new User();
                                                        Security.SetUser(resetUser);

                                                        deciding = false;

                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                          Press enter to continue.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        Console.ReadKey();
                                                        Console.Clear();

                                                        AdminPage.PrintAccountInfo();
                                                        break;
                                                    default:
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                          Invalid anwser. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        break;
                                                }
                                            }
                                            break;
                                        case "change password":
                                            SecureString changedPassword = new SecureString();

                                            bool changingPassword = true;

                                            while (changingPassword)
                                            {
                                                Console.Write("\n" + "Enter a password: ");

                                                SecureString firstCheckPassword = new SecureString();

                                                ConsoleKeyInfo newKeyEntry;

                                                //replaces password with * after saving character
                                                do
                                                {
                                                    newKeyEntry = Console.ReadKey(true);

                                                    if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                    {
                                                        firstCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                        Console.Write("*");
                                                    }
                                                    else
                                                    {
                                                        //checks if string is null or empty
                                                        if (!string.IsNullOrEmpty(firstCheckPassword.ToString()))
                                                        {
                                                            //remove last character
                                                            firstCheckPassword.RemoveAt(firstCheckPassword.Length - 1);

                                                            int pos = Console.CursorLeft;
                                                            //moves cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            //replace star character
                                                            Console.Write(" ");
                                                            //move cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                        }
                                                    }
                                                }
                                                while (newKeyEntry.Key != ConsoleKey.Enter);

                                                Console.Write("\n\n" + "Retype your password: ");

                                                SecureString secondCheckPassword = new SecureString();

                                                //replaces password with * after saving character
                                                do
                                                {
                                                    newKeyEntry = Console.ReadKey(true);

                                                    if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                    {
                                                        secondCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                        Console.Write("*");
                                                    }
                                                    else
                                                    {
                                                        //checks if string is null or empty
                                                        if (!string.IsNullOrEmpty(secondCheckPassword.ToString()))
                                                        {
                                                            //remove last character
                                                            firstCheckPassword.RemoveAt(secondCheckPassword.Length - 1);

                                                            int pos = Console.CursorLeft;
                                                            //moves cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            //replace star character
                                                            Console.Write(" ");
                                                            //move cursor to the left
                                                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                        }
                                                    }
                                                }
                                                while (newKeyEntry.Key != ConsoleKey.Enter);

                                                if (Security.ComparePasswords(firstCheckPassword, secondCheckPassword))
                                                {
                                                    changedPassword = firstCheckPassword;
                                                    changingPassword = false;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                              Password must match. Please try again.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                }
                                            }
                                            Admin currentAdmin = Security.GetAdmin();
                                            currentAdmin.password = changedPassword;

                                            AdminPage.UpdateAdmins(admins);

                                            Console.Clear();
                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                            Console.WriteLine("\n" + "                         Password Updated. press enter to continue.");
                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                            Console.ReadKey();
                                            Console.Clear();

                                            AdminPage.PrintAccountInfo();
                                            break;
                                        case "logout":
                                            adminCommandInProgress = false;
                                            adminLoggedIn = false;

                                            Console.Clear();
                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                            Console.WriteLine("\n" + "                    You have been logged out. Press enter to continue.");
                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        default:
                                            Console.Clear();
                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                            Console.WriteLine("\n" + "                      Error: Invalid anwser try again. press enter to continue.");
                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                            Console.ReadKey();
                                            Console.Clear();

                                            AdminPage.PrintAccountInfo();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                bool adminCorrectAnwser = false;

                                //if login is incorrect it will ask if user would like to switch roles or exit application
                                while (!adminCorrectAnwser)
                                {
                                    Console.Write("\n" + "Would you like to exit application or switch roles?(exit, continue, or switch) ");

                                    string exitAnwser = Console.ReadLine().ToLower().Trim();

                                    switch (exitAnwser)
                                    {
                                        case "exit":
                                            adminCorrectAnwser = true;
                                            adminLoggedIn = false;
                                            closeApp = true;
                                            break;
                                        case "continue":
                                            adminCorrectAnwser = true;
                                            break;
                                        case "switch":
                                            Console.Clear();
                                            adminCorrectAnwser = true;
                                            adminLoggedIn = false;
                                            break;
                                        default:
                                            Console.Clear();
                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                            Console.WriteLine("\n" + "                               Error: Invalid anwser try again.");
                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                            break;
                                    }
                                }
                            }
                         }
                            break;
                    case "user":
                        List<User> users = UserPage.GenerateUsers();

                        bool userLoggedIn = true;
 
                        //will run through login sequence untill user logsout or exits before logging in
                        while (userLoggedIn)
                        {
                            Console.WriteLine("\n" + " ----------------");
                            Console.WriteLine("/  Login User  /");
                            Console.WriteLine("----------------");
                            Console.Write("\n" + "Username: ");

                            string username = Console.ReadLine().ToLower().Trim();

                            Console.Write("\n" + "Password: ");

                            SecureString password = new SecureString();

                            ConsoleKeyInfo keyEntry;

                            //replaces password with * after saving character
                            do
                            {
                                keyEntry = Console.ReadKey(true);

                                if (keyEntry.Key != ConsoleKey.Backspace)
                                {
                                    password.AppendChar(keyEntry.KeyChar);

                                    Console.Write("*");
                                }
                                else
                                {   
                                    //checks if string is null or empty
                                    if (!string.IsNullOrEmpty(password.ToString()))
                                    {
                                        //remove last character
                                        password.RemoveAt(password.Length - 1);
                                        int pos = Console.CursorLeft;
                                        //moves cursor to the left
                                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                        //replace start character
                                        Console.Write(" ");
                                        //move cursor to the left
                                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                    }
                                }
                            }
                            while (keyEntry.Key != ConsoleKey.Enter);

                            //if user is authorized it will load users page
                            if(UserPage.LoginUser(username, password, users))
                            {
                                User checkUser = Security.GetUser();

                                bool accFrozen = checkUser.accountFreeze;

                                if (!accFrozen)
                                {
                                    bool userTransactionInProgress = true;

                                    //will loop through through tansactions untill user logs out
                                    while (userTransactionInProgress)
                                    {
                                        //TODO: add change password function
                                        Console.Write("\n" + "Would you like to withdraw, deposit, transfer, change password, or logout? ");

                                        string userTaskAnwser = Console.ReadLine().ToLower().Trim();

                                        switch (userTaskAnwser)
                                        {
                                            case "withdraw":
                                                Console.Write("\n" + "How much would you like to withdraw?(between(1 - 500) ");

                                                string withdrawAnwser = Console.ReadLine().Trim();

                                                if (int.TryParse(withdrawAnwser, out int withdrawAmount))
                                                {
                                                    if (withdrawAmount <= 500 && withdrawAmount > 0)
                                                    {
                                                        UserPage.TransactionFromAccount("withdraw", withdrawAmount, 0, users);
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                         Error: Must be between 1 - 500. press enter to continue.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        Console.ReadKey();
                                                        Console.Clear();

                                                        UserPage.PrintAccountInfo();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                        Error: Must be A positive Integer. press enter to continue.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    Console.ReadKey();
                                                    Console.Clear();

                                                    UserPage.PrintAccountInfo();
                                                }
                                                break;
                                            case "deposit":
                                                Console.Write("\n" + "How much would you like to deposit?(between 1 - 500) ");

                                                string depositAnwser = Console.ReadLine().Trim();

                                                if (int.TryParse(depositAnwser, out int depositAmount))
                                                {
                                                    if (depositAmount <= 500 && depositAmount > 0)
                                                    {
                                                        UserPage.TransactionFromAccount("deposit", depositAmount, 0, users);
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                         Error: Must be between 1 - 500. press enter to continue.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        Console.ReadKey();
                                                        Console.Clear();

                                                        UserPage.PrintAccountInfo();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                         Error: Must be A positive Integer. press enter to continue.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    Console.ReadKey();
                                                    Console.Clear();

                                                    UserPage.PrintAccountInfo();
                                                }
                                                break;
                                            case "transfer":
                                                Console.Write("\n" + "How much would you like to transfer?(between 1 - 500) ");

                                                string transferAnwser = Console.ReadLine().Trim();

                                                if (int.TryParse(transferAnwser, out int transferAmount))
                                                {

                                                    if (transferAmount <= 500 && transferAmount > 0)
                                                    {
                                                        Console.Write("\n" + "Which account would you like to transfer to?(6 digit account number required) ");

                                                        string accountNumberAnwser = Console.ReadLine().Trim();

                                                        if (int.TryParse(accountNumberAnwser, out int accountNumber) && accountNumberAnwser.Length == 6)
                                                        {
                                                            UserPage.TransactionFromAccount("transfer", transferAmount, accountNumber, users);
                                                        }
                                                        else
                                                        {
                                                            Console.Clear();
                                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                            Console.WriteLine("\n" + "                   Error: Invalid account number. Must be all numbers and 6 digits long.");
                                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                            Console.ReadKey();
                                                            Console.Clear();

                                                            UserPage.PrintAccountInfo();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                         Error: Must be between 1 - 500. press enter to continue.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                        Console.ReadKey();
                                                        Console.Clear();

                                                        UserPage.PrintAccountInfo();
                                                    }

                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                    Console.WriteLine("\n" + "                         Error: Must be A positive Integer. press enter to continue.");
                                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    Console.ReadKey();
                                                    Console.Clear();

                                                    UserPage.PrintAccountInfo();
                                                }
                                                break;
                                            case "change password":
                                                SecureString changedPassword = new SecureString();

                                                bool changingPassword = true;

                                                while (changingPassword)
                                                {
                                                    Console.Write("\n" + "Enter a password: ");

                                                    SecureString firstCheckPassword = new SecureString();

                                                    ConsoleKeyInfo newKeyEntry;

                                                    //replaces password with * after saving character
                                                    do
                                                    {
                                                        newKeyEntry = Console.ReadKey(true);

                                                        if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                        {
                                                            firstCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                            Console.Write("*");
                                                        }
                                                        else
                                                        {
                                                            //checks if string is null or empty
                                                            if (!string.IsNullOrEmpty(firstCheckPassword.ToString()))
                                                            {
                                                                //remove last character
                                                                firstCheckPassword.RemoveAt(firstCheckPassword.Length - 1);

                                                                int pos = Console.CursorLeft;
                                                                //moves cursor to the left
                                                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                                //replace star character
                                                                Console.Write(" ");
                                                                //move cursor to the left
                                                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            }
                                                        }
                                                    }
                                                    while (newKeyEntry.Key != ConsoleKey.Enter);

                                                    Console.Write("\n\n" + "Retype your password: ");

                                                    SecureString secondCheckPassword = new SecureString();

                                                    //replaces password with * after saving character
                                                    do
                                                    {
                                                        newKeyEntry = Console.ReadKey(true);

                                                        if (newKeyEntry.Key != ConsoleKey.Backspace)
                                                        {
                                                            secondCheckPassword.AppendChar(newKeyEntry.KeyChar);

                                                            Console.Write("*");
                                                        }
                                                        else
                                                        {
                                                            //checks if string is null or empty
                                                            if (!string.IsNullOrEmpty(secondCheckPassword.ToString()))
                                                            {
                                                                //remove last character
                                                                firstCheckPassword.RemoveAt(secondCheckPassword.Length - 1);

                                                                int pos = Console.CursorLeft;
                                                                //moves cursor to the left
                                                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                                //replace star character
                                                                Console.Write(" ");
                                                                //move cursor to the left
                                                                Console.SetCursorPosition(pos - 1, Console.CursorTop);
                                                            }
                                                        }
                                                    }
                                                    while (newKeyEntry.Key != ConsoleKey.Enter);

                                                    if (Security.ComparePasswords(firstCheckPassword, secondCheckPassword))
                                                    {
                                                        changedPassword = firstCheckPassword;
                                                        changingPassword = false;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                        Console.WriteLine("\n" + "                              Password must match. Please try again.");
                                                        Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                    }
                                                }
                                                User currentUser = Security.GetUser();
                                                currentUser.password = changedPassword;

                                                UserPage.UpdateUsers(users);

                                                Console.Clear();
                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                Console.WriteLine("\n" + "                         Password Updated. press enter to continue.");
                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                Console.ReadKey();
                                                Console.Clear();

                                                UserPage.PrintAccountInfo();
                                                break;
                                            case "logout":
                                                //resets user so no one can access the information
                                                User newUser = new User();
                                                Security.SetUser(newUser);

                                                userTransactionInProgress = false;
                                                userLoggedIn = false;

                                                Console.Clear();
                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                Console.WriteLine("\n" + "                    You have been logged out. Press enter to continue.");
                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                Console.ReadKey();
                                                Console.Clear();
                                                break;
                                            default:
                                                Console.Clear();
                                                Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                                Console.WriteLine("\n" + "                         Error: Invalid anwser try again. press enter to continue.");
                                                Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                                Console.ReadKey();
                                                Console.Clear();

                                                UserPage.PrintAccountInfo();
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    userLoggedIn = false;

                                    Console.Clear();
                                    Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                    Console.WriteLine("\n" + "               Your account is frozen. Contact administration. Press enter to continue.");
                                    Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                            } 
                            else
                            {
                                bool userCorrectAnwser = false;

                                //if login is incorrect it will ask if user would like to switch roles or exit application
                                while (!userCorrectAnwser) {
                                    Console.Write("\n" + "Would you like to exit application or switch roles?(exit, continue, or switch) ");

                                    string exitAnwser = Console.ReadLine().ToLower().Trim();

                                    switch (exitAnwser)
                                    {
                                        case "exit":
                                            userCorrectAnwser = true;
                                            userLoggedIn = false;
                                            closeApp = true;
                                            break;
                                        case "continue":
                                            userCorrectAnwser = true;
                                            break;
                                        case "switch":
                                            Console.Clear();
                                            userCorrectAnwser = true;
                                            userLoggedIn = false;
                                            break;
                                        default:
                                            Console.Clear();
                                            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
                                            Console.WriteLine("\n" + "                               Error: Invalid anwser try again.");
                                            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
                                            break;
                                    }     
                                }
                            }
                        }
                        break;
                    case "exit":
                        closeApp = true;
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
            Console.Clear();
            Console.WriteLine("\n\n----------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\n" + "                          You have exited the application. Press enter to exit.");
            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------");
            Console.ReadKey();
        }
    }
}
