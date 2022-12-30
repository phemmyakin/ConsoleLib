using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryManagement
{
    internal class Program
    {
        enum userOption
        {
            yes,
            no
        }
        public static string globalUser;
        public const string formattedSpace = "  ";
        static void Main(string[] args)
        {
            bool userExist = false;
            Book book = new Book();
            User bookUser = new User();
            string userQuestion = "";
            const string nameRequest = "\nPlease enter your unique username:";
            string[] allUsers = bookUser.GetAllUsers();
            Console.WriteLine("\tWelcome to Hucknall public library, \nyou can download any book from the available list of books!");    
            
            string userName = bookUser.ReadString(nameRequest).ToLower();
            bookUser.userName = userName;
            globalUser = userName;

            foreach (string user in allUsers)
            {
                if (userName == user)
                {
                    userExist = true;
                }
            }
            if (!userExist)
            {
                //SaveUser(userName);
                bookUser.SaveUser(userName);
            }
            else
            {
                // recall the history of the user here and continue
                //deserialize the user record at this point
                Console.WriteLine(formattedSpace + "\nWelcome back " + userName);
                try
                {
                    string displayMessage = "\nYou previosly downloaded the following books\n";
                    bookUser.DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception )
                {
                    Console.WriteLine(formattedSpace + "\nSorry, I could not fetch your records, the file could be missing/corrupted");
                }
            }
            userQuestion = "\nWould you like to download another book: ";
            book.DisplayBooks();
            bookUser.UserRequest(userQuestion);
            Console.ReadLine();
        }
        
    }
}
