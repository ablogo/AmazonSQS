using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AmazonSqs.Interfaces
{
    public interface IMessage
    {
        Task<SendMessageResponse> Send(string message);

        Task<ReceiveMessageResponse> Receive();

        Task<bool> Delete(string receiptHandle);

        Task<CreateQueueResponse> CreateQueue(string queueName);

    }
}
