using System.Data.Entity;
using System.Data.Entity.Infrastructure;



namespace Higgs.Mbale.EF.Context
{
  public  interface IDbContext
    {

        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        DbContextConfiguration Configuration { get; }
        int SaveChanges();
        void Dispose();
    }
}
