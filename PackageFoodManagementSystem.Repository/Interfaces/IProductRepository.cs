//using PackageFoodManagementSystem.Repository.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace PackageFoodManagementSystem.Repository.Interfaces
//{
//    public interface IProductRepository
//    {
//        IEnumerable<Product> GetAllProducts();
//        Product GetProductById(int id);
//        void AddProduct(Product product);
//        void Save();
//    }
//}

using PackageFoodManagementSystem.Repository.Models;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void Save();
}