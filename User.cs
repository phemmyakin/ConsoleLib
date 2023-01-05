using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryManagement
{
    public class User
    {
        public string userName;
        public const string formattedSpace = "  ";
        enum userOption
        {
            yes,
            no
        }

        public void SaveUser(string userName)
        {
            string path = "../../asset/userList.txt";
            FileStream fileStream = File.OpenWrite(path);
            fileStream.Close();
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(userName);
            writer.Close();
        }

        public void DisplayUserHistory(string userName, string displayMessage)
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
        public string[] GetAllUsers()
        {
            string fileDirectory = "../../asset";
            string path = "../../asset/userList.txt";
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            FileStream stream = File.OpenWrite(path);
            stream.Close();
            string[] users = File.ReadAllLines(path);
            return users;
        }

        public List<string> GetUserData(string userName)
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
            catch (Exception)
            {
                file.Close();
                File.Delete(fileName);
                throw;
            }

        }

        public void UserRequest(string message)
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

        public string GetUserResponse(string message)
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

        public string ReadString(string prompt)
        {
            string result;
            do
            {
                Console.Write(formattedSpace + prompt);
                result = Console.ReadLine();
            } while (result.Trim() == "");
            return result;
        }

        public bool ValidateResponse(string answer)
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


        public void DisplayResponse(string response)
        {
            Book book = new Book();
            if (response.ToLower() == userOption.yes.ToString())
            {
                book.DisplayBooks();
            }
            else
            {
                UploadRequest();
            }
        }

        public void UploadRequest()
        {
            Book book = new Book();
            string answer = GetUserResponse("\nWould you like to Upload a book: ");

            if (answer == userOption.yes.ToString())
            {
                book.StartUpload();
                Console.WriteLine("\nFile upload completed!!!");
            }
            else
            {
                Console.WriteLine("\nGood-Bye!!!");
                return;
            }
        }

    }
}
