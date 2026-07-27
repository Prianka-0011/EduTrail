public class PollResultDto
{
    public Guid PollId { get; set; }

    public int TotalVotes { get; set; }
    public bool IsCurrentUserVoted { get; set; }
    public List<PollOptionResultDto> Options { get; set; } = new();
}

public class PollOptionResultDto
{
    public Guid Id { get; set; }

    public string OptionText { get; set; } = string.Empty;

    public int VoteCount { get; set; }

    public double Percentage { get; set; }

    public bool IsSelectedByCurrentUser { get; set; }
}