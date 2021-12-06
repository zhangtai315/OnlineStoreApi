namespace CMCOnlineStoreApi
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public string SmallImagePath { get; set; }
        public string BigImagePath { get; set; }

    }
}