﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Syntax_Squad
{
    public class Login : User           //Anton SUT23
    {
       
       private const int MaxAttempts = 3;

        public int attempts = 0;
        private int userID;
        /// <summary>
        /// Method for login, checking user list for username and password
        /// </summary>
        public void LogIn()
        {

            List<User> AllUsers = User.AllUsers(); 

            do
            {
                Console.WriteLine("Welcome to Syntax Squad Bank!");
                Console.WriteLine("Username: ");
                string enterUsername = Console.ReadLine();

                Console.WriteLine("Password: ");
                string enterPassword = Console.ReadLine();

                User userTryLogin = AllUsers.Find(x => x.Name == enterUsername && x.Password == enterPassword);

                if (userTryLogin != null)
                {
                    Console.WriteLine("Login successful! press enter to continue to meny.");
                    Console.ReadKey();



                    if (userTryLogin.IsAdmin)
                    {
                        AdminMenu adminMenu = new AdminMenu();
                        adminMenu.ShowMenu(userTryLogin);
                        userTryLogin.IsLoggedIn = true;

                    }
                    else
                    {
                        UserMenu userMenu = new UserMenu();
                        userMenu.ShowMenu(userTryLogin);
                        userTryLogin.IsLoggedIn = true;

                    }
                    
                }
                else
                {
                    attempts++;
                    Console.WriteLine($"Incorrect username or password. Attempts left: {MaxAttempts - attempts}");
                }
            } while (attempts < MaxAttempts);
            Console.WriteLine("Maximum login attempts reached. Account locked.");



        } /// <summary>
          /// Method to get user ID
          /// </summary>
        public int GetUserID()
        {
            return userID;
        }
        /// <summary>
        /// Method for reseting attempts
        /// </summary>
        public void ResetAttempt()
        {
            attempts = 0;
        }

    }
}
