using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
    class Program
    {
        public static object BookToEdit { get; private set; }

        static void Main(string[] args)
        {

            GetUserChoice();
            Console.ReadKey();
        }

        public static void GetUserChoice()
        {
            Console.WriteLine("Welcome To The Library! Would you like to Add a book (A)? Delete a book (B), Edit a book (C)? Get all the books (D)? Or Get one book (E)?");
            Console.WriteLine("\n A. Add Book \n B. Delete Book \n C. Edit Book \n D. Get All Books \n E. Get One Book");
            string UserChoice = Console.ReadLine();
            switch (UserChoice)
            {
                case "A":
                    Console.WriteLine("What Title would you like to add to the library? ");
                    string Title = Console.ReadLine();
                    Console.WriteLine("What is the author's name?");
                    string Author = Console.ReadLine();
                    Console.WriteLine("How many pages does the book have?");
                    int Pages = 0;
                    int.TryParse(Console.ReadLine(), out Pages);
                    DisplayBooks(AddBook(Title, Author, Pages));
                    GetUserChoice();
                    break;

                case "B":
                    Console.WriteLine("Which book would you like to delete?");
                    DisplayBooks(DeleteBook(Console.ReadLine(), GetBooks()));
                    GetUserChoice();
                    break;

                case "C":
                    Console.WriteLine("Which book Book would you like to edit?");
                    string BookToEdit = Console.ReadLine();
                    Console.WriteLine("What is the new title?");
                    string NewBookTitle = Console.ReadLine();
                    Console.WriteLine("What is the new author's name?");
                    string NewAuthorName = Console.ReadLine();
                    Console.WriteLine("How many pages does the edited book have?");
                    int NewNumPages = 0;
                    int.TryParse(Console.ReadLine(), out Pages);
                    DisplayBooks(EditBook(BookToEdit, NewBookTitle, NewAuthorName, NewNumPages));
                    GetUserChoice();
                    break;

                case "D":
                    Console.WriteLine("Here are all the books currently in the library.");
                    DisplayBooks(GetBooks());
                    GetUserChoice();
                    break;

                case "E":
                    Console.WriteLine("What book would you like to see?");
                    DisplayBooks(GetBook(Console.ReadLine(), GetBooks()));
                    GetUserChoice();
                    break;

                default:
                    Console.WriteLine("I did not understand you choice, please try again.");
                    GetUserChoice();
                    break;
            }

        }

 

        public static List<Book> AddBook(string title, string author, int Pages)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            Book book = new Book();
            book.Title = title;
            book.Author = author;

            db.Execute("Insert INTO Books (Title, Author, Pages) VALUES ( @Title, @Author, @Pages)", new { @Title = title, @Author = author, Pages });

            return GetBooks();
        }
        public static List<Book> GetBooks()
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            List<Book> books = new List<Book>();
            books = db.Query<Book>("Select * from Books").ToList();
            return books;
        }

        public static List<Book> DeleteBook(string BookName, List<Book> books)
        {

            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            db.Execute("Delete from Books Where Title = @BookName", new { BookName });
            return GetBooks();
        }

        public static void DisplayBooks (List<Book> books)
        {
            foreach (Book book in books)
            {
                Console.WriteLine("\n Title:" + book.Title + "\n + Author:" + book.Author + "\n Pages:" + book.Pages);
            }
        }
        public static List<Book> GetBook(string BookName, List<Book> books)
        {
            return books.Where(x => x.Title == BookName).ToList();
        }


        public static List<Book> EditBook(string BookToEdit, string NewBookTitle, string NewAuthor, int NewNumPages)
        {
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);

            db.Execute("Update Books Set Title = @NewBookTitle, Author = @NewAuthor, Pages = @NewNumPages Where Title = @BookToEdit",
                new { @BookToEdit = BookToEdit, @NewBookTitle = NewBookTitle, @NewAuthor = NewAuthor, @NewNumPages = NewNumPages });

            return GetBooks();
        }

    }
}
