using System.Linq.Expressions;

namespace Aspose.Test.Storage
{
    public interface IStorage<T> where T : IEntity
    {
        Task<T> GetAsync(Guid id);
        Task SaveAsync(T value);
    }
}
