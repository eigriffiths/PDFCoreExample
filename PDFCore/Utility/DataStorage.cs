using PDFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFCore.Utility
{
    public static class DataStorage
    {
        public static List<Employee> GetAllEmployees() =>
            new List<Employee>
            {
                new Employee { Name = "Emyr", LastName = "Griffiths", Age = 38, Gender = "Male"},
                new Employee { Name = "Manuela", LastName = "Griffiths", Age = 39, Gender = "Female"},
                new Employee { Name = "Mike", LastName = "Turner", Age = 67, Gender = "Male"},
                new Employee { Name = "Sarah", LastName = "Toni", Age = 27, Gender = "Female"},
                new Employee { Name = "Alan", LastName = "Jones", Age = 18, Gender = "Male"},
            };
    }
}
