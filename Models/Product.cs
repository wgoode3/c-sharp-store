using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EasyBay.Models
{
    public class Product 
    {
        [Key]
        public int ProductId {get;set;}
        [Required(ErrorMessage="Price is required!")]
        [Range(1, 100000000, ErrorMessage="Price must be between $1.00 and $100,000,000.00")]
        public double Price {get;set;}
        [Required(ErrorMessage="Product Name is required")]
        [MinLength(2, ErrorMessage="Product Name must be 2 characters or longer")]
        public string Name {get;set;}
        [Required(ErrorMessage="Product Manufacturer is required")]
        [MinLength(2, ErrorMessage="Product Manufacturer must be 2 characters or longer")]
        public string Manufacturer {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User Seller {get;set;}
        public List<Order> Orders {get;set;}
    }
}