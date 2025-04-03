using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class UserRefreshToken : BaseEntity<long>
    {
        public long UserId { get; set; }
        public string Code { get; set; } = default!;
        public DateTime ExpireDate { get; set; }
    }
}
