namespace MarketPlace.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstLine { get; set; } = string.Empty;
        public string SecondLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
