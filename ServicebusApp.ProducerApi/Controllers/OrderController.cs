using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicebusApp.Common;
using ServicebusApp.Common.Dto;
using ServicebusApp.Common.Events;
using ServicebusApp.ProducerApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicebusApp.ProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AzureService azureService;

        public OrderController(AzureService azureService)
        {
            this.azureService = azureService;
        }



        [HttpPost]
        public async Task CreateOrder(OrderDto order)
        {
            // insert order into database

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                Id = order.Id,
                ProductName = order.ProductName,
                CreatedOn= DateTime.Now
            };

            //await azureService.CreateQueueIfNotExists(Constants.OrderCreatedQueueName);
            //await azureService.SendMessageToQueue(Constants.OrderCreatedQueueName, orderCreatedEvent);

            await azureService.CreateTopicIfNotExists(Constants.OrderTopic);
            await azureService.CreateSubscriptionIfNotExists(Constants.OrderTopic, Constants.OrderCreatedSubName, "OrderCreated", "OrderCreatedOnly");



            await azureService.SendMessageToTopic(Constants.OrderTopic, orderCreatedEvent, "OrderCreated");
        }

        [HttpDelete("{id}")]
        public async Task DeleteOrder(int id)
        {
            // insert order into database

            var orderDeletedEvent = new OrderDeletedEvent()
            {
                Id = id,
                CreatedOn = DateTime.Now
            };

            //await azureService.CreateQueueIfNotExists(Constants.OrderDeletedQueueName);
            //await azureService.SendMessageToQueue(Constants.OrderDeletedQueueName, orderDeletedEvent);

            await azureService.CreateTopicIfNotExists(Constants.OrderTopic);
            await azureService.CreateSubscriptionIfNotExists(Constants.OrderTopic, Constants.OrderDeletedSubName, "OrderDeleted", "OrderDeletedOnly");

            await azureService.SendMessageToTopic(Constants.OrderTopic, orderDeletedEvent, "OrderDeleted");
        }

    }

}
