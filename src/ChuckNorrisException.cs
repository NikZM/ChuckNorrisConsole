namespace GetBusy.ChuckNorrisApi;

[Serializable]
public class ChuckNorrisException : Exception
{
    public ChuckNorrisException() { }

    public ChuckNorrisException(string message)
        : base(message) { }

    public ChuckNorrisException(string message, Exception inner)
        : base(message, inner) { }

}