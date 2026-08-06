using System.ComponentModel.DataAnnotations;
using EduTrail.Domain.Interfaces;
namespace EduTrail.Domain.Entities
{
    public class PostUserAction
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;

        public Guid UserId { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsPinned { get; set; }

        public bool IsRead { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? ArchivedDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public DateTime? FavoritedDate { get; set; }
    }

}