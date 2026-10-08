public class PostReactionDto
{
    public Guid PostId { get; set; }

    public bool IsLiked { get; set; }

    public int LikeCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsBookmarked { get; set; }

    public int ShareCount { get; set; }
}