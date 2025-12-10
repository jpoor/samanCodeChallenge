using Saman.Backend.Business.baseServices;
using Saman.Backend.Business.Entity.Category.Rsx;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;

namespace Saman.Backend.Business.Entity.Category
{
    public class Category_Service : baseService
    {
        public Category_Service(CRUD_Service crudService)
        : base(crudService) { }

        public IQueryable<Category_dBo> GET(objFiltering? filtering = null)
        {
            var dbos = _crudService.GET<Category_dBo>();

            // searching
            if (!string.IsNullOrWhiteSpace(filtering?.SearchValue))
                dbos = dbos.Where(x => x.Name.Contains(filtering.SearchValue)
                                    || x.PathByName.Contains(filtering.SearchValue)
                                    || x.PathById.Contains(filtering.SearchValue));

            return dbos;
        }

        public async Task<Category_dBo> GET(int category_Id)
            => await _crudService.GET<Category_dBo>(category_Id)
            ?? throw new Exception_NotFound();

        public async Task<Category_dBo> POST(Category_dRo_POST dro)
        {
            Category_dBo? parent = null;

            // Find parent
            if (dro.Parent_Id != null)
                parent = await GET(dro.Parent_Id.Value);

            // Objects
            var dbo = new Category_dBo(
                dro.Name,
                (parent?.Level ?? 0) + 1,
                dro.Parent_Id);

            // Actions
            var result = await _crudService.POST(dbo)
                      ?? throw new Exception_InternalServerError();

            // generate Id
            await _crudService.FlushAsync();

            // generate Path info
            result.PathByName = generatePath(dro.Name, parent?.PathByName);
            result.PathById = generatePath(result.Id.ToString(), parent?.PathById);

            // Update path info
            await _crudService.PUT(result);

            return result;
        }

        public async Task<Category_dBo> PUT(int category_Id, Category_dRo_PUT dro)
        {
            // Objects
            var dbo = await GET(category_Id);

            // Changes
            dbo.Name = dro.Name;
            dbo.PathByName = generatePath(dro.Name, dbo.Parent?.PathByName);

            // Actions
            var result = await _crudService.PUT(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }

        public async Task<Category_dBo> DELETE(int category_Id)
        {
            // Objects
            var dbo = await GET(category_Id);

            // Validations
            if (dbo.Children.Any(x => !x.Deactive))
                throw new Exception_AccessDenied(Category_Rsx.Exception_HasChildren);
            if (dbo.Products.Any(x => !x.Deactive))
                throw new Exception_AccessDenied(Category_Rsx.Exception_HasProduct);

            // delete object
            var result = await _crudService.DELETE(dbo)
                         is not null;

            return dbo;
        }


        private string generatePath(string currentPath, string? parentPath)
        {
            if (string.IsNullOrWhiteSpace(currentPath))
                return string.Empty;

            var chars = currentPath.Trim()
                                   .Normalize(System.Text.NormalizationForm.FormD)
                                   .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                                            != System.Globalization.UnicodeCategory.NonSpacingMark);

            if (string.IsNullOrWhiteSpace(parentPath))
                return currentPath;

            return $"{parentPath}/{currentPath}";
        }
    }
}
