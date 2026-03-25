using EBCaseStudy.Models;
using EBCaseStudy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EBCaseStudy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IApiService _apiService;

        public ProductsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string category = null, string sortBy = null)
        {
            var products = await _apiService.GetProductsAsync(pageNumber, pageSize, category, sortBy);
            ViewBag.Category = category;
            ViewBag.SortBy = sortBy;
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _apiService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddYears(-1);
            var priceHistory = await _apiService.GetPriceHistoryAsync(id, startDate, endDate);

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                PriceHistory = priceHistory
            };

            return View(viewModel);
        }
    }
}
