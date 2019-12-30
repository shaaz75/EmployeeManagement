using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id=1,Name="John", Email="John@niksaj.com", Department="IT"},
                new Employee(){ Id=2,Name="Smith", Email="Smith@niksaj.com", Department="HR"},
                new Employee(){ Id=3,Name="Rhey", Email="Rhey@niksaj.com", Department="IT"}
            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeList;
        }
    }
}
