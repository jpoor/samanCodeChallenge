using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saman.Backend.Business.Entity.Category;
using Saman.Backend.Business.Entity.Log;
using Saman.Backend.Business.Entity.Product;
using Saman.Backend.Business.Entity.Product.Rsx;
using Saman.Backend.Endpoint.Admin.baseClasses;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;
using Saman.Backend.Share.shareServices;

namespace Saman.Backend.Endpoint.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : baseController
    {
        private readonly Product_Service _productService;
        private readonly Category_Service _categoryService;
        private readonly Log_Service _logService;

        public ProductController(
            Product_Service productService,
            Category_Service categoryService,
            Log_Service logService,
            CurrentUser_Service currentUser)
        : base(currentUser)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logService = logService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GET(string? serviceData)
        {
            var filtering = shareConvertor.JsonDeserialize<objFiltering>(serviceData) ?? new objFiltering();

            // Get Entities
            var dbos = _productService.GET(filtering);

            // ordering
            if (filtering?.Orders is null || filtering.Orders.Count == 0)
                dbos = dbos.OrderBy(o => o.Sequence);

            // Generate result
            var result = objList<Product_dTo>.ToPagedList
            (
                dbos.Select(x => new Product_dTo(x)),
                filtering
            );

            // create Log
            await _logService.POST(
                    new Log_dRo_POST(
                        _currentUser_Id,
                        LogOperation_dEo.LogOperation.List,
                        result));

            // Return result
            return Ok(result);
        }

        // GET: api/Product/ByCategory/{categoryId}
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<IActionResult> GET(int categoryId, string? serviceData)
        {
            var filtering = shareConvertor.JsonDeserialize<objFiltering>(serviceData) ?? new objFiltering();

            // Find Category
            var category = await _categoryService.GET(categoryId);

            // Find subTree categories
            var categoryIds = await _categoryService
                                    .GET()
                                    .Where(x => x.PathById.StartsWith(category.PathById))
                                    .Select(x => x.Id)
                                    .ToListAsync();

            // Get Entities
            var dbos = _productService.GET(filtering)
                                      .Where(x => categoryIds.Contains(x.Category_Id));

            // ordering
            if (filtering?.Orders is null || filtering.Orders.Count == 0)
                dbos = dbos.OrderBy(o => o.Sequence);

            // Generate result
            var result = objList<Product_dTo>.ToPagedList
            (
                dbos.Select(x => new Product_dTo(x)),
                filtering
            );

            // create Log
            await _logService.POST(
                    new Log_dRo_POST(
                        _currentUser_Id,
                        LogOperation_dEo.LogOperation.List,
                        result));

            // Return result
            return Ok(result);
        }

        // GET: api/Product/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(int productId)
        {
            // Find Entity
            var dbo = await _productService.GET(productId);

            // Generate result
            var result = new Product_dToD(dbo);

            // create Log
            await _logService.POST(
                    new Log_dRo_POST(
                        _currentUser_Id,
                        LogOperation_dEo.LogOperation.Read,
                        result));

            // return as dTo
            return Ok(result);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> POST(Product_dRo_POST dro)
        {
            // Check category level
            await checkCategoryLevel(dro.Category_Id);

            try
            {
                await _productService.BeginTransactionAsync();

                // Create new Entity
                var dbo = await _productService.POST(dro);

                // create Id
                await _productService.FlushAsync();

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Create,
                            new Product_dToD(dbo)));

                // save chanegs
                await _productService.CommitAsync();

                // return Id
                return Ok(dbo.Id);
            }
            catch
            {
                await _productService.RollbackAsync();
                throw;
            }
        }

        // PUT: api/Product/{productId}
        [HttpPut("{productId}")]
        public async Task<IActionResult> PUT(int productId, Product_dRo_PUT dro)
        {
            // Check category level
            await checkCategoryLevel(dro.Category_Id);

            try
            {
                await _productService.BeginTransactionAsync();

                // Update Entity
                var dbo = await _productService.PUT(productId, dro);

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Update,
                            new Product_dToD(dbo)));

                // save chanegs
                await _productService.CommitAsync();

                // return
                return Ok(true);
            }
            catch
            {
                await _productService.RollbackAsync();
                throw;
            }
        }

        // PUT: api/Product/Price/{productId}
        [HttpPut("Price/{productId}")]
        public async Task<IActionResult> PUT(int productId, Product_dRo_PUT_Price dro)
        {
            try
            {
                await _productService.BeginTransactionAsync();

                // Update Entity
                var dbo = await _productService.PUT(productId, dro.Price);

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Update,
                            new Product_dToD(dbo)));

                // save chanegs
                await _productService.CommitAsync();

                // return
                return Ok(true);
            }
            catch
            {
                await _productService.RollbackAsync();
                throw;
            }
        }

        // DELETE: api/Product/{productId}
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DELETE(int productId)
        {
            try
            {
                await _productService.BeginTransactionAsync();

                // Update Entity
                var dbo = await _productService.DELETE(productId);

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Delete,
                            new Product_dToD(dbo)));

                // save chanegs
                await _productService.CommitAsync();

                // return
                return Ok(true);
            }
            catch
            {
                await _productService.RollbackAsync();
                throw;
            }
        }

        private async Task checkCategoryLevel(int category_Id)
        {
            var category = await _categoryService.GET(category_Id);
            if (!category.LevelAllowedForProduct)
                throw new Exception_BadRequest(Product_Rsx.Exception_LevelAllowedForProduct);
        }
    }
}
