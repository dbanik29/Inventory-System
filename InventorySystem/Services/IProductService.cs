using System;
using System.Linq;
using InventorySystem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventorySystem.Services
{
    public interface IProductService
    {
       // Task<List<UserInformation>> GetAllUsers();
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(int? productId);
        Task<int> AddProduct(Product product);
        Task<int> DeleteProduct(int? productId);
        Task UpdateProduct(Product product);
    }
}