using Microsoft.EntityFrameworkCore;
using Saman.Backend.Business.baseServices;
using Saman.Backend.Business.Entity.Product.Rsx;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;

namespace Saman.Backend.Business.Entity.Product
{
    public class Product_Service : baseService
    {
        public Product_Service(CRUD_Service crudService)
        : base(crudService) { }
    
        public IQueryable<Product_dBo> GET(objFiltering? filtering = null)
        {
            var dbos = _crudService.GET<Product_dBo>();

            // searching
            if (!string.IsNullOrWhiteSpace(filtering?.SearchValue))
                dbos = dbos.Where(x => x.Name.Contains(filtering.SearchValue)
                                    || x.SKU.Contains(filtering.SearchValue)
                                    || x.Description.Contains(filtering.SearchValue));

            return dbos;
        }

        public async Task<Product_dBo> GET(int product_Id)
            => await _crudService.GET<Product_dBo>(product_Id)
            ?? throw new Exception_NotFound();

        public async Task<Product_dBo?> Find(string sku)
            => await _crudService
                     .GET<Product_dBo>()
                     .FirstOrDefaultAsync(x => x.SKU == sku);

        public async Task<Product_dBo> POST(Product_dRo_POST dro)
        {
            var duplicateSKU = await Find(dro.SKU);
            if (duplicateSKU != null)
                throw new Exception_BadRequest(Product_Rsx.Exception_DuplicateSKU);

            // Objects
            var dbo = new Product_dBo(
                dro.SKU,
                dro.Name,
                dro.Description,
                dro.Price,
                dro.Sequence,
                dro.Category_Id);

            // Actions
            var result = await _crudService.POST(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }

        public async Task<Product_dBo> PUT(int product_Id, Product_dRo_PUT dro)
        {
            // Objects
            var dbo = await GET(product_Id);

            // Changes
            dbo.Name = dro.Name;
            dbo.Description = dro.Description;
            dbo.Price = dro.Price;
            dbo.Sequence = dro.Sequence;
            dbo.Category_Id = dro.Category_Id;

            // Actions
            var result = await _crudService.PUT(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }

        public async Task<Product_dBo> PUT(int product_Id, decimal price)
        {
            // Objects
            var dbo = await GET(product_Id);

            // Changes
            dbo.Price = price;

            // Actions
            var result = await _crudService.PUT(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }

        public async Task<Product_dBo> PUT(int product_Id, int onHand)
        {
            // Objects
            var dbo = await GET(product_Id);

            // Changes
            dbo.OnHand = onHand;

            // Actions
            var result = await _crudService.PUT(dbo)
                      ?? throw new Exception_InternalServerError();

            return result;
        }

        public async Task<Product_dBo> DELETE(int product_Id)
        {
            // Objects
            var dbo = await GET(product_Id);

            // delete object
            var result = await _crudService.DELETE(dbo)
                         is not null;

            return dbo;
        }
    }
}
