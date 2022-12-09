namespace GetBusy.ChuckNorrisApi;

public interface IDataProvider<T> {
    Task<T?> get();
}