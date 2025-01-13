namespace Template.Domain.Models.Entity
{
    public abstract class Base
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now.ToUniversalTime();
        public DateTimeOffset? UpdatedAt { get; protected set; }
        public bool Deleted { get; protected set; } = false;

        public void UpdateMe()
            => UpdatedAt = DateTimeOffset.Now.ToUniversalTime();

        public void SoftDelete()
        {
            Deleted = true;
            UpdateMe();
        }
    }
}
