using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicebusApp.Common
{
    public static class Constants
    {
        public const string ConnectionString = "Endpoint=sb://techbuddy.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DQSCCs+m5BliFyO+NAuwl2bbYgzZEE2st+mUWfBCwSs=";

        public const string OrderCreatedQueueName = "OrderCreatedQueue";
        public const string OrderDeletedQueueName = "OrderDeletedQueue";

        public const string OrderTopic = "OrderTopic";
        public const string OrderCreatedSubName = "OrderCreatedSub";
        public const string OrderDeletedSubName = "OrderDeletedSub";
    }
}
