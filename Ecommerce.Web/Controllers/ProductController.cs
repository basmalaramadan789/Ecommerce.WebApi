using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Specifications;
using Ecommerce.Web.Dtos;
using Ecommerce.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(
            IProductRepository productRepository,
            IGenericRepository<Product> productRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productRepo = productRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // CREATE: api/products
        [HttpPost]
        public async Task<ActionResult<ProductReturnDto>> CreateProduct(ProductCreateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Validate Product Type and Product Brand
            var productBrand = await _productRepo.GetByIdAsync(productDto.ProductBrandId);
            var productType = await _productRepo.GetByIdAsync(productDto.ProductTypeId);

            if (productBrand == null || productType == null)
            {
                return BadRequest("Invalid Product Brand or Product Type");
            }

            var product = _mapper.Map<ProductCreateDto, Product>(productDto);

            _productRepo.Add(product);
            await _unitOfWork.Complete(); // Ensure SaveChanges is called to persist data

            var productToReturn = _mapper.Map<Product, ProductReturnDto>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productToReturn);
        }

        // READ: api/products (List all products with pagination)
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithTypeBrandAndSubTypeSpec(productParams);
            var countSpec = new ProductWithFilterForCount(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);
            var products = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductReturnDto>>(products);

            return Ok(new Pagination<ProductReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        // READ: api/products/{id} (Get a product by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypeBrandAndSubTypeSpec(id);
            var product = await _productRepo.GetEntityWithSpec(spec);

            if (product == null)
                return NotFound();

            return Ok(_mapper.Map<Product, ProductReturnDto>(product));
        }

        // UPDATE: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productDto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            // Validate Product Type and Product Brand
            var productBrand = await _productRepo.GetByIdAsync(productDto.ProductBrandId);
            var productType = await _productRepo.GetByIdAsync(productDto.ProductTypeId);

            if (productBrand == null || productType == null)
            {
                return BadRequest("Invalid Product Brand or Product Type");
            }

            _mapper.Map(productDto, product);
            _productRepo.Update(product);
            await _unitOfWork.Complete();

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            _productRepo.Delete(product);
            await _unitOfWork.Complete();

            return NoContent();
        }

        // Extra Endpoints (for brands, types, subtypes)
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productRepository.GetProductBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productRepository.GetTypesAsync();
            return Ok(types);
        }

        [HttpGet("subtypes")]
        public async Task<ActionResult<IReadOnlyList<ProductSubType>>> GetProductSubTypes()
        {
            var subtypes = await _productRepository.GetSubTypesAsync();
            return Ok(subtypes);
        }
    }
}
