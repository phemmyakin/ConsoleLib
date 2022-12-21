﻿using System;
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
        static void Main(string[] args)
        {
            bool userExist = false;
            string userQuestion = "";
            string[] allUsers = GetAllUsers();
            Console.WriteLine("  Welcome to HOE public library!!! ");
            const string welcomeNote = "\n  Please enter your unique username:";
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
                userQuestion = "\n  Would you like to download a book: ";
            }
            else
            {
                // recall the history of the user here and continue
                //deserialize the user record at this point
                Console.WriteLine("\n   Welcome back " + userName);
                try
                {
                    userQuestion = "\n  Would you like to download another book: ";
                    string displayMessage = "\n  You previosly downloaded the following books\n\n";
                    DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n  Sorry, I could not fetch your records, the file could be missing/corrupted");
                    //mainGuy(userQuestion);
                }               
            }
            UserRequest(userQuestion);
            Console.ReadLine();
        }

        public static void DisplayUserHistory(string userName, string displayMessage)
        {
            List<string> fetchedData = GetUserData(userName);

            Console.WriteLine(displayMessage);
            for (int i = 1; i < fetchedData.Count; i++)
            {
                Console.WriteLine(i + ". " + fetchedData[i]);
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
                message = "\nWould you like to download another book: ";
            } while (response != userOption.no.ToString());

        }
        public static string GetUserResponse(string message)
        {
            string result;
            result = ReadString(message);
            while (!ValidateResponse(result))
            {
                Console.WriteLine("Please enter 'Yes' or 'No'");
                result = ReadString(message);
            }
            return result;
        }
        public static string ReadString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
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
            var cc = File.OpenWrite(path);
            cc.Close();
            var dd = File.ReadAllLines(path);
            return dd;
        }

       public static  List<string> GetUserData( string userName)
        {
            //handle missing file here and catch the exception
            string fileName = userName + ".bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(fileName);
            try
            {
                file.Position = 0;
                List<string> array = (List<string>)bf.Deserialize(file);
                file.Close();
                return array;
            }
            catch (Exception ex)
            {
                file.Close();
                File.Delete(fileName);
               //FileStream newFile = File.Create(fileName);
               // newFile.Close();
                //Console.WriteLine(ex.Message);
                throw;
            }
         
        }
    }
}
