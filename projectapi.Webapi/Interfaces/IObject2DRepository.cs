namespace projectapi.Webapi.Interfaces
{
    public interface IObject2DRepository
    {
        Task<Object2D?> ReadAsync(int id);
    }
}