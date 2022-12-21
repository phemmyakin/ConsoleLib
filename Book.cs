using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryManagement
{
    public class Book
    {
        public string title;
        public string author;

        public Book(string _title, string _author)
        {
            this.author = _author;
            this.title = _title;
        }

        public Book()
        {

        }
        public List<Book> FetchBooks(string response)
        {
            string[] splittedBooks;
            Book book;
            List<Book> books = new List<Book>();
            string filePath = "../../asset/books.txt";
            // ArrayList bookList = new ArrayList();
            string[] allbooks = File.ReadAllLines(filePath);
            foreach (string bookstring in allbooks)
            {
                splittedBooks = bookstring.Split(',');
                book = new Book(splittedBooks[0], splittedBooks[1]);
                books.Add(book);

            }
            return books;
        }

        public Dictionary<string, Book> GetAllBooks()
        {
            string[] splittedBooks;
            Book book;
            Dictionary<string, Book> dictionaryOfBooks = new Dictionary<string, Book>();
            string filePath = "../../asset/books.txt";
            string[] content = File.ReadAllLines(filePath);
            foreach (string bookstring in content)
            {
                if (bookstring.Length > 0)
                {
                    splittedBooks = bookstring.Split(',');
                    book = new Book(splittedBooks[0], splittedBooks[1]);

                    //Only add books with title and authors to the list
                    if (book.author.Length > 0 && book.title.Length > 0)
                    {
                        dictionaryOfBooks.Add(book.title, book);
                    }
                }
            }
            return dictionaryOfBooks;
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

        public static string readBookString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                while (result.Length < 3)
                {
                    Console.WriteLine("\nPlease enter a word with at least 3 characters!!!");
                    Console.Write(prompt);
                    result = Console.ReadLine();

                }
            } while (result.Trim() == "");
            return result;
        }
        public void BorrowBook(Dictionary<string, Book> books)
        {
            string samplefileUrl = "https://www.africau.edu/images/default/sample.pdf";
            string booktitle = readBookString("\nPlease search for a book by its title: ");

            //get a list of all keys and convert to lower case
            List<string> bookKeys = new List<string>(books.Keys);
            List<string> matchingRecords = new List<string>();
            Dictionary<string, Book> matchedBook = new Dictionary<string, Book>();
            for (int i = 0; i < bookKeys.Count; i++)
            {
                if (bookKeys[i].ToLower().Contains(booktitle.ToLower()))
                {
                    matchingRecords.Add(bookKeys[i]);
                }
            }
            if (matchingRecords.Count > 0)
            {
                if (matchingRecords.Count > 1)
                {
                    Console.WriteLine("\nHere are the corresponding matches to your search: ");
                    for (int i = 0; i < matchingRecords.Count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + books[matchingRecords[i]].title + " by " + books[matchingRecords[i]].author);
                        Book newBook = new Book
                        {
                            author = books[matchingRecords[i]].author,
                            title = books[matchingRecords[i]].title
                        };
                        matchedBook.Add(books[matchingRecords[i]].title, newBook);
                    }
                    //makes the user select one book at a time
                    BorrowBook(matchedBook);
                }
                else
                {
                    Console.WriteLine("\nThis is the book");
                    Console.WriteLine("\n" + (1) + ". " + books[matchingRecords[0]].title + " by " + books[matchingRecords[0]].author);
                    Console.Write("\npress 1 to download or any key to view all available books again: ");
                    string response = Console.ReadLine();
                    string positive = "1";
                    if (response == positive)
                    {
                        string user = Program.globalUser;
                        downloadFile(user,samplefileUrl, books[matchingRecords[0]].title);
                        Console.WriteLine("\n" + books[matchingRecords[0]].title + " downloaded successfully.");
                        //Console.WriteLine("\n You have successfully downloaded the following books");
                        string displayMessage = "\n You have successfully downloaded the following books";
                        Program.DisplayUserHistory(user, displayMessage);

                    }
                    else
                    {
                        DisplayBooks();
                    }
                }

            }
            else
            {
                Console.WriteLine("\nThere are no matches for your request");
                DisplayBooks();

            }
        }


        /*
         code to download file from  gotten from :
         https://stackoverflow.com/questions/307688/how-to-download-a-file-from-a-url-in-c
        assessed 15/12/2022
         */
        private static void downloadFile(string user, string url, string file)
        {           
            string filename = file + ".pdf";
            WebClient cln = new WebClient();
            cln.DownloadFile(url, filename);
            List<string> userData = new List<string>
            {
                user,
                file
            };
            //userData.Add(user);
            //userData.Add(file);
            saveUserData(userData);
        }

        static void saveUserData(List<string> dataList)
        {
            List<string> allData = new List<string>();
            string userName = dataList[0];
            string fileName = dataList[1];           
            string binFile = userName + ".bin";
            BinaryFormatter bf = new BinaryFormatter();

            //Check if the previous record exist
            //deserialie the previous records and add the new record to it

            try
            {
                if (File.Exists(binFile))
                {
                    try
                    {
                        allData = Program.GetUserData(userName);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        allData.Add(fileName);
                        List<string> distinctData = allData.Distinct().ToList();
                        FileStream file = File.Create(binFile);
                        bf.Serialize(file, distinctData);
                        file.Close();
                    }
                }
                else
                {
                    FileStream file = File.Create(binFile);
                    bf.Serialize(file, dataList);
                    file.Close();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayBooks()
        {
            Dictionary<string, Book> allBooks;
            allBooks = this.GetAllBooks();
            if (allBooks.Count > 0)
            {
                Console.WriteLine("\n  These are the available books with authors, you can download one book at a time\n");

                //Order in ascending 
                List<Book> bookList = new List<Book>(allBooks.Values).OrderBy(x => x.title).ToList();

                Console.WriteLine("    Book Title                  |    Author");
                for (int i = 0; i < bookList.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + bookList[i].title + "   by " + bookList[i].author);
                }
                BorrowBook(allBooks);
            }
            else
            {
                Console.WriteLine("\n  There are no books available right now");
                DisplayBooks();
            }
        }
    }
}
