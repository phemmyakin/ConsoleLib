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
            string download = "1";
            const string nameRequest = "\nPlease enter your unique username:";

            Console.WriteLine("\tWelcome to HOE public library. \nYou can download any book from the available list of books  or upload a book!");
           

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
                    //string displayMessage = "\nYou previosly downloaded the following books\n";
                    string displayMessage = "\nYou previosly performed the following operations\n";
                    libraryUser.DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception)
                {
                    Console.WriteLine(formattedSpace + "\nSorry, I could not fetch your records, the file could be missing/corrupted");
                }
            }
            Console.Write("\nTo download a book, Press 1 \nTo upload a book, Press 2: ");
            string answer = Console.ReadLine();
            string option = libraryUser.ValidateUserChoice(answer);
            if (option == download)
            {
                //Start download process
                Dictionary<string, Book> allBooks = book.GetAllBooks();
                book.DisplayBooks();             

                int bookCount = allBooks.Count;
                if (bookCount > zero)
                {
                    book.BorrowBook(allBooks);
                    userQuestion = "\nWould you like to download another book? (yes/no): ";
                    libraryUser.DownloadRequest(userQuestion);
                }                
            }
            else
            {
                //start upload process
                
                book.StartUpload();
                string uploadQue = "\nWould you like to upload another book? (yes/no): ";
                libraryUser.UploadRequest(uploadQue);
            }
            Console.ReadLine();
        }
    }
}
