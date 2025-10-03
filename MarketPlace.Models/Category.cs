using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Category : IAuditableEntity
    {
        public int Id { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Category? ParentCategory { get; set; }
    }
}
