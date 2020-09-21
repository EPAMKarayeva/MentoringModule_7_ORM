﻿using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORMModule.Models
{
  [Table("[dbo].[Suppliers]")]
  public class Supplier
  {
    [Column("SupplierID")]
    [PrimaryKey]
    [Identity]
    public int SupplierId { get; set; }

    [Column("CompanyName")]
    [NotNull]
    public string CompanyName { get; set; }

    [Column("ContactName")]
    public string ContactName { get; set; }

    [Association(ThisKey = nameof(SupplierId), OtherKey = nameof(Models.Product.SupplierId), CanBeNull = true)]
    public IEnumerable<Product> Products { get; set; }
  }
}
