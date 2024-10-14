namespace DotNect_SQL.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
