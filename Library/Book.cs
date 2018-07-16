using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }


        public Book(string title, string author, int pages)
        {
            this.Author = author;
            this.Title = title;
            this.Pages = pages;
        }
        public Book()
        {
            this.Author = "";
            this.Title = "";
            this.Pages = 0;
        }
    }
}
