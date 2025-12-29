namespace EduTrail.Infrastructure.Data
{
    public class AuditEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? TableName { get; set; } = null!;
        public Guid? RecordId { get; set; }
        public string? Operation { get; set; } = null!;
        public DateTime? ChangeDate { get; set; }
        public Guid? ChangedById { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? ChangedProperties { get; set; }
    }


}