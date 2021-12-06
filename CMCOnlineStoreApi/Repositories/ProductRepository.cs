using CMCDataStore;
using Microsoft.Extensions.Caching.Memory;

namespace CMCOnlineStoreApi
{
    public class ProductRepository : IProductRepository
    {
        private ProductSource ProductSource;
        private IMemoryCache memoryCache;
        private const string memoryCacheKey = "ThisKeyCanBeConfigured";

        public ProductRepository(IMemoryCache memoryCache)
        {
            ProductSource = new ProductSource();
            this.memoryCache = memoryCache;
        }

        //in real production, we should design the system to get data asynchronously
        public async Task<Product> GetProductByIdAsync(int Id, bool useCachedData = true)
        {
            //the requirement doesn't tell if GetDataById is also costly, so by default, use Cached all data which should contain any requested id.
            if (useCachedData)
            {
                var Product = GetSetCachedData(Id);
                if (Product.Any())
                    return Product.First();
                else
                    return null;
            }

            return ProductSource.GetDataById(Id);
        }

        //in real production, we should design the system to get data asynchronously
        public async Task<IEnumerable<Product>> GetProductsAsync(string? searchText = null)
        {
            List<Product> data = GetSetCachedData();
            if (searchText == null)
                return data;

            return data
                 .Where(x => x != null && Helper.SearchProperties<Product>(x, searchText));

        }

        public List<Product> GetSetCachedData(Nullable<int> Id = null)
        {
            List<Product> Products;
            if (!memoryCache.TryGetValue(memoryCacheKey, out Products))
            {
                Products = ProductSource.GetAllData();
                //Calling the production third party data source (ProductSource.GetAllData()) is costly and data only gets updated every 24 hours
                var memoryCacheEntryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) };
                memoryCache.Set<List<Product>>(memoryCacheKey, Products, memoryCacheEntryOptions);
            }

            if (Id.HasValue)
                return Products.Where(a => a.Id == Id.Value).ToList();
            return Products;
        }
    }
}
