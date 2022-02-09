namespace IntelligenceReporting.Repositories;

public interface IRepository<T> where T: class
{
    Task Add(params T[] items);
    Task<T[]> GetAll();
    Task Update(params T[] items);
    Task Delete(params T[] items);
}