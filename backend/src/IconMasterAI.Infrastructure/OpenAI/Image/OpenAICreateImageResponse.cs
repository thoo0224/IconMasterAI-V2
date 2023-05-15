namespace IconMasterAI.Infrastructure.OpenAI.Image;

internal sealed class OpenAICreateImageResponse
{
    public required ulong Created { get; set; }
    public required Image[] Data { get; set; }

    public class Image
    {
        public string Url { get; set; }
    }
}
