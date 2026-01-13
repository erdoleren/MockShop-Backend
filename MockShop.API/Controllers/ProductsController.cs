using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockShop.Domain.Entities;
using MockShop.Infrastructure.Persistance;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products (Tüm ürünleri listele)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products); // HTTP 200 döner
    }

    // POST: api/products (Yeni ürün ekle)
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product); // HTTP 201 döner
    }
}
