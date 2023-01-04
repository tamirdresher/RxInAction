namespace LINQExamples
{
    class Book
    {
        public Book(string name, int authorID)
        {
            this.Name = name;
            this.AuthorID = authorID;
        }

        public string Name { get; set; }
        //public string ISBN { get; set; }
        public int AuthorID { get; set; }
    }
}
