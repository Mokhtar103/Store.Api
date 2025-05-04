using AutoMapper;
using Domain.Contract;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService(
        IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string buyerEmail)
        {
            // Address
            var address = mapper.Map<Address>(orderRequest.ShippingAddress);

            // Basket
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);

            if (basket is null)
                throw new BasketNotFoundException(orderRequest.BasketId);

            var orderRepo = unitOfWork.GetRepository<Order, Guid>();

            var existingOrderSpecs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await orderRepo.GetAsync(existingOrderSpecs);

            if(existingOrder is not null)
                orderRepo.Delete(existingOrder);

            var orderItems = new List<OrderItem>();

            var productRepo = unitOfWork.GetRepository<Product, int>();

            foreach(var item in basket.Items)
            {
                var product = await productRepo.GetAsync(item.Id);

                if(product is null)
                    throw new ProductNotFoundException(item.Id);

                var productInOrderItem = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);

                var orderItem = new OrderItem(productInOrderItem, item.Quantity, product.Price);

                orderItems.Add(orderItem);
            }

            // Delivery Method
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);

            if(deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            // Subtotal
            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);

            var order = new Order(buyerEmail, address, orderItems, deliveryMethod, subTotal);

            await unitOfWork.GetRepository<Order, Guid>().AddAsync(order);

            await unitOfWork.SaveChangesAsync();

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var orderSpecs = new OrderWithIncludeSpecification(id);

            var order = await unitOfWork.GetRepository<Order, Guid>().GetAsync(orderSpecs);

            if(order is null)
                throw new OrderNotFoundException(id);   

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            var ordersSpecs = new OrderWithIncludeSpecification(email);

            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(ordersSpecs);

            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
