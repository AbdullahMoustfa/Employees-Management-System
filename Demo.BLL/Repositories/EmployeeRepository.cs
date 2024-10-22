using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        
        public EmployeeRepository(ApplicationDbContext dbContext) // Ask CLR For Createing Object From DbContext
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Emplpoyees.Where(E => E.Address.ToLower() == address.ToLower());   
        }

        public IQueryable<Employee> SearchByName(string name)
          => _dbContext.Emplpoyees.Where(E => E.Name.ToLower().Contains(name));
    }
}
