namespace Saman.Backend.Business.Entity.Product
{
    public record Product_dRo_POST(
       string SKU,
       string Name,
       string Description,
       decimal Price,
       int Sequence,
       int Category_Id);

    public record Product_dRo_PUT(
       string Name,
       string Description,
       decimal Price,
       int Sequence,
       int Category_Id);

    public record Product_dRo_PUT_Price(
       decimal Price);
}
