using System;
using Higgs.Mbale.EF.Context;
using Higgs.Mbale.EF.Repository;

namespace Higgs.Mbale.EF.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : IDbContext
    {
        IRepository<TEntity> Get<TEntity>() where TEntity : class;
        int SaveChanges();
    }
}
