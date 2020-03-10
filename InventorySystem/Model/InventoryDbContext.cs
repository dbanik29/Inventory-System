using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Model
{
    public class InventoryDbContext : DbContext
    {
     
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Product> ProductDetailss { get; set; }
        public virtual DbSet<User> UserInformations { get; set; }

}
}
