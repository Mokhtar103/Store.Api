using Domain.Contract;
using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderWithPaymentIntentSpecification(string paymentIntentId)
        : Specification<Order>(x => x.PaymentIntentId == paymentIntentId)
    {
    }
}
