using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                string dateProcess = splittedData[dateIndex];
                string[] data = dateProcess.Split('%');
                string downloadDate = data[0];
                string actionPerformed = data[1];
                Console.WriteLine(formattedSpace + (i+1) + ". "+ "You "+ actionPerformed + " " + bookTitle + " on " + downloadDate);
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



        public void SaveUserData(List<string> dataList)
        {
            //saves a book at a time
            int userNameIndex = 0;
            int fileNameIndex = 1;
            
            List<string> userHistory = new List<string>();
            string userName = dataList[userNameIndex];
            string fileName = dataList[fileNameIndex];
            string binFile = userName + ".bin";
            BinaryFormatter bf = new BinaryFormatter();

            //Check if the previous record exist
            //deserialize the previous records and add the new record to it
            try
            {
                if (File.Exists(binFile))
                {
                    try
                    {
                        userHistory = GetUserData(userName);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        userHistory.Add(fileName);
                        List<string> distinctData = userHistory.Distinct().ToList();
                        FileStream file = File.Create(binFile);
                        bf.Serialize(file, distinctData);
                        file.Close();
                    }
                }
                else
                {
                    List<string> newRecord = new List<string>
                    {
                        dataList[fileNameIndex]
                    };
                    FileStream file = File.Create(binFile);
                    bf.Serialize(file, newRecord);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void DownloadRequest(string message)
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


        public string ValidateUserChoice(string number)
        {
            string download = "1";
            string upload = "2";
            while (number != download && number != upload)
            {
                Console.Write("\nPlease enter a 1 or 2: ");
                string answer = Console.ReadLine();
                number = answer;
            }
            return number;
        }

        public void DisplayResponse(string response)
        {
            Book book = new Book();
            if (response.ToLower() == userOption.yes.ToString())
            {
                book.BookDisplaySearch();
            }
            else
            {
                string question = "\nWould you like to Upload a book? (yes/no): ";
                UploadRequest(question);
            }
        }

        public void UploadRequest( string question)
        {
            Book book = new Book();
            string answer;
            do
            {
                 answer = GetUserResponse(question);
                if (answer.ToLower() == userOption.yes.ToString())
                {
                    book.StartUpload();
                    question = "\nWould you like to Upload another book (yes/no): ";
                }
                else
                {
                    Console.WriteLine("\nHope to see you next time, Bye!!!");
                   
                }

            } while (answer.ToLower() != userOption.no.ToString());
           
        }

    }
}
