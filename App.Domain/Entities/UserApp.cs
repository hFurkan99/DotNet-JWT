using App.Domain.Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Entities
{
    public class UserApp : IdentityUser<long>, IAuditEntity
    {
        public string City { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public DateTime? DeletedAt { get; set;}
        public bool IsActive { get; set;}
        public bool IsDeleted { get; set;}
        public string? CreatedBy { get; set;}
        public string? UpdatedBy { get; set;}
        public string? DeletedBy { get; set;}
    }
}
