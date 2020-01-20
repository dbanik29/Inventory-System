using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Model
{
    [Table("ProductDetails")]
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public int item_id { get; set; }
    }
}
