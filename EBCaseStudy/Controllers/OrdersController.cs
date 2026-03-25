using EBCaseStudy.Data;
using EBCaseStudy.Extensions;
using EBCaseStudy.Models;
using EBCaseStudy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBCaseStudy.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IApiService _apiService;

        public OrdersController(ApplicationDbContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string filter = "New")
        {
            ViewBag.Filter = filter;
            IQueryable<Order> orders = _context.Orders.OrderByDescending(o => o.OrderDate);

            if (Enum.TryParse<OrderStatus>(filter, out var status))
            {
                orders = orders.Where(o => o.Status == status);
            }

            return View(await orders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dictionary<string, int> quantities)
        {
            var orderToUpdate = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);

            if (orderToUpdate == null)
            {
                return NotFound();
            }

            foreach (var quantity in quantities)
            {
                if (int.TryParse(quantity.Key.Replace("quantity-", ""), out int orderItemId))
                {
                    var item = orderToUpdate.OrderItems.FirstOrDefault(oi => oi.Id == orderItemId);
                    if (item != null)
                    {
                        if (quantity.Value > 0)
                        {
                            item.Quantity = quantity.Value;
                        }
                        else
                        {
                            _context.OrderItems.Remove(item);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reorder(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var cart = new Cart();
            foreach (var item in order.OrderItems)
            {
                var priceHistory = await _apiService.GetPriceHistoryAsync(item.ProductId, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                var latestPrice = priceHistory.OrderByDescending(p => p.Date).FirstOrDefault()?.Value ?? 0;

                cart.Items.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = latestPrice,
                    Quantity = item.Quantity
                });
            }

            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index", "Cart");
        }


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
