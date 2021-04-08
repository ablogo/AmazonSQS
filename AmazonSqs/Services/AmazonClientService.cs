using Amazon.SQS;
using Amazon.SQS.Model;
using AmazonSqs.Dtos;
using AmazonSqs.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AmazonSqs.Services
{
    public class AmazonClientService : IAmazonClient<AmazonSQSClient>
    {
        private AmazonSQSClient _amazonSQSClient;
        private readonly AmazonConfiguration _amazonConfiguration;

        public AmazonClientService(IOptions<AmazonConfiguration> amazonConfiguration)
        {
            _amazonConfiguration = amazonConfiguration.Value;
            _ = GetClient(_amazonConfiguration.ServiceURL);
        }

        public async Task<AmazonSQSClient> GetClient(string url)
        {
            try
            {
                if (_amazonSQSClient == null)
                {
                    var sqsConfig = new AmazonSQSConfig();
                    sqsConfig.ServiceURL = url;

                    _amazonSQSClient = new AmazonSQSClient(_amazonConfiguration.AccessKeyId, _amazonConfiguration.SecretAccessKey, sqsConfig);
                }
                else 
                {
                    return _amazonSQSClient;
                }

            }
            catch (Exception ex) 
            {
            }
            return _amazonSQSClient;
        }

        public async Task<string> GetQueueUrl(string QueueName, string QueueOwnerAWSAccountId)
        {
            string url = "";
            try
            {
                var request = new GetQueueUrlRequest()
                {
                    QueueName = QueueName,
                    QueueOwnerAWSAccountId = QueueOwnerAWSAccountId
                };
                var response = await _amazonSQSClient.GetQueueUrlAsync(request);
                url = response.QueueUrl;
            }
            catch (Exception ex) 
            {
            }
            return url;
        }
    }
}
