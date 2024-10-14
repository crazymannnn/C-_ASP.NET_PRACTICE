namespace CQRS_TEST.Class
{
    public class AddProductCommand : IRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class GetProductQuery : IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}
