using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

    Task<Order> UpdateOrderPaymentSucceded(string paymentIntentId);
    Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
}
