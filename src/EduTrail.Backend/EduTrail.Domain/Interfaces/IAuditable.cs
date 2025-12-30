namespace EduTrail.Domain.Interfaces
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; set; }
        Guid? CreatedById { get; set; }
        DateTimeOffset? UpdatedDate { get; set; }
        Guid? UpdatedById { get; set; }
    }
}