using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class Product : BaseEntity<long>
    {
        public string Name { get; set; } = default!;
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public long UserId { get; set; }
    }
}
