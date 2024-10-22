using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T:ModelBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);
            
        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);
           
        public void Delete(T entity)
           =>_dbContext.Set<T>().Remove(entity);
            
        public async Task<T> GetAsync(int id)
        {
            /// var T = _dbContext.Ts.Where(D => D.Id== id).FirstOrDefault();  
            ///  if (T is null)
            ///      _dbContext.Ts.Where(D => D.Id == id).FirstOrDefault(); 
            ///
            ///  return T;

            return await _dbContext.Set<T>().FindAsync(id);

            //  return _dbContext.Find<T>(id); 
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            ///if (typeof(T) == typeof(Employee))
            ///{
            ///    return (IEnumerable<T>)_dbContext.Emplpoyees.AsNoTracking().Include(E => E.Department).ToList();
            ///}
            /// return _dbContext.Set<T>().AsNoTracking().ToList();

            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _dbContext.Emplpoyees.AsNoTracking().Include(E => E.Department).ToListAsync();    
            }
            return await _dbContext.Set<T>().AsNoTracking().AsNoTracking().ToListAsync();
        }

    }
}
