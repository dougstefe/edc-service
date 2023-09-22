namespace Edc.Core.SharedContext.ValueObjects;

public class Image {
    protected Image() {}
    public Image(string url)
    {
        Url = url;
    }

    public string Url { get; } = String.Empty;
}