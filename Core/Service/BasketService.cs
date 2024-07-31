using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class BasketService
    {
        private IBasketRepository _basketRepository;

        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task AddItem(string store, string item, decimal price)
        {
            await _basketRepository.AddItem(store, item, price);
        }
        public async Task RemoveItem(string store, string item)
        {
            await _basketRepository.RemoveItem(store, item);
        }

        public async Task<List<Tuple<string, decimal>>> GetBasketItems(string store)
        {
            if (string.IsNullOrEmpty(store))
            {
                throw new ArgumentException("Store cannot be null or empty");
            }
            return await _basketRepository.GetBasketItems(store);
        }

        public async Task<decimal> GetTotalAmount(string store)
        {
            return await _basketRepository.GetTotalAmount(store);
        }
    }
}