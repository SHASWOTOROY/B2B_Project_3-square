namespace MarketPlace.Models
{
    public class ProductVariation
    {
        public int TypeId { get; set; }
        public int ValueId { get; set; }
        public string Type {  get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
