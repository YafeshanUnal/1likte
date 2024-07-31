using Core;
using Moq;

public class BasketTestFixture
{
    public IBasketRepository BasketRepository { get; private set; }

    public BasketTestFixture()
    {
        var mockBasketRepository = new Mock<IBasketRepository>();

        var basketItems = new Dictionary<string, List<Tuple<string, decimal>>>();

        mockBasketRepository.Setup(repo => repo.AddItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
            .Callback<string, string, decimal>((store, item, price) =>
            {
                if (!basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
                {
                    value = new List<Tuple<string, decimal>>();
                    basketItems[store] = value;
                }

                value.Add(Tuple.Create(item, price));
            });

        mockBasketRepository.Setup(repo => repo.RemoveItem(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((store, item) =>
            {
                if (basketItems.TryGetValue(store, out List<Tuple<string, decimal>>? value))
                {
                    var itemToRemove = value.FirstOrDefault(i => i.Item1 == item);
                    if (itemToRemove != null)
                    {
                        value.Remove(itemToRemove);
                    }
                }
            });

        mockBasketRepository.Setup(repo => repo.GetBasketItems(It.IsAny<string>()))
            .Returns(Task.FromResult(new List<Tuple<string, decimal>>()));

        mockBasketRepository.Setup(repo => repo.GetTotalAmount(It.IsAny<string>()))
            .Returns(Task.FromResult(3.0m));

        BasketRepository = mockBasketRepository.Object;
    }
}