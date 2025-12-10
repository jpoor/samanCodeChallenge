using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Saman.Backend.Business.DataAccess;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareServices;
using Saman.Backend.Share.shareValidators;
using System.Data;

namespace Saman.Backend.Business.baseServices
{
    public class CRUD_Service : IDisposable
    {
        private IDbContextTransaction? dbTr;
        private readonly DbContext _dbContext;
        internal readonly CurrentUser_Service currentUser;

        public CRUD_Service(mainDbContext dbContext, CurrentUser_Service currentUserService)
        {
            _dbContext = dbContext;
            currentUser = currentUserService;
        }

        public async Task BeginTransactionAsync()
        {
            if (dbTr == null)
            {
                dbTr = await _dbContext
                             .Database
                             .BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();

            if (dbTr != null)
            {
                await dbTr.CommitAsync();

                dbTr = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (dbTr != null)
            {
                await dbTr.RollbackAsync();

                dbTr = null;
            }
        }

        public async Task FlushAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void DetectChanges()
        {
            _dbContext.ChangeTracker
                      .DetectChanges();
        }

        public async Task ReloadAsync(baseDBo dbo)
        {
            await _dbContext.Entry(dbo)
                            .ReloadAsync();
        }


        internal IQueryable<T> GET<T>(bool asNoTracking = true, bool considerDeactives = false) where T : baseDBo
        {
            var entity = _dbContext
                         .Set<T>();

            var query = entity
                       .Where(x => !x.Deactive || considerDeactives);

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        internal async Task<T?> GET<T>(object id) where T : baseDBo
        {
            try
            {
                var entity = _dbContext.Set<T>();

                var item = await entity.FindAsync(id);

                if (item != null && item.Deactive)
                    return null;

                return item;
            }
            catch { }

            return null;
        }

        internal async Task<T?> POST<T>(T dbo) where T : baseDBo
        {
            dbo.CreationDate = DateTime.UtcNow;
            dbo.CreatorId = currentUser.Id;

            shareConvertor.RefactorFarsiProperties(dbo);

            baseValidator.ValidateAndThrowIfInvalid(dbo);

            try
            {
                _dbContext.Entry(dbo).State = EntityState.Added;

                if (dbTr == null)
                    await FlushAsync();

                return dbo;
            }
            catch { }

            return null;
        }

        internal async Task<IEnumerable<T>?> POST<T>(IEnumerable<T> dbo) where T : baseDBo
        {
            var dboList = dbo.ToList();

            foreach (var ent in dboList)
            {
                ent.CreationDate = DateTime.UtcNow;
                ent.CreatorId = currentUser.Id;

                shareConvertor.RefactorFarsiProperties(ent);
            }

            baseValidator.ValidateAndThrowIfInvalid(dbo);

            try
            {
                var entity = _dbContext.Set<T>();

                entity.AddRange(dboList);

                if (dbTr == null)
                    await FlushAsync();

                return dboList;
            }
            catch { }

            return null;
        }

        internal async Task<T?> PUT<T>(T dbo) where T : baseDBo
        {
            dbo.UpdateDate = DateTime.UtcNow;
            dbo.UpdaterId = currentUser.Id;

            shareConvertor.RefactorFarsiProperties(dbo);

            baseValidator.ValidateAndThrowIfInvalid(dbo);

            try
            {
                _dbContext.Entry(dbo).State = EntityState.Modified;

                if (dbTr == null)
                    await FlushAsync();

                return dbo;
            }
            catch { }

            return null;
        }

        internal async Task<IEnumerable<T>?> PUT<T>(IEnumerable<T> dbo) where T : baseDBo
        {
            var dboList = dbo.ToList();

            foreach (var ent in dboList)
            {
                ent.UpdateDate = DateTime.UtcNow;
                ent.UpdaterId = currentUser.Id;

                shareConvertor.RefactorFarsiProperties(ent);

                _dbContext.Entry(ent).State = EntityState.Modified;
            }

            baseValidator.ValidateAndThrowIfInvalid(dbo);

            try
            {
                if (dbTr == null)
                    await FlushAsync();

                return dboList;
            }
            catch { }

            return null;
        }

        internal async Task<T?> DELETE<T>(object id, bool PhysicalDelete = false) where T : baseDBo
        {
            try
            {
                var dbo = await GET<T>(id);

                if (dbo != null)
                    return await DELETE(dbo, PhysicalDelete);
            }
            catch { }

            return null;
        }

        internal async Task<T?> DELETE<T>(T dbo, bool PhysicalDelete = false) where T : baseDBo
        {
            try
            {
                if (PhysicalDelete)
                {
                    _dbContext.Entry(dbo).State = EntityState.Deleted;

                    if (dbTr == null)
                        await FlushAsync();
                }

                else
                {
                    dbo.Deactive = true;

                    await PUT(dbo);
                }

                return dbo;
            }
            catch { }

            return null;
        }

        internal async Task<IEnumerable<T>?> DELETE<T>(IEnumerable<T> dbo, bool PhysicalDelete = false) where T : baseDBo
        {
            try
            {
                if (PhysicalDelete)
                {
                    var entity = _dbContext.Set<T>();

                    entity.RemoveRange(dbo);

                    if (dbTr == null)
                        await FlushAsync();

                    return dbo;
                }
                else
                {
                    var dboList = dbo.ToList();

                    foreach (var ent in dboList)
                        ent.Deactive = true;

                    await PUT(dboList);

                    return dboList;
                }
            }
            catch { }

            return null;
        }

        internal dynamic SqlQuery<T>(string storedProcedureName, params object[] parameters)
            => _dbContext
               .Database
               .SqlQueryRaw<T>(storedProcedureName, parameters);

        internal List<dynamic> ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new List<dynamic>();

            using (SqlConnection connection = new(_dbContext.Database.GetConnectionString()))
            {
                SqlCommand command = new(storedProcedureName, connection);

                command.CommandType = CommandType.StoredProcedure;

                // Set parameters
                foreach (SqlParameter parameter in parameters)
                    command.Parameters.Add(parameter);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var rowData = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = string.IsNullOrWhiteSpace(reader.GetName(i))
                                              ? "Col" + i
                                              : reader.GetName(i);

                            object columnValue = reader.GetValue(i);

                            rowData.Add(columnName, columnValue);
                        }

                        result.Add(rowData);
                    }

                    reader.Close();
                }
                catch
                {
                    throw;
                }
            }

            return result;
        }

        private bool disposedValue = false; // To detect redundant calls
        public virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && dbTr != null)
                    dbTr.Dispose();

                disposedValue = true;
            }
        }
        public void Dispose() { Dispose(true); }
    }
}
