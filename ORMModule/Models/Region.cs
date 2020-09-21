using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORMModule.Models
{
  [Table("[dbo].[Region]")]
  public class Region
  {
    [Column("RegionID")]
    [Identity]
    [PrimaryKey]
    public int RegionId { get; set; }

    [Column("RegionDescription")]
    [NotNull]
    public string RegionDescription { get; set; }
  }
}
