namespace MathsQuiz
{
    public static class HttpResponseExtensions
    {
        public static Task RedirectTemporaryAsync(this HttpResponse response, string location)
        {
            response.Redirect(location, permanent: false);
            return Task.CompletedTask;
        }
    }
}
