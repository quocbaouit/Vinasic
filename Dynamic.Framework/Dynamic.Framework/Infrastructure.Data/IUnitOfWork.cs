using System.Data.Entity;

namespace Dynamic.Framework.Infrastructure.Data
{
    public interface IUnitOfWork<out T> where T : DbContext
    {
        T DataContext { get; }

        void Commit();
    }
}
