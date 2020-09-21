using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORMModule;
using LinqToDB;
using System;
using ORMModule.Models;
using System.Linq;
using System.Runtime.InteropServices;

namespace ORMModuleTest
{
  [TestClass]
  public class QueryDemonstration
  {
    private NorthwindConnection connection = new NorthwindConnection("System.Data.SqlClient", @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Northwind;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");


    [TestMethod]
    public void Query1()
    {
      foreach (var product in connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier))
      {
        Console.WriteLine($"Product name: {product.ProductName}; Category: {product.Category?.CategoryName}; Supplier: {product.Supplier?.ContactName}");
      }
    }

    [TestMethod]

    public void Query2()
    {
      var query = from emp in connection.Employees
                  join employeeTer in connection.EmployeeTerritories on emp.EmployeeId equals employeeTer.EmployeeId into el
                  from w in el.DefaultIfEmpty()
                  join ter in connection.Territories on w.TerritoryId equals ter.TerritoryId into zl
                  from z in zl.DefaultIfEmpty()
                  join r in connection.Regions on z.RegionId equals r.RegionId into kl
                  from k in kl.DefaultIfEmpty()
                  select new { emp.FirstName, emp.LastName, Region = k };
      query = query.Distinct();

      foreach (var item in query.ToList())
      {
        Console.WriteLine($"First name: {item.FirstName}; Last name: {item.LastName}; Region: {item.Region?.RegionDescription}");
      }
    }

    [TestMethod]
    public void Query3()
    {
      var query = (from e in connection.Employees
                   join o in connection.Orders on e.EmployeeId equals o.EmployeeId into el
                   from w in el.DefaultIfEmpty()
                   join s in connection.Shippers on w.ShipperId equals s.ShipperId into zl
                   from z in zl.DefaultIfEmpty()
                   select new { e.EmployeeId, e.FirstName, e.LastName, z.CompanyName }).Distinct().OrderBy(t => t.EmployeeId);

      foreach (var item in query.ToList())
      {
        Console.WriteLine($"Employee: {item.FirstName} {item.LastName} Shipper: {item.CompanyName}");
      }

    }

    [TestMethod]
    public void Query4()
    {
      Employee employee = new Employee { FirstName = "TestName", LastName = "TestLastName" };

      try
      {
        connection.BeginTransaction();
        employee.EmployeeId = Convert.ToInt32(connection.InsertWithIdentity(employee));
        connection.Territories.Where(t => t.TerritoryDescription.Length <= 6)
            .Insert(connection.EmployeeTerritories, t => new EmployeeTerritory { EmployeeId = employee.EmployeeId, TerritoryId = t.TerritoryId });
        connection.CommitTransaction();
      }
      catch
      {
        connection.RollbackTransaction();
      }
    }

    [TestMethod]
    public void Query5()
    {
      int newCategory = connection.Products.Update(p => p.CategoryId == 1, pr => new Product
      {
        CategoryId = 2
      });

      Console.WriteLine(newCategory);
    }

    [TestMethod]
    public void Query6()
    {
      var orderDetails = connection.OrderDetails.LoadWith(od => od.Order)
                .Where(od => od.Order.ShippedDate == null).ToList();
      foreach (var orderDetail in orderDetails)
      {
        connection.OrderDetails.LoadWith(od => od.Product).Update(od => od.OrderId == orderDetail.OrderId && od.ProductId == orderDetail.ProductId,
            od => new OrderDetail
            {
              ProductId = connection.Products.First(p => !connection.OrderDetails.Where(t => t.OrderId == od.OrderId)
                              .Any(t => t.ProductId == p.ProductId) && p.CategoryId == od.Product.CategoryId).ProductId
            });
      }
    }
  }
}
