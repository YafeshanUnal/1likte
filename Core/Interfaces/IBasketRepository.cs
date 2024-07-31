namespace Core
{
    public interface IBasketRepository
    {
        Task AddItem(string store, string item, decimal price);
        Task RemoveItem(string store, string item);
        Task<List<Tuple<string, decimal>>> GetBasketItems(string store);
        Task<decimal> GetTotalAmount(string store);
    }
}