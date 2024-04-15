namespace Niveau.Areas.Admin.Models.Products
{
    public class ProductListViewModel
    {
        
        public IEnumerable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


    }
}
