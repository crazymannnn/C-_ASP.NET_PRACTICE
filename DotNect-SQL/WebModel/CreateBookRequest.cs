using DotNect_SQL.Entities;

namespace DotNect_SQL.WebModel
{
    public class CreateBookRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
    }
}
