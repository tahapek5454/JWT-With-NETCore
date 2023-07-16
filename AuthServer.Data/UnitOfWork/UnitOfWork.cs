using AuthServer.Core.UnitOfWork;
using AuthServer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Commit()
            => _appDbContext.SaveChanges();

        public async Task CommitAsync()
            => await _appDbContext.SaveChangesAsync();
    }
}
