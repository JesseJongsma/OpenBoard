using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace Open_Board
{
    class GUIBuilder
    {
        protected static Users CurrentUser; // This is the current user in the program

        /// <summary>
        /// Writes a line of a specified character and length on the console
        /// </summary>
        /// <param name="character">Character to be repeated</param>
        /// <param name="amount">Amount of characters</param>
        /// <param name="writeLine">WriteLine() or Write()</param>
        protected void LineBuilder(char character, int amount, bool writeLine)
        {
            string line = "";
            for (int i = 0; i <= amount; i++)
            {
                line += character;
            }
            if (writeLine)
                Console.WriteLine(line);
            else
                Console.Write(line);
        }

        /// <summary>
        /// Draws a sleek banner on the screen
        /// </summary>
        /// <param name="messageBoardTitle">An optional messageboard title</param>
        protected void Banner(String messageBoardTitle = "")
        {
            // Check if the user has joined a board
            ConsoleColor UsernameColour = ConsoleColor.Cyan; // Standard colour for the username
            if (CurrentUser.messageboard != null)
                UsernameColour = (CurrentUser.messageboard.Admin == CurrentUser.Username) ? ConsoleColor.Red : UsernameColour;

            LineBuilder('=', 80, true);
            Console.Write("Welcome: ");
            WriteColouredText(CurrentUser.Username, UsernameColour, false); // Write username after "Welcome: "
            LineBuilder(' ', 20, false);

            // Write the messageboard title if the user has joined a board
            if (CurrentUser.messageboard != null)
                WriteColouredText(CurrentUser.messageboard.MessageboardTitle, ConsoleColor.Green, false);

            Console.WriteLine();
            LineBuilder('=', 80, true);
        }

        /// <summary>
        /// Writes a text in colour
        /// </summary>
        /// <param name="inputString">The text to be written</param>
        /// <param name="consoleColor">The colour of the text</param>
        /// <param name="writeLine">WriteLine() or Write()</param>
        protected void WriteColouredText(String inputString, ConsoleColor consoleColor, bool writeLine)
        {
            Console.ForegroundColor = consoleColor;
            if (writeLine)
                Console.WriteLine(inputString);
            else
                Console.Write(inputString);
            Console.ResetColor();
        }

        /// <summary>
        /// Check if the input is valid to the given regex
        /// </summary>
        /// <param name="input">The text to be validated</param>
        /// <param name="allow">A regex string</param>
        /// <returns>Returns the correctness of the inputs</returns>
        protected static bool CheckInput(String input, String allow)
        {
            bool correct = false;
            if (Regex.IsMatch(input, allow))
            {
                correct = true;
                return correct;
            }
            return correct;
        }
    }
}
