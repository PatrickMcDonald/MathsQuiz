namespace MathsQuizApp.Models;

public record ErrorViewModel(string RequestId)
{
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
