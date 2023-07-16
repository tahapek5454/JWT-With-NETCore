using AuthServer.Core.Repositories;
using AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task AddAsync(T entity)
            => await Table.AddAsync(entity);

        public IQueryable<T> GetAllAsync()
            => Table.AsQueryable();

        public async Task<T?> GetByIdAsync(int id)
            => await Table.FindAsync(id);


        public void Remove(T entity)
         =>  Table.Remove(entity);

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
            => Table.Where(predicate);
    }
}
