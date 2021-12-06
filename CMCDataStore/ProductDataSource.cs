using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;

namespace CMCDataStore
{
    public class ProductSource
    {
        private static int _pk;
        private static DataSet _dsProducts = new();

        private static DataSet Products
        {
            get
            {
                lock (_dsProducts)
                {
                    if (_dsProducts.Tables.Count > 0)
                        return _dsProducts;
                    var streamReader = new StreamReader(Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("CMCDataStore.ProductData.xml"));
                    _dsProducts = new DataSet();
                    var num = (int)_dsProducts.ReadXml(streamReader, XmlReadMode.ReadSchema);
                    if (_dsProducts.Tables["Product"].PrimaryKey == null ||
                        _dsProducts.Tables["Product"].PrimaryKey.Length == 0)
                        _dsProducts.Tables["Product"].PrimaryKey = new DataColumn[1]
                        {
                            _dsProducts.Tables["Product"].Columns["Id"]
                        };
                    var dataView = new DataView(_dsProducts.Tables["Product"], null, "Id DESC",
                        DataViewRowState.CurrentRows);
                    _pk = dataView.Count <= 0 ? 0 : Convert.ToInt32(dataView[0]["Id"]);
                    return _dsProducts;
                }
            }
        }

        public List<Product> GetAllData()
        {
            var ProductList = new List<Product>();
            if (Products != null)
            {
                foreach (DataRow row in (InternalDataCollectionBase)Products.Tables["Product"].Rows)
                    ProductList.Add(GetProduct(row));
            }

            return ProductList;
        }

        public Product GetDataById(int id)
        {
            if (Products != null)
            {                
                var dataRowArray = Products.Tables["Product"].Select("Id = " + id);
                if (dataRowArray.Length > 0)
                    return GetProduct(dataRowArray[0]);
            }

            return null;
        }

        private Product GetProduct(DataRow dr)
        {
            var Product = new Product();
            Product.Id = Convert.ToInt32(dr["Id"]);
            Product.Name = dr["Name"].ToString();
            Product.Description = dr["Description"].ToString();
            Product.Price = Convert.ToDecimal(dr["Price"]);
            Product.ImageName = dr["ImageName"].ToString();
            return Product;
        }
    }
}
