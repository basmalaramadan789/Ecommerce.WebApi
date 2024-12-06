using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.OrderAggregate;
using Ecommerce.Core.Interfaces;
using Ecommerce.Web.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private const string WhSecret = "whsec_d408184a73bf3f3f62dc3fc9613454976bcd50f03f999207da3960f37c0e1f9a";
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

        if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

        return basket;
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

        PaymentIntent intent;
        Order order;

        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment succeded", intent.Id);
                order = await _paymentService.UpdateOrderPaymentSucceded(intent.Id);
                _logger.LogInformation("Order Updated to payment recived", order.Id);
                break;

            case "payment_intent.payment_failed":
                intent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment failed", intent.Id);
                order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                _logger.LogInformation("Order Updated to payment failed", order.Id);
                break;

        }

        return new EmptyResult();



    }

}
