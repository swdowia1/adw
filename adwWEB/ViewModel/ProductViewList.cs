using adwWEB.Models;
using System.Collections.Generic;

namespace adwWEB.ViewModel
{
    public class ProductViewList
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int QuanityAll { get; set; }
       
        public string ProductCategoryName { get; internal set; }
        public int? ProductSubcategory { get; internal set; }
        public string ProductSubcategoryName { get; set; }
        public bool WithPhoto { get; set; }
        public List<ProductListPriceHistory> ProductListPriceHistory { get; set; }
        public List<Product> ProductRandom { get; set; }
    }
}