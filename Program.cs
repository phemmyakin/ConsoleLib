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
        static void Main(string[] args)
        {
            bool userExist = false;
            string userQuestion = "";
            string[] allUsers = getUsers();
            string response;
            Console.WriteLine("Welcome to HOE public library!!! ");
            const string welcomeNote = "\nPlease enter your unique username:";
            string userName = readString(welcomeNote);
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
                saveUser(userName);
                userQuestion = "\nWould you like to download a book: ";
                //do all other stuff
                //try
                //{
                //    DisplayResponse(response);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    //recovery code should be to start over again
                //}
            }
            else
            {
                // recall the history of the user here and continue
                //deserilaize the use record at this point
                Console.WriteLine("\nWelcome back " + userName);
                try
                {
                    userQuestion = "\nWould you like to download another book: ";
                    string displayMessage = "\n You previosly downloaded the following books\n\n";
                    DisplayUserHistory(userName, displayMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nSorry, I could not fetch your records, the file could be missing/corrupted");
                    //mainGuy(userQuestion);
                }               
            }
            mainGuy(userQuestion);
            Console.ReadLine();
        }

        public static void DisplayUserHistory(string userName, string displayMessage)
        {
            List<string> fetchedData = getUserData(userName);

            Console.WriteLine(displayMessage);
            for (int i = 1; i < fetchedData.Count; i++)
            {
                Console.WriteLine(i + ". " + fetchedData[i]);
            }
        }

        public static void mainGuy(string message)
        {
            string response;
            do
            {
                response = Intro(message);
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
        public static string Intro(string message)
        {
            string result;
            result = readString(message);
            while (!validateResponse(result))
            {
                Console.WriteLine("Please enter 'Yes' or 'No'");
                result = readString(message);
            }
            return result;
        }
        public static string readString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result.Trim() == "");
            return result;
        }

        public static bool validateResponse(string answer)
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

        static void saveUser(string userName)
        {
            string path = "../../asset/userList.txt";
            var cc = File.OpenWrite(path);
            cc.Close();
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(userName);
            writer.Close();
        }
        static string[] getUsers()
        {
            string path = "../../asset/userList.txt";
            var cc = File.OpenWrite(path);
            cc.Close();
            var dd = File.ReadAllLines(path);
            return dd;
        }

       public static  List<string> getUserData( string user)
        {
            //handle missing file here and catch the exception
            string fileName = user + ".bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(fileName);
            try
            {            
                List<string> array = (List<string>)bf.Deserialize(file);
                file.Close();
                return array;
            }
            catch (Exception ex)
            {
                file.Close();
                //Console.WriteLine(ex.Message);
                throw;
            }
         
        }
    }
}
