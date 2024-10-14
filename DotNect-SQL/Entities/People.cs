namespace DotNect_SQL.Entities
{
    public class People
    {   
        public int Id { get; set; }
        public String Name { get; set; }
        public int Age { get; set; }
        public List<BorrowHistory> BorrowBooks { get; set; }
    }
}
