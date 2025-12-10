namespace Saman.Backend.Business.Entity.Category
{
    public record Category_dRo_POST(
        string Name,
        int? Parent_Id = null);

    public record Category_dRo_PUT(
        string Name);
}
