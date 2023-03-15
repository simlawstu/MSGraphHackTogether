namespace SendSummarizedEmailToTeams.Abstractions
{
    public interface IFactory<T>
    {
        T Build();
    }
}