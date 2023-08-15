using eShopSolutions.Application.Catalog.Product;
using eShopSolutions.ViewModels.Catalog.ProductImage;
using eShopSolutions.ViewModels.Catalog.Products;
using eShopSolutions.ViewModels.Catalog.Products.Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolutions.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductServices _publicProductServices;
        private readonly IManagerProductServices _managerProductServices;


        public ProductController(IPublicProductServices publicProductServices, IManagerProductServices managerProductServices) 
        {
            _publicProductServices = publicProductServices;
            _managerProductServices = managerProductServices;

        }



        [HttpGet]
           public async Task<IActionResult> GetAll()
           {
            var product = await _publicProductServices.GetAll();
             return Ok(product);
           }

        [HttpGet("publicPaging")]
        public async Task<IActionResult> GetPaging ([FromQuery]GetPublicProductPagingRequest  request)
        {
            var product = await _publicProductServices.GetAllByCategories(request);
            return Ok(product);
        }
        [HttpGet("{productId}/{language}")]
        public async Task<IActionResult> GetById(int productId, string language)
        {   
            var product = await _managerProductServices.GetById(productId, language);
            if(product == null)
            {
                return BadRequest("can't fint product");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           var productId = await _managerProductServices.Create(request); 
            if(productId == 0)
            {
                return BadRequest();  
            }
            var product = await _managerProductServices.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
            /* return Ok();*/
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _managerProductServices.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpDelete("productId")]
        public async Task<IActionResult> Delete(int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _managerProductServices.Delete(productId);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPatch("{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice( int productId, decimal newPrice)
        {
            var affectedResulf =await _managerProductServices.UpdatePrice(productId, newPrice);
            if(affectedResulf)
            { 
                return Ok();  

            }
            return BadRequest();    
        }
        [HttpGet("stock/{productId}/{newQuantity}")]
        public async Task<IActionResult> UpdateStock(int productId, int addQuantity)
        {
            var isSuccesResult = await _managerProductServices.UpdateStock(productId, addQuantity);
            if(isSuccesResult )
            {
                return Ok(isSuccesResult);  
            }
            return BadRequest();    
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage( int productId, [FromForm] ProductImageCreateRequest request)
        {
            var productImage = await _managerProductServices.AddImage(productId, request);
            if(productImage == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            var query = await _managerProductServices.UpdateImage(imageId, request);
            if (query == 0 )
            {
                return BadRequest(query);
            }
            return Ok();    
        }
        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var query = await _managerProductServices.DeleteImage(imageId);   
            if(query == 0)
            {
                return BadRequest(query);   
            }
            return Ok(query);

        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductPagingRequest request)
        {
            var query = await _managerProductServices.GetAllPaging(request);  
            return  Ok(query);  
        }

    }
}
