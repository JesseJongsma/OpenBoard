using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Board
{
    class LoginScreen : GUIBuilder
    {
        public List<Users> UsersList = new List<Users>();

        /// <summary>
        /// Prompt the user to login
        /// </summary>
        /// <returns>Returns true if the user has logged in with success</returns>
        public bool PromptUsername()
        {
            String username = "";
            String password = "";
            bool inValid = true;
            do
            {
                username = PromptInput("Please enter your username.");

                if (!CheckDuplicate(username) && username != "") // Create a new account
                {
                    password = PromptInput("Please enter your password.");
                    CurrentUser = new Users(username, password);
                    UsersList.Add(CurrentUser);

                    Console.Clear();
                    inValid = false;
                }
                else if (CheckDuplicate(username) && username != "") // Login to an existing account
                {
                    password = PromptInput("Please login with your password.");
                    Users login = Login(username, password);

                    if (login != null) // The user logged in to an existing account
                    {
                        CurrentUser = login;

                        Console.Clear();
                        inValid = false;
                    }
                    else
                    {
                        WriteColouredText("Invalid password.", ConsoleColor.Red, true);
                        inValid = true;
                    }
                }
                else
                {
                    WriteColouredText("Please enter a a valid username.", ConsoleColor.Red, true);
                    inValid = true;
                }
            }
            while ( inValid );
            return !inValid;
        }

        /// <summary>
        /// Tries to log in an user with the given crdentials
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns></returns>
        private Users Login(String username, String password)
        {
            Users userResult = null; // The user which could be accessed
            foreach (Users user in UsersList)
            {
                if (user.Username == username && user.Password == password)
                {
                    userResult = user;
                }
            }
            return userResult;
        }

        /// <summary>
        /// Prompt the user with a question
        /// </summary>
        /// <param name="question">Question to the user</param>
        /// <returns>Returns the input of the user if correct</returns>
        private String PromptInput(String question)
        {
            String input;
            LineBuilder('=', 80, true);
            Console.WriteLine(question);
            LineBuilder('=', 80, true);

            input = Console.ReadLine().ToString();
            input = (CheckInput(input, "^[a-zA-Z0-9 !$_-]\\w+$")) ? input : ""; // Check allowed chars
            input = (input.Length >= 4 && input.Length < 16) ? input : ""; // Check allowed length

            return input;
        }

        /// <summary>
        /// Check if the inputUsername already exists
        /// </summary>
        /// <param name="inputUsername">Input username to be checked</param>
        /// <returns>Returns true if user already exists</returns>
        private bool CheckDuplicate(String inputUsername)
        {
            foreach (Users user in UsersList)
            {
                if (user.Username == inputUsername)
                    return true;
            }
            return false;
        }
    }

    class Users
    {
        public Users(String username, String password)
        {
            Username = username;
            Password = password;
        }

        public String Username;
        public String Password;
        public Messageboards messageboard { get; set; }
    }
}
