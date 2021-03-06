﻿using CarsUnlimited.CartAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CarsUnlimited.CartShared.Entities;

namespace CarsUnlimited.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;
        private readonly IConfiguration _config;

        public CartController(ICartService cartService, ILogger<CartController> logger, IConfiguration configuration)
        {
            _cartService = cartService;
            _logger = logger;
            _config = configuration;
        }

        [HttpPost]
        [Route("add-to-cart")]
        public IActionResult AddItemToCart([FromHeader(Name = "X-CarsUnlimited-SessionId")] string sessionId, [FromBody]CartItem cartItem) 
        {

            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                _logger.LogInformation($"Adding item to cart {sessionId}");

                cartItem.SessionId = sessionId;

                _cartService.AddToCart(cartItem);

                return StatusCode(200);
            } else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("get-cart-items")]
        public async Task<IActionResult> GetItemsInCart([FromHeader(Name = "X-CarsUnlimited-SessionId")] string sessionId)
        {
            if(!string.IsNullOrWhiteSpace(sessionId))
            {
                List<CartItem> cartItems = await _cartService.GetItemsInCart(sessionId);
                return StatusCode(200, cartItems);
            } else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("get-cart-items-count")]
        public async Task<IActionResult> GetItemsInCartCount([FromHeader(Name = "X-CarsUnlimited-SessionId")] string sessionId)
        {
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return StatusCode(200, await _cartService.GetItemsInCartCount(sessionId));
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("delete-item-from-cart")]
        public async Task<IActionResult> DeleteItemFromCart([FromHeader(Name = "X-CarsUnlimited-SessionId")] string sessionId, string carId)
        {
            if(!string.IsNullOrWhiteSpace(sessionId) && !string.IsNullOrWhiteSpace(carId))
            {
                return StatusCode(200, await _cartService.DeleteFromCart(sessionId, carId));
            }

            return StatusCode(404);
        }

        [HttpGet]
        [Route("delete-cart")]
        public async Task<IActionResult> DeleteCart([FromHeader(Name = "X-CarsUnlimited-SessionId")] string sessionId)
        {
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                if (await _cartService.DeleteAllFromCart(sessionId))
                {
                    return StatusCode(200);
                }

                return StatusCode(404);
            }

            return StatusCode(404);
        }

        [HttpPost]
        [Route("complete-cart")]
        public async Task<IActionResult> CompleteCart([FromHeader(Name = "X-CarsUnlimited-CartApiKey")] string cartApiKey, string sessionId)
        {
            if(!string.IsNullOrWhiteSpace(cartApiKey) && cartApiKey == _config.GetValue<string>("CartApiKey"))
            {
                await _cartService.CompleteCart(sessionId);
                return StatusCode(200);
            }

            return StatusCode(401);
        }
    }
}
