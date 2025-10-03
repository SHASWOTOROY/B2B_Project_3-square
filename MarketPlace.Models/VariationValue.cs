using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class VariationValue : IAuditableEntity
    {
        public int Id { get; set; }
        public int VariationTypeId { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public VariationType? VariationType { get; set; }
    }
}
