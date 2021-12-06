using CMCDataStore;

namespace CMCOnlineStoreApi
{
    public static class ProductExtension
    {
        public static ProductDto AsDto(this Product Product)
        {
            return new ProductDto { Id = Product.Id, Name = Product.Name, Description = Product.Description, Price = Product.Price,
            SmallImagePath = @"https://localhost:8888/api/Image/small/" + Product.ImageName,
            BigImagePath = @"https://localhost:8888/api/Image/big/" + Product.ImageName,
            };
        }
    }
}
