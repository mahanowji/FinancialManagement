namespace CmsKit.Domain.Abstractions
{
    public interface IEntity
    {
    }

    public abstract class BaseEntity<T> : IEntity
    {

        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            CreatedBy = 0;
        }

        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public bool Deleted { get; set; }

    }

    public class AuditEntity<T> : BaseEntity<T>
    {
        protected AuditEntity()
        {
            ModifiedDate = null;
            ModifiedBy = null;
        }


        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class BaseEntity : BaseEntity<int>
    {
    }

    public class AuditEntity : AuditEntity<int>
    {
    }
}
