using System;
using System.Collections.Generic;

namespace LibraryManagement
{
    internal class Program
    {
        public static string globalUser;
        public const string formattedSpace = "  ";
        static void Main(string[] args)
        {
            bool userExist = false;
            int zero = 0;
            Book book = new Book();
            User libraryUser = new User();
            string userQuestion = "";
            const string nameRequest = "\nPlease enter your unique username:";

            Console.WriteLine("\tWelcome to Hucknall public library, \nyou can download any book from the available list of books!");

            string userName = libraryUser.ReadString(nameRequest).ToLower();
            globalUser = userName;
            string[] allUsers = libraryUser.GetAllUsers();
            foreach (string user in allUsers)
            {
                if (userName == user)
                {
                    userExist = true;
                }
            }
            if (!userExist)
            {
                Console.WriteLine(formattedSpace + "\nHello " + userName);
                libraryUser.SaveUser(userName);
            }
            else
            {
                // recall the history of the user here and continue
                //deserialize the user record at this point
                Console.WriteLine(formattedSpace + "\nWelcome back " + userName);
                try
                {
                    string displayMessage = "\nYou previosly downloaded the following books\n";
                    libraryUser.DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception)
                {
                    Console.WriteLine(formattedSpace + "\nSorry, I could not fetch your records, the file could be missing/corrupted");
                }
            }
            userQuestion = "\nWould you like to download another book: ";


            book.DisplayBooks();
            Dictionary<string, Book> allBooks = book.GetAllBooks();
            int bookCount = allBooks.Count;
            if (bookCount > zero)
            {
                libraryUser.UserRequest(userQuestion);
            }
            Console.ReadLine();
        }
    }
}
