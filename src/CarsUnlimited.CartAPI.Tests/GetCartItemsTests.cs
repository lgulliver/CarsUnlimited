using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using StackExchange.Redis.Extensions.Core.Abstractions;
using CarsUnlimited.CartAPI.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CarsUnlimited.CartShared.Entities;
using StackExchange.Redis;

namespace CarsUnlimited.CartAPI.Tests
{
    [TestClass]
    public class GetCartItemsTests
    {
        Mock<IRedisCacheClient> _mockIRedisCacheClient = null!;
        Mock<ILogger<UpdateCartService>> _mockILogger = null!;
        

        [TestInitialize]
        public void Initialise()
        {
            _mockIRedisCacheClient = new Mock<IRedisCacheClient>();
            _mockILogger = new Mock<ILogger<UpdateCartService>>();
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().SearchKeysAsync(It.IsAny<string>())).ReturnsAsync(new List<string>(){ "one", "two" });
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().GetAsync<CartItem>("one", It.IsAny<CommandFlags>())).ReturnsAsync(new CartItem() { SessionId = "one", Id = "three" });
            _mockIRedisCacheClient.Setup(x => x.GetDbFromConfiguration().GetAsync<CartItem>("two", It.IsAny<CommandFlags>())).ReturnsAsync(new CartItem() { SessionId = "two", Id = "four" });
        }

        [TestMethod]
        public async Task GivenASessionId_WhenAllItemsForSessionAreRequested_ThenAllItemsAreReturned()
        {
            GetCartItems getCartItems = new GetCartItems(_mockIRedisCacheClient.Object, _mockILogger.Object);
            var result = await getCartItems.GetItemsInCart("test");

            Assert.AreEqual("three", result[0].Id);
            Assert.AreEqual("four", result[1].Id);

        }
        
        [TestMethod]
        public async Task GivenASessionId_WhenNumberOfItemsInCartIsRequested_ThenCorrectNumberIsReturned()
        {
            GetCartItems getCartItems = new GetCartItems(_mockIRedisCacheClient.Object, _mockILogger.Object);
            var result = await getCartItems.GetItemsInCart("test");

            Assert.AreEqual(2, result.Count);
        }

    }
}