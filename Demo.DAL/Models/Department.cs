using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department : ModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> Employees { get; set; }  =new HashSet<Employee>(); 
    
    }
}
