using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORMModule.Models
{
  [Table("[dbo].[EmployeeTerritories]")]
  public class EmployeeTerritory
  {
    [Column("EmployeeID")]
    [NotNull]
    public int EmployeeId { get; set; }

    [Column("TerritoryID")]
    [NotNull]
    public int TerritoryId { get; set; }

    [Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(Models.Employee.EmployeeId))]
    public Employee Employee { get; set; }

    [Association(ThisKey = nameof(TerritoryId), OtherKey = nameof(Models.Territory.TerritoryId))]
    public Territory Territory { get; set; }
  }
}
