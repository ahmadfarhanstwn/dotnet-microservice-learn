namespace Play.Catalogue.Service.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}