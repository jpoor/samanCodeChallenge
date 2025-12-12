namespace Saman.Backend.Business.Entity.Log
{
    public record Log_dRo_POST(
        string User_Id,
        LogOperation_dEo.LogOperation Operation,
        object entity);
}
