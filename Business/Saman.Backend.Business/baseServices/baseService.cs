using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.baseServices
{
    public abstract class baseService
    {
        public readonly CRUD_Service _crudService;

        public baseService(CRUD_Service crudService) => _crudService = crudService;

        public virtual async Task BeginTransactionAsync() => await _crudService.BeginTransactionAsync();

        public virtual async Task CommitAsync() => await _crudService.CommitAsync();

        public virtual async Task RollbackAsync() => await _crudService.RollbackAsync();

        public virtual async Task FlushAsync() => await _crudService.FlushAsync();

        public virtual void DetectChanges() => _crudService.DetectChanges();

        public virtual async Task ReloadAsync(baseDBo dbo) => await _crudService.ReloadAsync(dbo);
    }
}