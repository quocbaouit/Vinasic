using System.Data.Entity;

namespace Dynamic.Framework.Infrastructure.Data
{
    public interface IUnitOfWork<T> where T : DbContext
    {
        T DataContext { get; }

        void Commit();
    }
}
