namespace Core
{
    public class BasketRepository : IBasketRepository
    {
        private Dictionary<string, List<Tuple<string, decimal>>> basketItems;

        public BasketRepository()
        {
            basketItems = new Dictionary<string, List<Tuple<string, decimal>>>();
        }

        public async Task AddItem(string store, string item, decimal price)
        {
            await Task.CompletedTask;
            if (!basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
            {
                value = new List<Tuple<string, decimal>>();
                basketItems[store] = value;
            }

            value.Add(Tuple.Create(item, price));
        }

        public async Task RemoveItem(string store, string item)
        {
            await Task.CompletedTask;
            if (basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
            {
                var itemToRemove = value.FirstOrDefault(i => i.Item1 == item);
                if (itemToRemove != null)
                {
                    value.Remove(itemToRemove);
                }
            }
        }

        public async Task<List<Tuple<string, decimal>>> GetBasketItems(string store)
        {
            await Task.CompletedTask;
            if (basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
            {
                return value;
            }

            return new List<Tuple<string, decimal>>();
        }

        public async Task<decimal> GetTotalAmount(string store)
        {
            await Task.CompletedTask;
            if (basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
            {
                return value.Sum(i => i.Item2);
            }

            return 0;
        }

    }
}