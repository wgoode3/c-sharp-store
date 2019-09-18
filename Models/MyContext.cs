using System;
using Microsoft.EntityFrameworkCore;

namespace EasyBay.Models
{
    public class MyContext: DbContext 
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<Order> Orders {get;set;}
    }

}
