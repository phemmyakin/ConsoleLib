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
            string userQuestion = "";
            
            string[] allUsers = GetAllUsers();
            Console.WriteLine("\tWelcome to HOE public library!!! ");

            const string welcomeNote = "\nPlease enter your unique username:";
            string userName = ReadString(welcomeNote);
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
                // ask for the user first name and last name
                SaveUser(userName);
                //userQuestion = "\n  Would you like to download a book: ";
            }
            else
            {
                // recall the history of the user here and continue
                //deserialize the user record at this point
                Console.WriteLine(formattedSpace + "\nWelcome back " + userName);
                try
                {
                   // userQuestion = "\n  Would you like to download another book: ";
                    string displayMessage = "\nYou previosly downloaded the following books\n";
                    DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(formattedSpace+"\nSorry, I could not fetch your records, the file could be missing/corrupted");            
                }               
            }
            userQuestion = "\nWould you like to download another book: ";
            book.DisplayBooks();
            UserRequest(userQuestion);
            Console.ReadLine();
        }

        public static void DisplayUserHistory(string userName, string displayMessage)
        {

            List<string> fetchedData = GetUserData(userName);

            Console.WriteLine(formattedSpace + displayMessage);
            for (int i = 0; i < fetchedData.Count; i++)
            {
                string[] splittedData = fetchedData[i].Split('@');
                string bookTitle = splittedData[0];
                string downloadDate = splittedData[1];
                Console.WriteLine(formattedSpace + (i+1) + ". " + bookTitle + " on "+ downloadDate);
            }
        }

        public static void UserRequest(string message)
        {
            string response;
            do
            {
                response = GetUserResponse(message);
                try
                {
                    DisplayResponse(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (response != userOption.no.ToString());

        }
        public static string GetUserResponse(string message)
        {
            string result;
            result = ReadString(message);
            while (!ValidateResponse(result))
            {
                Console.WriteLine(formattedSpace + "Please enter 'Yes' or 'No'");
                result = ReadString(message);
            }
            return result;
        }
        public static string ReadString(string prompt)
        {
            string result;
            do
            {
                Console.Write(formattedSpace + prompt);
                result = Console.ReadLine();
            } while (result.Trim() == "");
            return result;
        }

        public static bool ValidateResponse(string answer)
        {
            if (answer.ToLower() == userOption.yes.ToString())
            {
                return true;
            }
            else if (answer.ToLower() == userOption.no.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static void DisplayResponse(string response)
        {
            Book book = new Book();
            if (response.ToLower() == userOption.yes.ToString())
            {
                book.DisplayBooks();
            }
            else
            {
                Console.WriteLine("This platform is for borrowing books");
            }
        }

        static void SaveUser(string userName)
        {
            string path = "../../asset/userList.txt";
            var cc = File.OpenWrite(path);
            cc.Close();
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(userName);
            writer.Close();
        }
        static string[] GetAllUsers()
        {
            string path = "../../asset/userList.txt";
            FileStream stream = File.OpenWrite(path);
            stream.Close();
            string[] users = File.ReadAllLines(path);
            return users;
        }

       public static  List<string> GetUserData( string userName)
        {
            //handle missing file here and catch the exception
            string fileName = userName + ".bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(fileName);
            try
            {
                List<string> userData = (List<string>)bf.Deserialize(file);
                file.Close();
                return userData;
            }
            catch (Exception ex)
            {
                file.Close();
                File.Delete(fileName);
                throw;
            }
         
        }
    }
}
