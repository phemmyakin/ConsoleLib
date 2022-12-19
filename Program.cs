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
              string u = "";
        string[] allUsers = getUsers();
            string response;
            Console.WriteLine("Welcome to HOE public library!!! ");
            const string welcomeNote = "\nPlease enter your unique username:";
            string userName = readString(welcomeNote);
            globalUser = userName;
            //Person person = new Person();  
         
            //person.savePerson(userName);

            foreach (string user in allUsers)
            {
                if (userName == user)
                {
                    userExist = true;
                }
            }
            //var cc = person.getPerson();
            if (!userExist)
            {
                // ask for the user first name and last name
                saveUser(userName);
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
                    List<string> fetchedData = getUserData(userName);
                    Console.WriteLine("\nYou previously downloaded:  \n");
                    for (int i = 1; i < fetchedData.Count; i++)
                    {
                        Console.WriteLine(i + ". "+ fetchedData[i]);
                    }                    
                }
                catch (Exception ex)
                {

                    //string binFile = userName + ".bin";
                    
                    //FileStream file = File.Create(binFile);
                    //file.Close();
                    //Console.WriteLine(ex.Message);
                    Console.WriteLine("Sorry, I could not fetch your records, the file could be missing/corrupted");
                 
                }

                //List<string>fetchedData = getUserData(userName);
                //string previousBookCollected = fetchedData[1];

                //Console.WriteLine("\nYou previously collected "+previousBookCollected);
               
            }

            do
            {
                response = Intro("\nWould you like to download a book: ");
                try
                {
                    DisplayResponse(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (response != userOption.no.ToString());


          


            Console.ReadLine();
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
            //Dictionary<string, Book> allBooks;
            if (response.ToLower() == userOption.yes.ToString())
            {
                book.DisplayBooks();
            }
            //else
            //{
            //    response = readString("Would you like to return a book: ");
            //    while (!validateResponse(response))
            //    {
            //        Console.WriteLine("Please enter 'Yes' or 'No'");
            //        response = readString("Would you like to return a book: ");
            //    }
            //    if (response == userOption.yes.ToString())
            //    {

            //    }
            //    else
            //    {
            //        Console.WriteLine("How can i help you?");
            //    }
            //}

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


        //serilaize portion
        //static void saveUserData(List<string> array)
        //{
        //    string binFile = array + ".bin";
        //    BinaryFormatter bf = new BinaryFormatter();
        //    FileStream file = File.Create(binFile);
        //    bf.Serialize(file, array);
        //    file.Close();
        //}

       // [Serializable]
       public static  List<string> getUserData( string user)
        {
            //handle missing file here
            string fileName = user + ".bin";
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(fileName);
            try
            {            
                List<string> array = (List<string>)bf.Deserialize(file);
                file.Close();
                return array;
            }
            catch (Exception)
            {
                file.Close();
                throw;
            }
         
        }
    }

    //public class Person
    //{
    //    public string userName { get; set; }
       
    //    public string getPerson()
    //    {            
    //        return this.userName;
    //    }
    //    public void savePerson(string _userName)
    //    {
    //        this.userName = _userName;
            
    //    }
    //}
}
