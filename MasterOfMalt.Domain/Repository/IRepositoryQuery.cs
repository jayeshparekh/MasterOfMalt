namespace MasterOfMalt.Domain.Repository
{
    public interface IRepositoryQuery<T>
    {
        T Execute();
    }
}
