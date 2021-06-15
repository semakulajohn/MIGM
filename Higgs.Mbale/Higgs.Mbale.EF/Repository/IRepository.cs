using System.Linq;


namespace Higgs.Mbale.EF.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable();
        TEntity AddNew(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Detach(TEntity entity);
    }
}
