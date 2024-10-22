using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T> where T: ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync(); // Because it will return List Of T
        Task<T> GetAsync(int id);
        Task AddAsync(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
