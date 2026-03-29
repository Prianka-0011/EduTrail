namespace EduTrail.Application.Chats
{
    public class ChatMessageDto
    {
        public Guid UserId { get; set; }        
        public Guid ReceiverId { get; set; }    
        public Guid CourseOfferingId { get; set; }
        public string UserName { get; set; }    
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
