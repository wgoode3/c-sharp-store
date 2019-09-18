using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EasyBay.Models
{
    public class Order 
    {
        [Key]
        public int OrderId {get;set;}
        public int CustomerId{get;set;}
        public User Customer{get;set;}
        public int ProductId{get;set;}
        public Product Item {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}