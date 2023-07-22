using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _genericService;

        public ProductsController(IGenericService<Product, ProductDto> genericService)
        {
            _genericService = genericService;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = _genericService.GetAllAsync();
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var result = await _genericService.AddAsync(productDto);
            return ActionResultInstance(result);
        }

        [HttpPut]
        public IActionResult UpdateProduct(ProductDto productDto)
        {
            var result = _genericService.Update(productDto);
            return ActionResultInstance(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var result = await _genericService.Remove(id);
            return ActionResultInstance(result);
        }


    }
}
