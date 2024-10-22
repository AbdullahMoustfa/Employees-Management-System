using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly ApplicationDbContext _dbContext;
         
        // Automatic Property
        public IEmployeeRepository EmployeeRepository { get; set; }            // Refere To Null 
        public IDepartmentRepoistory DepartmentRepoistory { get; set; }        // Refere To Null 
        public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR For Object For DbContext
        {
            EmployeeRepository = new EmployeeRepository(dbContext); 
            DepartmentRepoistory = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> Compelete()
          =>  await _dbContext.SaveChangesAsync();   

        public async ValueTask DisposeAsync()
           =>  await _dbContext.DisposeAsync();     // Close Connection

       
    }
}
