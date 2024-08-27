using IdentityService.Domain.Abstraction;

namespace IdentityService.Domain.Entities.@base
{
    public abstract class Entity:IEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = "Admin";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }


    }
}
