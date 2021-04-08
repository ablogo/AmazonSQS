using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AmazonSqs.Interfaces
{
    public interface IAmazonClient<T>
    {
        Task<T> GetClient(string url);

        Task<string> GetQueueUrl(string QueueName, string QueueOwnerAWSAccountId);
    }
}
