using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Data.Migrations;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepoistory
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext) // Ask CLR For Createing Object From D.bContext
            :base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
