using ControllerLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll()
        {
            var items = _service.GetAll();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProductDto> GetById(int id)
        {
            var product = _service.GetById(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<ProductDto> Create([FromBody] CreateProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required.");
            }

            var created = _service.Create(request.Name, request.Price);

          

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
