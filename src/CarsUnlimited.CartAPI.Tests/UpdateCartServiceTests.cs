using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarsUnlimited.CartAPI.Services;
using CarsUnlimited.CartShared.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using StackExchange.Redis;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Collections.Generic;

namespace CarsUnlimited.CartAPI.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        Mock<IRedisCacheClient> _mockIRedisCacheClient = null!;
        Mock<ILogger<UpdateCartService>> _mockILogger = null!;
        Mock<IConfiguration> _mockIConfiguration = null!;
        Mock<IGetCartItems> _mockGetCartItems = null!;
        Mock<IConnectionFactory> _mockIConnectionFactory = null!;
        Mock<IConnection> _mockIConnection = null!;
        Mock<IModel> _mockIModel = null!;

        [TestInitialize]
        public void Initialise()
        {
            _mockIRedisCacheClient = new Mock<IRedisCacheClient>();
            _mockILogger = new Mock<ILogger<UpdateCartService>>();
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockGetCartItems = new Mock<IGetCartItems>();
            _mockIConnectionFactory = new Mock<IConnectionFactory>();
            _mockIConnection = new Mock<IConnection>();
            _mockIModel = new Mock<IModel>();

            _mockGetCartItems.Setup(x => x.GetItemsInCart(It.IsAny<string>())).ReturnsAsync(new List<CartItem>(){ new CartItem() { SessionId = "1", Id = "3" }, new CartItem() { SessionId = "2", Id = "4" } });
        }

        [TestMethod]
        public async Task GivenCartItem_WhenAddedToCart_ThenItemIsAddedToCart()
        {
            var cartItem = new CartItem();
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().AddAsync<CartItem>(It.IsAny<string>(), It.IsAny<CartItem>(), 
                                            It.IsAny<DateTimeOffset>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);
            UpdateCartService service = new UpdateCartService(_mockIRedisCacheClient.Object, _mockILogger.Object, _mockIConfiguration.Object, _mockGetCartItems.Object);
            var result = await service.AddToCart(cartItem);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task GivenCartWithItems_WhenCartIsCompleted_ThenAllItemsSavedToInventory()
        {
            CartCompleteSetup();
            UpdateCartService service = new UpdateCartService(_mockIRedisCacheClient.Object, _mockILogger.Object, _mockIConfiguration.Object, _mockGetCartItems.Object);

            var result = await service.CompleteCart("test", _mockIConnectionFactory.Object);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task GivenCartWith2Items_WhenCartIsCompleted_Then2QueuesAreDeclared()
        {
            CartCompleteSetup();
            UpdateCartService service = new UpdateCartService(_mockIRedisCacheClient.Object, _mockILogger.Object, _mockIConfiguration.Object, _mockGetCartItems.Object);

            var result = await service.CompleteCart("test", _mockIConnectionFactory.Object);

            _mockIModel.Verify(x => x.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task GivenCartWithItems_WhenRequestedToRemoveAllItems_ThenAllItemsAreRemoved()
        {
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().RemoveAllAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CommandFlags>())).ReturnsAsync(new long());

            UpdateCartService service = new UpdateCartService(_mockIRedisCacheClient.Object, _mockILogger.Object, _mockIConfiguration.Object, _mockGetCartItems.Object);

            var result = await service.DeleteAllFromCart("");

            _mockIRedisCacheClient.Verify(x => x.GetDbFromConfiguration().RemoveAllAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenCartWithItems_WhenRequestedToRemoveIndividualItem_ThenOnlyThatItemIsRemoved()
        {
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().RemoveAsync(It.IsAny<string>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);

            UpdateCartService service = new UpdateCartService(_mockIRedisCacheClient.Object, _mockILogger.Object, _mockIConfiguration.Object, _mockGetCartItems.Object);

            var result = await service.DeleteFromCart("1", "3");

            _mockIRedisCacheClient.Verify(x => x.GetDbFromConfiguration().RemoveAsync("1_3", It.IsAny<CommandFlags>()), Times.Once);
            _mockIRedisCacheClient.Verify(x => x.GetDbFromConfiguration().RemoveAsync("2_4", It.IsAny<CommandFlags>()), Times.Never);
        }

        private void CartCompleteSetup()
        {
            _mockIConnection.Setup(x => x.CreateModel()).Returns(_mockIModel.Object);
            _mockIConnectionFactory.Setup(x => x.CreateConnection()).Returns(_mockIConnection.Object);
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().RemoveAsync(It.IsAny<string>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);
        }
    }
}