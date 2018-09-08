using System;
using System.Collections.Generic;

namespace Open_Board
{
    class Program : GUIBuilder
    {
        private static LoginScreen loginScreen;
        private static bool _tooManyTags = false; // Set to true when the user inputs too many hashtags

        static void Main(string[] args)
        {
            // Let the user login and save the username
            loginScreen = new LoginScreen();
            loginScreen.PromptUsername();

            // Show the user a menu of options
            CommandController commandController = new CommandController();
            commandController.ShowHelp(true);

            CommandLoop(commandController);
        }

        /// <summary>
        /// This method keeps listening for commands
        /// </summary>
        /// <param name="commandController">The instance of the CommandController class</param>
        private static void CommandLoop(CommandController commandController)
        {
            String input = "";
            String error = "Something went wrong, please try again.";

            do
            {
                // Reset variables foreach new input
                String command = "", param = "";
                String[] commandArray = null;
                List<String> tags = new List<String>();

                // Begin listening to user input
                input = Console.ReadLine();

                // If CheckInput() fails, vars won't be set and the user will have to re-input
                if (CheckInput(input, "^[a-zA-Z0-9 _!?#]+$"))
                {
                    commandArray = input.Split(' ');

                    command = (commandArray.Length != 0) ? commandArray[0] : "";
                    param = (commandArray.Length > 1) ? commandArray[1] : "";
                }

                if (command == "list") // List all messageboards
                    commandController.List();

                else if (command == "create" && param != "") // Create new messageboard
                    commandController.Create(param);

                else if (command == "join" && param != "") // Join a messageboard
                    commandController.Join(param);

                else if (command == "exit" && commandArray.Length == 1) // Exit the current board
                    CurrentUser.messageboard = null;

                else if (command == "switch") // Switch user
                {
                    if (loginScreen.PromptUsername())
                    {
                        commandController.LoadBeginScreen();
                    }

                }
                else if (command == "search" && TagExtractor(commandArray).Count > 0) // Search tags
                {
                    commandController.SearchTag(TagExtractor(commandArray));
                }
                else if (command == "add" && param != "" && param[0] != '#') // Add message to board
                {
                    // If the input of title is correct the title will equal the input, else null
                    String title = (CheckInput(commandArray[1], "^[a-zA-Z0-9 _!?]+$")) ? commandArray[1] : null;
                    Console.WriteLine("Please add your message.");
                    String content = Console.ReadLine(); // Listen to user input
                    if (title != null && content != null && !_tooManyTags)
                        commandController.AddMessage(title, content, TagExtractor(commandArray));
                    else
                        Console.WriteLine("Please check your input.");
                }
                else if (command == "help") // Show commands on screen
                    commandController.ShowHelp();

                else if (command == "reload") // Clear the screen
                    commandController.Reload();
                else
                    Console.WriteLine(error);

                _tooManyTags = false;
            } while (input != "quit");
        }

        /// <summary>
        /// Extracts hashtags from a string array
        /// </summary>
        /// <param name="input">Array of strings</param>
        /// <returns>Returns a list of tags</returns>
        private static List<String> TagExtractor(String[] input)
        {
            List<String> tags = new List<String>();
            foreach (string tag in input)
            {
                if (tag[0] == '#')
                {
                    tags.Add(tag);
                }
            }
            if (tags.Count > 4)
                _tooManyTags = true;

            return tags;
        }
    }
}
