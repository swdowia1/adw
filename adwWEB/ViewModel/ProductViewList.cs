namespace adwWEB.ViewModel
{
    public class ProductViewList
    {
        public int Id { get; set; }
        public string NameProduct { get; internal set; }
        public string ProductCategoryName { get; internal set; }
        public int? ProductSubcategory { get; internal set; }
        public string ProductSubcategoryName { get; set; }
    }
}