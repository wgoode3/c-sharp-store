using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EasyBay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace EasyBay.Controllers
{
    public class HomeController : Controller
    {

        private MyContext context;
        
        public HomeController(MyContext mc)
        {
            context = mc;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
            }
            if(ModelState.IsValid)
            {
                var hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);
                context.Users.Add(user);
                context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return Redirect("/market");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userData)
        {
            User userInDb = context.Users.FirstOrDefault(u => u.Email == userData.LoginEmail);
            if(userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Email not found!");
            } 
            else
            {
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userData, userInDb.Password, userData.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Incorrect Password!");
                }
            }
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return Redirect("/market");
        }

        [HttpGet("market")]
        public IActionResult Market()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            if(HttpContext.Session.GetString("cart") == null)
            {
                ViewBag.CartSize = 0;
            }
            else
            {
                string[] cart = HttpContext.Session.GetString("cart").Split(",");
                ViewBag.CartSize = cart.Length;
            }
            ViewBag.Products = context.Products.Include(p => p.Seller);
            return View();
        }

        [HttpGet("product/new")]
        public IActionResult NewProduct()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost("product")]
        public IActionResult SellProduct(Product product)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                product.UserId = (int) UserId;
                context.Products.Add(product);
                context.SaveChanges();
                return Redirect("/market");
            }
            return View("NewProduct");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet("addtocart/{ProductId}")]
        public IActionResult AddToCart(int ProductId)
        {
            string cart = HttpContext.Session.GetString("cart");
            if(cart == null)
            {
                HttpContext.Session.SetString("cart", ProductId.ToString());
            }
            else
            {
                cart += $",{ProductId}";
                HttpContext.Session.SetString("cart", cart);
            }
            Console.WriteLine(HttpContext.Session.GetString("cart"));
            return Redirect("/market");
        }

        [HttpGet("checkout")]
        public IActionResult Checkout()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            string cart = HttpContext.Session.GetString("cart");
            if(cart == null)
            {  
                return Redirect("/market");
            }
            string[] productIds = HttpContext.Session.GetString("cart").Split(",");
            List<Product> products = new List<Product>();
            double total = 0d;
            foreach(var id in productIds)
            {
                Product aproduct = context.Products.FirstOrDefault(p => p.ProductId == Int32.Parse(id));
                products.Add(aproduct);
                total += aproduct.Price;
            }
            ViewBag.Products = products;
            ViewBag.Total = total;
            return View();
        }

        [HttpGet("buy")]
        public IActionResult Buy()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            string cart = HttpContext.Session.GetString("cart");
            if(cart == null)
            {  
                return Redirect("/market");
            }
            string[] productIds = HttpContext.Session.GetString("cart").Split(",");
            foreach(var id in productIds)
            {
                int ProductId = Int32.Parse(id);
                Order o = new Order(){
                    CustomerId = (int) UserId,
                    ProductId = ProductId
                };
                context.Orders.Add(o);
                context.SaveChanges();
            }
            return Redirect("/market");
        }

        [HttpGet("orders")]
        public IActionResult Orders()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            List<Order> Orders = context.Orders
                .Where(o => o.CustomerId == (int) UserId)
                .Include(o => o.Item)
                .ThenInclude(i => i.Seller)
                .OrderByDescending(o => o.Item.Price)
                .ToList();
            ViewBag.Orders = Orders;
            return View();
        }
    }
}
