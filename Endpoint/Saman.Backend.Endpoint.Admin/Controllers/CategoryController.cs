using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saman.Backend.Business.Entity.Category;
using Saman.Backend.Business.Entity.Log;
using Saman.Backend.Endpoint.Admin.baseClasses;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareServices;

namespace Saman.Backend.Endpoint.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : baseController
    {
        private readonly Category_Service _categoryService;
        private readonly Log_Service _logService;

        public CategoryController(
            Category_Service categoryService,
            Log_Service logService,
            CurrentUser_Service currentUser)
        : base(currentUser)
        {
            _categoryService = categoryService;
            _logService = logService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GET(string? serviceData)
        {
            var filtering = shareConvertor.JsonDeserialize<objFiltering>(serviceData) ?? new objFiltering();

            // Get Entities
            var dbos = _categoryService.GET(filtering);

            // ordering
            if (filtering?.Orders is null || filtering.Orders.Count == 0)
                dbos = dbos.OrderByDescending(o => o.Id);

            // Generate result
            var result = objList<Category_dTo>.ToPagedList
            (
                dbos.Select(x => new Category_dTo(x)),
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

        // GET: api/Category/{categoryId}
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> Get(int categoryId)
        {
            // Find Entity
            var dbo = await _categoryService.GET(categoryId);

            // Generate result
            var result = new Category_dToD(dbo);

            // create Log
            await _logService.POST(
                    new Log_dRo_POST(
                        _currentUser_Id,
                        LogOperation_dEo.LogOperation.Read,
                        result));

            // return as dTo
            return Ok(result);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> POST(Category_dRo_POST dro)
        {
            try
            {
                await _categoryService.BeginTransactionAsync();

                // Create new Entity
                var dbo = await _categoryService.POST(dro);

                // create Id
                await _categoryService.FlushAsync();

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Create,
                            new Category_dToD(dbo)));

                // save chanegs
                await _categoryService.CommitAsync();

                // return Id
                return Ok(dbo.Id);
            }
            catch
            {
                await _categoryService.RollbackAsync();
                throw;
            }
        }

        // PUT: api/Category/{categoryId}
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> PUT(int categoryId, Category_dRo_PUT dro)
        {
            try
            {
                await _categoryService.BeginTransactionAsync();

                // Update Entity
                var dbo = await _categoryService.PUT(categoryId, dro);

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Update,
                            new Category_dToD(dbo)));

                // save chanegs
                await _categoryService.CommitAsync();

                // return
                return Ok(true);
            }
            catch
            {
                await _categoryService.RollbackAsync();
                throw;
            }
        }

        // DELETE: api/Category/{categoryId}
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DELETE(int categoryId)
        {
            try
            {
                await _categoryService.BeginTransactionAsync();

                // Update Entity
                var dbo = await _categoryService.DELETE(categoryId);

                // create Log
                await _logService.POST(
                        new Log_dRo_POST(
                            _currentUser_Id,
                            LogOperation_dEo.LogOperation.Delete,
                            new Category_dToD(dbo)));

                // save chanegs
                await _categoryService.CommitAsync();

                // return
                return Ok(true);
            }
            catch
            {
                await _categoryService.RollbackAsync();
                throw;
            }
        }
    }
}
