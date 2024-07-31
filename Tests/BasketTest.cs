using Core;
using Moq;
using Xunit;

namespace Tests
{
    public class BasketTest : IClassFixture<BasketTestFixture>
    {
        private IBasketRepository _basketRepository;

        public BasketTest(BasketTestFixture fixture)
        {
            _basketRepository = fixture.BasketRepository;
        }

        [Fact]
        public async Task TestAddItem()
        {
            var mockBasketRepository = new Mock<IBasketRepository>();
            var basketItems = new Dictionary<string, List<Tuple<string, decimal>>>();

            mockBasketRepository.Setup(repo => repo.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Callback((string store, string item, decimal price) =>
                {
                    if (!basketItems.TryGetValue(store, out var value))
                    {
                        value = new List<Tuple<string, decimal>>();
                        basketItems[store] = value;
                    }

                    value.Add(Tuple.Create(item, price));
                });

            BasketService basketService = new BasketService(mockBasketRepository.Object);
            string store = "Store1";
            string item = "Apple";
            decimal price = 1.0m;

            await basketService.AddItem(store, item, price);
            await basketService.AddItem(store, "Banana", 2.0m);

            mockBasketRepository.Verify(repo => repo.AddItem(store, item, price), Times.Once);
            mockBasketRepository.Verify(repo => repo.AddItem(store, "Banana", 2.0m), Times.Once);

            var basket = await basketService.GetBasketItems(store);
            Assert.Contains(basket, i => i.Item1 == item);
        }

        [Fact]
        public async Task TestRemoveItem()
        {
            BasketService basketService = new BasketService(_basketRepository);
            string store = "Store1";
            string item = "Apple";
            decimal price = 1.0m;

            await basketService.AddItem(store, item, price);
            await basketService.RemoveItem(store, item);

            var basketItems = await basketService.GetBasketItems(store);
            Assert.DoesNotContain(basketItems, i => i.Item1 == item);
        }


        [Fact]
        public async Task TestGetTotalAmount()
        {
            BasketService basketService = new BasketService(_basketRepository);
            string store = "Store1";
            string item1 = "Apple";
            string item2 = "Banana";
            decimal price1 = 1.0m;
            decimal price2 = 2.0m;

            await basketService.AddItem(store, item1, price1);
            await basketService.AddItem(store, item2, price2);

            decimal totalAmount = await basketService.GetTotalAmount(store);

            Assert.Equal(price1 + price2, totalAmount);
        }
    }
}