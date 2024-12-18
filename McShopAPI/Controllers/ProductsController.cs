using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using McShopAPI.Data;
using McShopAPI.Models;


namespace McShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MusicShopDbContext _context;

        public ProductsController(MusicShopDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    price = p.Price,
                    category = p.Category.Name.ToLower(), // Преобразуем в нижний регистр, как в примере
                    image = p.Image
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}