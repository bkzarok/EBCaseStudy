using EBCaseStudy.Data;
using EBCaseStudy.Extensions;
using EBCaseStudy.Models;
using EBCaseStudy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EBCaseStudy.Controllers
{
    public class CartController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ApplicationDbContext _context;

        public CartController(IApiService apiService, ApplicationDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _apiService.GetProductAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var priceHistory = await _apiService.GetPriceHistoryAsync(productId, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                var latestPrice = priceHistory.OrderByDescending(p => p.Date).FirstOrDefault()?.Value ?? 0;

                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = latestPrice,
                    Quantity = quantity
                });
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = HttpContext.Session.Get<Cart>("Cart");
            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    if (quantity > 0)
                    {
                        cartItem.Quantity = quantity;
                    }
                    else
                    {
                        cart.Items.Remove(cartItem);
                    }
                    HttpContext.Session.Set("Cart", cart);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            return UpdateCart(productId, 0);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order model)
        {
            var cart = HttpContext.Session.Get<Cart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    UserName = model.UserName,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.New
                };

                foreach (var item in cart.Items)
                {
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        PriceAtTimeOfOrder = item.Price
                    });
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("Cart");

                return RedirectToAction("Confirmation", new { id = order.Id });
            }

            return View(model);
        }

        public IActionResult Confirmation(int id)
        {
            return View(id);
        }
    }
}
