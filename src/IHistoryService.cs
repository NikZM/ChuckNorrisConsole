namespace GetBusy.ChuckNorrisApi;

public interface IHistoryService<T>
{
    void Append(T data);
    T? Next();
    T? Previous();
    bool HasNext { get; }
    bool HasPrevious { get; }
}