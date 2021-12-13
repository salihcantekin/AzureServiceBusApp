using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using ServicebusApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicebusApp.ProducerApi.Services
{
    public class AzureService
    {
        private readonly ManagementClient managementClient;

        public AzureService(ManagementClient managementClient)
        {
            this.managementClient = managementClient;
        }

        public async Task SendMessageToQueue(string queueName, object messageContent, string messageType = null)
        {
            IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);

            await SendMessage(client, messageContent, messageType);
        }

        public async Task CreateQueueIfNotExists(string queueName)
        {
            if (!await managementClient.QueueExistsAsync(queueName))
                await managementClient.CreateQueueAsync(queueName);
        }

        public async Task SendMessageToTopic(string topicName, object messageContent, string messageType = null)
        {
            ITopicClient client = new TopicClient(Constants.ConnectionString, topicName);

            await SendMessage(client, messageContent, messageType);
        }

        public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName, string messageType = null, string ruleName = null)
        {
            if (await managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
                return;

            if (messageType != null)
            {
                SubscriptionDescription sd = new(topicName, subscriptionName);

                CorrelationFilter filter = new();
                filter.Properties["MessageType"] = messageType;

                RuleDescription rd = new(ruleName ?? messageType + "Rule", filter);

                await managementClient.CreateSubscriptionAsync(sd, rd);
            }
            else
                await managementClient.CreateSubscriptionAsync(topicName, subscriptionName);
        }

        public async Task CreateTopicIfNotExists(string topicName)
        {
            if (!await managementClient.TopicExistsAsync(topicName))
                await managementClient.CreateTopicAsync(topicName);
        }




        private async Task SendMessage(ISenderClient client, object messageContent, string messageType = null)
        {
            var byteArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            var message = new Message(byteArr);
            message.UserProperties["MessageType"] = messageType;

            await client.SendAsync(message);
        }

    }
}
