namespace DotNect_SQL.Entities
{
    public class BorrowHistory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PeopleId { get; set; }
        public Book Book { get; set; }
        public People People { get; set; }
    }
}
