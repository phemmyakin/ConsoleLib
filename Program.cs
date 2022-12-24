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
            const string nameRequest = "\nPlease enter your unique username:";
            string[] allUsers = GetAllUsers();
            Console.WriteLine("\tWelcome to Hucknall public library, where you download any book of choice from the available list of books!!! ");    
            
            string userName = ReadString(nameRequest).ToLower();
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
                SaveUser(userName);
            }
            else
            {
                // recall the history of the user here and continue
                //deserialize the user record at this point
                Console.WriteLine(formattedSpace + "\nWelcome back " + userName);
                try
                {
                    string displayMessage = "\nYou previosly downloaded the following books\n";
                    DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(formattedSpace + "\nSorry, I could not fetch your records, the file could be missing/corrupted");
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
                int titleIndex = 0;
                int dateIndex = 1;
                string[] splittedData = fetchedData[i].Split('@');
                string bookTitle = splittedData[titleIndex];
                string downloadDate = splittedData[dateIndex];
                Console.WriteLine(formattedSpace + (i + 1) + ". " + bookTitle + " on " + downloadDate);
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
                Console.WriteLine("This applicatiion is for downloading free books, \n Book upload will be availbale in version 2 ");
            }
        }

        public static void SaveUser(string userName)
        {
            string path = "../../asset/userList.txt";
            FileStream fileStream = File.OpenWrite(path);
            fileStream.Close();
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(userName);
            writer.Close();
        }
        public static string[] GetAllUsers()
        {
            string path = "../../asset/userList.txt";
            FileStream stream = File.OpenWrite(path);
            stream.Close();
            string[] users = File.ReadAllLines(path);
            return users;
        }

        public static List<string> GetUserData(string userName)
        {
            //handle missing file here and catch the exception
            //save the user record using the unique username of the user
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
