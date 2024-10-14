using DotNect_SQL.Entities;

namespace DotNect_SQL.WebModel
{
    public class GetAllAuthorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<AuthorResponse> Books { get; set; }
    }
}
