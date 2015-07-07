namespace LINQExamples
{
    class Book
    {
        public Book(string name, int authorID)
        {
            Name = name;
            AuthorID = authorID;
        }
        public string Name { get; set; }
        //public string ISBN { get; set; }
        public int AuthorID { get; set; }
    }
}