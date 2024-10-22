using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Signature For Property For Each and Every Repository Interface
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepoistory DepartmentRepoistory { get; set; }
        Task<int> Compelete();
        ValueTask DisposeAsync(); 

    }
}
