using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORMModule.Models
{
  [Table("[dbo].[Employees]")]
  public class Employee
  {
    [Column("EmployeeID")]
    [PrimaryKey]
    [Identity]
    public int EmployeeId { get; set; }

    [Column("LastName")]
    public string LastName { get; set; }

    [Column("FirstName")]
    public string FirstName { get; set; }
  }
}

