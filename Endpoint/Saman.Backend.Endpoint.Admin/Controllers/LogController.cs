using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saman.Backend.Business.Entity.Log;
using Saman.Backend.Endpoint.Admin.baseClasses;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareServices;

namespace Saman.Backend.Endpoint.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : baseController
    {
        private readonly Log_Service _logService;

        public LogController(
            Log_Service logService,
            CurrentUser_Service currentUser)
        : base(currentUser)
            => _logService = logService;

        // GET: api/Log
        [HttpGet]
        public async Task<IActionResult> GET(string? serviceData)
        {
            var filtering = shareConvertor.JsonDeserialize<objFiltering>(serviceData) ?? new objFiltering();

            // Get Entities
            var dbos = _logService.GET(filtering);

            // ordering
            if (filtering?.Orders is null || filtering.Orders.Count == 0)
                dbos = dbos.OrderByDescending(o => o.Id);

            // Generate result
            var result = objList<Log_dTo>.ToPagedList
            (
                dbos.Select(x => new Log_dTo(x)),
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

        // GET: api/Log/{logId}
        [HttpGet("{logId}")]
        public async Task<IActionResult> Get(long logId)
        {
            // Find Entity
            var dbo = await _logService.GET(logId);

            // Generate result
            var result = new Log_dToD(dbo);

            // create Log
            await _logService.POST(
                    new Log_dRo_POST(
                        _currentUser_Id,
                        LogOperation_dEo.LogOperation.Read,
                        result));

            // return as dTo
            return Ok(result);
        }
    }
}
