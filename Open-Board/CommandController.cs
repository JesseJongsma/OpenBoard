using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Board
{
    class CommandController : GUIBuilder
    {
        private Messageboards MessageBoard  { get; set; }
        private List<Messageboards> MessageBoardList = new List<Messageboards>();

        /// <summary>
        /// Writes all messageboards to the terminal
        /// </summary>
        public void List()
        {
            bool occurrence = false;
            Console.WriteLine("==LISTING ALL MESSAGE BOARDS==");
            foreach (Messageboards messageboard in MessageBoardList)
            {
                WriteColouredText(messageboard.MessageboardTitle, ConsoleColor.Green, true);
                occurrence = true;
            }

            if(!occurrence)
                WriteColouredText("==NO BOARDS WERE FOUND==", ConsoleColor.Green, true);
        }

        /// <summary>
        /// Create a new messageboard
        /// </summary>
        /// <param name="messageBoardTitle">The title of the new messageboard</param>
        public void Create(String messageBoardTitle)
        {
            MessageBoard = new Messageboards();
            bool alreadyExists = false;

            // Loop through all boards and check if the new board already exists
            foreach (Messageboards messageBoard in MessageBoardList)
            {
                if (messageBoard.MessageboardTitle == messageBoardTitle)
                    alreadyExists = true;
            }

            // Create the board
            if (!alreadyExists)
            {
                if (CurrentUser.Admin)
                {
                    MessageBoardList.Add(MessageBoard);
                    MessageBoard.createMessageboard(messageBoardTitle);
                    Console.WriteLine("==CREATED {0}==", messageBoardTitle);
                }
                else
                    Console.WriteLine("==PERMISSION DENIED==");
            }
            else
                Console.WriteLine("=={0} ALREADY EXISTS==", messageBoardTitle);

            
        }

        /// <summary>
        /// Join the entered board
        /// </summary>
        /// <param name="messageboardTitle">The board title to be joined</param>
        public void Join(String messageboardTitle)
        {
            bool found = false;

            // Loop through board to check if the board actually exists
            foreach (Messageboards board in MessageBoardList)
            {
                if (board.MessageboardTitle == messageboardTitle)
                {
                    CurrentUser.messageboard = board;
                    CurrentUser.messageboard.RetrieveMessages();
                    found = true;
                }
            }

            if(!found)
                Console.WriteLine("==MESSAGEBOARD {0} COULDN't BE FOUND==", messageboardTitle);
            else
                Console.WriteLine("==JOINED {0}==", messageboardTitle);
        }

        /// <summary>
        /// Adds a new message to a messageboard
        /// </summary>
        /// <param name="title">Title of the message</param>
        /// <param name="content">Content of the message</param>
        /// <param name="tags">Tags of the message</param>
        public void AddMessage(String title, String content, List<String> tags)
        {
            // Check if the user has joined a group and then add the message
            if (CurrentUser.messageboard != null)
                CurrentUser.messageboard.AddMessage(CurrentUser, title, content, tags);
            else
                WriteColouredText("Please join a board first", ConsoleColor.Red, true);
        }

        /// <summary>
        /// Search messages by tags
        /// </summary>
        /// <param name="TagsToSearch">Tag to search</param>
        public void SearchTag(List<String> TagsToSearch)
        {
            // Loop through the boards
            foreach (Messageboards messageboard in MessageBoardList)
            {
                String messageBoardTitle = messageboard.MessageboardTitle;

                // Loop through the messages of each board
                foreach (Messages message in messageboard.MessagesList)
                {
                    // Loop through the tags of each message
                    foreach (String tag in message.Tags)
                    {
                        // Check if the tags correspond with the search tag
                        if (TagsToSearch.Contains(tag))
                        {
                            Console.Write("Message found in ");
                            WriteColouredText(messageBoardTitle, ConsoleColor.Green, false);
                            Console.WriteLine(".");
                            messageboard.ShowMessage(message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the correct screen for the user
        /// </summary>
        public void LoadBeginScreen()
        {
            if (CurrentUser.messageboard != null)
                CurrentUser.messageboard.RetrieveMessages();
            else
                ShowHelp(true);
        }

        /// <summary>
        /// Reload the screen
        /// </summary>
        public void Reload()
        {
            Console.Clear();
            LoadBeginScreen();
        }

        /// <summary>
        /// Show instructions to the user
        /// </summary>
        /// <param name="ShowBanner">wether to show the banner</param>
        public void ShowHelp(bool ShowBanner = false)
        {
            if(ShowBanner)
                Banner();

            LineBuilder('-', 80, true);
            Console.WriteLine("list: List all messageboards.");
            Console.WriteLine("reload: Clears the terminal.");
            Console.WriteLine("load: Prints all messages of the currently joined messageboard.");
            Console.WriteLine("create <Messageboard_title> [#tag1 ... #tag4]: Create a new messageboard");
            Console.WriteLine("join <Messageboard_title>: Join the specified messageboard");
            Console.WriteLine("add <Title> [#tag1 ... #tag4]: Add messages to a joined messageboard");
            Console.WriteLine("switch: Switch to a new or existing account");
            Console.WriteLine("search [#tag1 ... #tag4]: Search messages based on the tags");
            Console.WriteLine("exit: Exit a currently joined messageboard");
            Console.WriteLine("quit: Close the program");
            LineBuilder('-', 80, true);
        }
    }
}
