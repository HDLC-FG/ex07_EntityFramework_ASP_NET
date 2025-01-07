namespace ApplicationCore.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T?> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
