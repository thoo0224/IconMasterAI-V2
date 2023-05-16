namespace IconMasterAI.Core.Entities;

// TODO: Create 'CreatedAt'
public class Icon
{
    public required string Id { get; set; }
    public required string Url { get; set; }
    public required string RawPrompt { get; set; }
    public required string FinalPrompt { get; set; }

    public static Icon Create(string id, string url, string rawPrompt, string finalPrompt)
        => new()
        {
            Id = id,
            Url = url,
            RawPrompt = rawPrompt,
            FinalPrompt = finalPrompt
        };
}
