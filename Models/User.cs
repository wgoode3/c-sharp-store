using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EasyBay.Models
{
    public class User 
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="First Name is required")]
        [MinLength(2, ErrorMessage="First Name must be 2 characters or longer")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="Last Name is required")]
        [MinLength(2, ErrorMessage="Last Name must be 2 characters or longer")]
        public string LastName {get;set;}
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage="Invalid Email")]
        public string Email {get;set;}
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Required(ErrorMessage="Confirm Password is required")]
        [Compare("Password", ErrorMessage="Confirm Password must match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
        public List<Product> Inventory {get;set;}
        public List<Order> Orders {get;set;}
    }
}