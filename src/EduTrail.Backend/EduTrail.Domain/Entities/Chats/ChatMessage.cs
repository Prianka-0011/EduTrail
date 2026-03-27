using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduTrail.Domain.Interfaces;

namespace EduTrail.Domain.Entities
{
    public class ChatMessage : IAuditable
    {
        [Key]
        public Guid Id { get; set; }

        // Sender
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        // Receiver
        [Required]
        [ForeignKey(nameof(Receiver))]
        public Guid ReceiverId { get; set; }
        public User Receiver { get; set; }

        [Required]
        [ForeignKey(nameof(CourseOffering))]
        public Guid CourseOfferingId { get; set; }
        public CourseOffering CourseOffering { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTimeOffset? CreatedDate { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
