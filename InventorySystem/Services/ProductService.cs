using InventorySystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventorySystem.Services
{
    public class ProductService : IProductService
    {
        InventoryDbContext _context; //object create for InventoryDbContext class
        public ProductService(InventoryDbContext _dbcontext)
        {
            _context = _dbcontext;
        }
        //public async Task<List<UserInformation>> GetAllUsers()
        //{
        //    if (_context != null)
        //    {
        //        return await _context.UserInformations.ToListAsync();
        //    }
        //    return null;
        //}
        public async Task<List<Product>> GetProducts()
        {
            if (_context != null)
            {
                return await _context.ProductDetailss.ToListAsync();
                //return await _context.ProductDetailss.Where(p => p.productId > 2).ToListAsync();
            }
            return null;
        }
        public async Task<Product> GetProduct(int? productId)
        {
            if (_context != null)
            {
                return await (from p in _context.ProductDetailss
                              where p.productId == productId
                              select new Product
                              {
                                  productId = p.productId,
                                  productName = p.productName,
                                  price = p.price,
                                  quantity = p.quantity,
                                  item_id = p.item_id

                              }).FirstOrDefaultAsync();
            }
            return null;
        }
        //Add Product
        public async Task<int> AddProduct(Product product)
        {
            if (_context != null)
            {
                await _context.ProductDetailss.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.productId;
            }
            return 0;
        }
        public async Task<int> DeleteProduct(int? productId)
        {
            int result = 0;
            if (_context != null)
            {
                var product = await _context.ProductDetailss.FirstOrDefaultAsync(x => x.productId == productId);
                if (product != null)
                {
                    _context.ProductDetailss.Remove(product);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
        public async Task UpdateProduct(Product product)
        {
            if (_context != null)
            {
                _context.ProductDetailss.Update(product);
                await _context.SaveChangesAsync();
            }
        }
       
        
    }
}