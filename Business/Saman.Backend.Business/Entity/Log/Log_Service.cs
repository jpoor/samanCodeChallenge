using Saman.Backend.Business.baseServices;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;

namespace Saman.Backend.Business.Entity.Log
{
    public class Log_Service : baseService
    {
        public Log_Service(CRUD_Service crudService)
        : base(crudService) { }

        public IQueryable<Log_dBo> GET(objFiltering? filtering = null)
        {
            var dbos = _crudService.GET<Log_dBo>();

            // searching
            if (!string.IsNullOrWhiteSpace(filtering?.SearchValue))
                dbos = dbos.Where(x => x.EntityId.Contains(filtering.SearchValue)
                                    || x.EntityName.Contains(filtering.SearchValue));

            return dbos;
        }

        public async Task<Log_dBo> GET(long log_Id)
            => await _crudService.GET<Log_dBo>(log_Id)
            ?? throw new Exception_NotFound();

        public async Task<Log_dBo> POST(Log_dRo_POST dro)
        {
            // Objects
            var dbo = new Log_dBo(
                dro.User_Id,
                dro.Operation,
                dro.entity);

            // Actions
            var result = await _crudService.POST(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }
    }
}
