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
        public Dictionary<string, Book> GetAllBooks()
        {
            string[] splittedBooks;
            Book book;
            Dictionary<string, Book> dictionaryOfBooks = new Dictionary<string, Book>();
            string filePath = "../../asset/books.txt";
            FileStream fileStream = File.OpenWrite(filePath);
            fileStream.Close();
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


        public static string ReadBookString(string prompt)
        {
            string result;
            do
            {
                Console.Write(Program.formattedSpace + prompt);
                result = Console.ReadLine();
                //while (result.Length < 3)
                //{
                //    Console.WriteLine(Program.formattedSpace + "\nPlease enter a word with at least 3 characters!!!");
                //    Console.Write(prompt);
                //    result = Console.ReadLine();
                //}
            } while (result.Trim() == "");
            return result;
        }


        public int ValidateBookNumber(int value, int count)
        {
            int result;
            while (value <= 0 || value > count)
            {
                Console.Write("\nPlease enter a serial number that corresponds to a book number: ");
                string answer = Console.ReadLine();
                value = int.Parse(answer);
            }
            result = value;
            return result;

        }


        public void BorrowBook(Dictionary<string, Book> books)
        {
            int number = 0;
            int zero = 0;
            int one = 1;
            int result;
            string bookRequest = ReadBookString(Program.formattedSpace + "\nPlease search for a book by its title or serial number: ");

            bool success = int.TryParse(bookRequest, out number);
            //order in ascending
            List<string> bookTitles = new List<string>(books.Keys).OrderBy(i => i).ToList();
            if (success)
            {
                //Uses the serial number to search the book               
                try
                {
                    result = ValidateBookNumber(number, bookTitles.Count);
                    int bookIndex = result - one;
                    string bookTitle = bookTitles[bookIndex];
                    ProcessDownload(books, bookTitle);
                }
                catch (Exception)
                {
                    Console.WriteLine("\nYou have entered an invalid input!!!");
                    BorrowBook(books);
                }
            }
            else
            {
                //Uses the book title to search the list
                //get a list of all keys and convert to lower case

                List<string> matchingRecords = new List<string>();
                Dictionary<string, Book> matchedBook = new Dictionary<string, Book>();
                for (int i = 0; i < bookTitles.Count; i++)
                {
                    if (bookTitles[i].ToLower().Contains(bookRequest.ToLower()))
                    {
                        matchingRecords.Add(bookTitles[i]);
                    }
                }
                if (matchingRecords.Count > zero)
                {
                    if (matchingRecords.Count > one)
                    {
                        Console.WriteLine(Program.formattedSpace + "\nHere are the corresponding matches to your search: ");
                        for (int i = 0; i < matchingRecords.Count; i++)
                        {
                            Console.WriteLine(Program.formattedSpace + (i + one) + ". " + books[matchingRecords[i]].title + " by " + books[matchingRecords[i]].author);
                            Book newBook = new Book
                            {
                                author = books[matchingRecords[i]].author,
                                title = books[matchingRecords[i]].title
                            };
                            matchedBook.Add(books[matchingRecords[i]].title, newBook);
                        }
                        //user can only download one book at a time,
                        BorrowBook(matchedBook);
                    }
                    else
                    {
                        string bookTitle = books[matchingRecords[zero]].title;
                        ProcessDownload(books, bookTitle);
                    }
                }
                else
                {
                    Console.WriteLine(Program.formattedSpace + "\nThere are no matches for your request");
                    //DisplayBooks();
                    twoMethod();
                }
            }
        }

        public void ProcessDownload(Dictionary<string, Book> books, string title)
        {
            User libUser = new User();
            string samplefileUrl = "https://www.africau.edu/images/default/sample.pdf";

            Console.WriteLine(Program.formattedSpace + "\nYour searched result: ");
            Console.WriteLine(Program.formattedSpace + "\n" + (1) + ". " + books[title].title + " by " + books[title].author);
            Console.Write(Program.formattedSpace + "\npress 1 to download or any key to view all available books again: ");
            string response = Console.ReadLine();
            string positive = "1";
            if (response == positive)
            {
                string user = Program.globalUser;
                string datePattern = "dddd, dd MMMM yyyy hh:mm tt";
                string downloadDate = DateTime.Now.ToString(datePattern);
                DownloadFile(user, samplefileUrl, title + "@" + downloadDate +"%downloaded");
                Console.WriteLine(Program.formattedSpace + "\n" + books[title].title + " downloaded successfully, Please check your folder. \n");
                string displayMessage = "\n You have successfully performed the following \n";
                libUser.DisplayUserHistory(user, displayMessage);
            }
            else
            {
                Book book = new Book();
                //book.DisplayBooks();
                book.twoMethod();
            }
        }

        /*
 code to download file from  gotten from :
 https://stackoverflow.com/questions/307688/how-to-download-a-file-from-a-url-in-c
assessed 15/12/2022
 */
        private void DownloadFile(string user, string fileUrl, string file)
        {
            //split the file to separate the book title from the date
            User bookUser = new User();
            string[] newFile = file.Split('@');
            string filename = newFile[0] + ".pdf";
            //string downloadDate = newFile[1];
            WebClient client = new WebClient();
            client.DownloadFile(fileUrl, filename);
            List<string> userData = new List<string>
            {
                user,
                file
            };
            bookUser.SaveUserData(userData);
        }
         

        public void DisplayBooks()
        {
            Dictionary<string, Book> allBooks;
            User libUser = new User();
            allBooks = this.GetAllBooks();
            if (allBooks.Count > 0)
            {
                Console.WriteLine(Program.formattedSpace + "\nThese are the available books with authors, you can download one book at a time\n");
                Console.WriteLine(Program.formattedSpace + "S/N : BOOK TITLE\n");

                //Order in ascending 
                List<Book> bookList = new List<Book>(allBooks.Values).OrderBy(x => x.title).ToList();

                for (int i = 0; i < bookList.Count; i++)
                {
                    Console.WriteLine(Program.formattedSpace + (i + 1) + ". " + bookList[i].title + "   by " + bookList[i].author);
                }
               // BorrowBook(allBooks);
            }
            else
            {
                Console.WriteLine(Program.formattedSpace + "\nThere are no books available right now.!");
                string uploadQue = "\nWould you like to upload a book? (yes/no): ";
                libUser.UploadRequest(uploadQue);
            }
        }

        public void StartUpload()
        {
            User bookUser = new User();
            string filePath = "../../asset/books.txt";
            FileStream fileStream = File.OpenWrite(filePath);
            fileStream.Close();
            string title = ReadBookString("\nPlease enter the title of the book you want to upload: ");
            string author = ReadBookString("\nEnter the authors name: ");
            string content = title + "," + author;

            StreamWriter writer = new StreamWriter(filePath, true);
            writer.WriteLine(content);
            writer.Close();
            Console.WriteLine($"\n {title} by {author} upload completed!!!");

            string user = Program.globalUser;
            string datePattern = "dddd, dd MMMM yyyy hh:mm tt";
            string downloadDate = DateTime.Now.ToString(datePattern);
            string file = title + "@" + downloadDate + "%uploaded";

            List<string> userData = new List<string>
            {
                user,
                file
            };
            bookUser.SaveUserData(userData);

        }


        public void twoMethod()
        {

            Dictionary<string, Book> allBooks = GetAllBooks();
            DisplayBooks();
            BorrowBook(allBooks);
        }
    }
}
