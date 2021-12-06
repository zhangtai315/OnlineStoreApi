using CMCDataStore;

namespace CMCOnlineStoreApi
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(string searchText = null);
    }
}
