using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Board
{
    class Messageboards : GUIBuilder
    {
        public String MessageboardTitle;
        public List<Messages> MessagesList = new List<Messages>();

        /// <summary>
        /// Creates a new messageboard
        /// </summary>
        /// <param name="messageboardTitle">The title of the new messageboard</param>
        public void createMessageboard(String messageboardTitle)
        {
            MessageboardTitle = messageboardTitle;
        }

        /// <summary>
        /// Adds a new message to the messageboard
        /// </summary>
        /// <param name="author">The author</param>
        /// <param name="title">The title</param>
        /// <param name="content">The content</param>
        /// <param name="tags">The tags</param>
        public void AddMessage(Users author, String title, String content, List<String> tags)
        {
            Messages message = new Messages(author, title, content, tags);
            MessagesList.Add(message);
            RetrieveMessages(); // refresh the screen upon adding
        }

        /// <summary>
        /// Retrieves messages from the MessagesList
        /// </summary>
        public void RetrieveMessages()
        {
            Banner(MessageboardTitle);
            foreach (Messages message in MessagesList)
            {
                ShowMessage(message);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Writes a message to the screen
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        public void ShowMessage(Messages message)
        {
            WriteColouredText(message.Title, ConsoleColor.DarkCyan, true);
            WriteColouredText(message.Content, ConsoleColor.White, true);
            if (message.Tags.Count != 0)
            {
                message.Tags.ForEach(x => WriteColouredText(x + " ", ConsoleColor.DarkGray, false));
                Console.WriteLine();
            }
            
            if(message.Author.Admin)
                WriteColouredText(message.Author.Username, ConsoleColor.Red, true);
            else
                WriteColouredText(message.Author.Username, ConsoleColor.Cyan, true);

            Console.WriteLine();
        }
    }

    class Messages
    {
        public Messages(Users author, String title, String content, List<String> tags)
        {
            Author = author;
            Title = title;
            Content = content;
            Tags = tags;
        }

        public Users Author { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public List<String> Tags;
    }
}
