using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskCrud.Data;
using TaskCrud.DTO_s.Product;
using TaskCrud.Model;

namespace TaskCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAsync()
        {
            var allProducts=await context.Products.ToListAsync();
            var allProductsDto = allProducts.Adapt<IEnumerable <GetAllProductDto>>();
            if (allProductsDto is null) 
            {
                return NotFound();
            }
            return Ok(allProductsDto);
        }
        [HttpGet("ById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product=await context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            var productDto=product.Adapt<GetAllProductDto>();
            return Ok(productDto);
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateAsync(ProductDto productDtos,
            [FromServices] IValidator<ProductDto>validator)
        {
            var validationResult=validator.Validate(productDtos);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => new
                {
                    Field = error.PropertyName,
                    Error = error.ErrorMessage
                }));
            }
           
            var productDto=productDtos.Adapt<Product>();
            await context.Products.AddAsync(productDto);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(int id,UpdateProductDto updateProduct)
        {
            var product = await context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;

            await context.SaveChangesAsync();
            return Ok(product);

        }
        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            var product=await context.Products.FindAsync(id);
            if (product is null)
            {
                return NotFound();
            }
             context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
