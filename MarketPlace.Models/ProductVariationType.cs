namespace MarketPlace.Models
{
    public class ProductVariationType
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public IList<ProductVariationValue> Values { get; set; } = new List<ProductVariationValue>();
    }
}
