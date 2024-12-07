using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NetflixApi.Model;
using NetflixApi.Services;

namespace NetflixApi.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
   
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionServices _subscriptionService;
        public SubscriptionController(SubscriptionServices subscriptionServices)
        {
            _subscriptionService = subscriptionServices;
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> GetSubscriptionsByUserId([FromBody] UserIdRequest request)
        {
            var userId = request.UserId;
            var subscriptions = await _subscriptionService.GetSubscriptionsByUserIdAsync(userId);

            if (subscriptions == null || subscriptions.Count == 0)
            {
                return Ok("No subscriptions found for the specified user.");
            }

            return Ok(subscriptions);
        }


        [HttpPost]
        public IActionResult CreateSubscription(Subscription subscription)
        {
            // Perform payment processing and validation here
            // You may need to integrate with a payment gateway

            _subscriptionService.CreateSubscription(subscription);

            return Ok(subscription);
        }
    }
}
