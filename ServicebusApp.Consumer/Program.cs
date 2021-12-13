using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServicebusApp.Common;
using ServicebusApp.Common.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServicebusApp.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsumeSub<OrderCreatedEvent>(Constants.OrderTopic, Constants.OrderCreatedSubName, i => 
            {
                Console.WriteLine($"OrderCreatedEvent ReceivedMessage with id: {i.Id}, Name: {i.ProductName}");
            }).Wait();


            ConsumeSub<OrderDeletedEvent>(Constants.OrderTopic, Constants.OrderDeletedSubName, i =>
            {
                Console.WriteLine($"OrderDeletedEvent ReceivedMessage with id: {i.Id}");
            }).Wait();

            Console.WriteLine("\n\n\n");

            Console.ReadLine();
        }



        private static async Task ConsumeSub<T>(string topicName, string subName, Action<T> receivedAction)
        {
            ISubscriptionClient client = new SubscriptionClient(Constants.ConnectionString, topicName, subName);

            client.RegisterMessageHandler( async (message, ct) => 
            {
                var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));

                receivedAction(model);

                await Task.CompletedTask;
            }, 
            new MessageHandlerOptions(i => Task.CompletedTask));

            Console.WriteLine($"{typeof(T).Name} is listening....");
        }


    }
}
