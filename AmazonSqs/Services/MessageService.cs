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
    public class MessageService : IMessage
    {
        private readonly IAmazonClient<AmazonSQSClient> _amazonClient;
        private readonly AmazonSQSClient _sqsClient;
        private readonly AmazonConfiguration _amazonConfiguration;

        public MessageService(IAmazonClient<AmazonSQSClient> amazonClient, IOptions<AmazonConfiguration> amazonConfiguration)
        {
            _amazonClient = amazonClient;
            _amazonConfiguration = amazonConfiguration.Value;
            _sqsClient = _amazonClient.GetClient(_amazonConfiguration.ServiceURL).Result;
        }

        public async Task<ReceiveMessageResponse> Receive()
        {
            ReceiveMessageResponse response = null;
            try
            {
                var receiveMessageRequest = new ReceiveMessageRequest()
                {
                    VisibilityTimeout = Convert.ToInt32(_amazonConfiguration.VisibilityTimeout),
                    MaxNumberOfMessages = Convert.ToInt32(_amazonConfiguration.MaxNumberOfMessages),
                    WaitTimeSeconds = Convert.ToInt32(_amazonConfiguration.WaitTimeSeconds)
                };
                receiveMessageRequest.QueueUrl = _amazonConfiguration.QueueUrl;

                response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);
            }
            catch (Exception ex) 
            {
            }
            return response;
        }

        public async Task<SendMessageResponse> Send(string message)
        {
            SendMessageResponse sendMessageResponse = null;
            try
            {
                var sendMessageRequest = new SendMessageRequest();

                sendMessageRequest.QueueUrl = await _amazonClient.GetQueueUrl(_amazonConfiguration.QueueName, _amazonConfiguration.AccountId);
                sendMessageRequest.MessageBody = StringToBase64(message);

                sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);
            }
            catch (Exception ex)
            {
            }
            return sendMessageResponse;
        }

        public async Task<bool> Delete(string receiptHandle)
        {
            bool result = false;
            try
            {
                var deleteMessageRequest = new DeleteMessageRequest();

                deleteMessageRequest.QueueUrl = await _amazonClient.GetQueueUrl(_amazonConfiguration.QueueName, _amazonConfiguration.AccountId);
                deleteMessageRequest.ReceiptHandle = receiptHandle;

                var response = await _sqsClient.DeleteMessageAsync(deleteMessageRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) result = true;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public async Task<CreateQueueResponse> CreateQueue(string queueName)
        {
            CreateQueueResponse createQueueResponse = null;
            try
            {
                var createQueueRequest = new CreateQueueRequest();

                createQueueRequest.QueueName = queueName;
                var attrs = new Dictionary<string, string>();
                attrs.Add(QueueAttributeName.VisibilityTimeout, _amazonConfiguration.VisibilityTimeout);
                createQueueRequest.Attributes = attrs;

                createQueueResponse = await _sqsClient.CreateQueueAsync(createQueueRequest);

            }
            catch (Exception ex)
            {
            }
            return createQueueResponse;
        }

        private string StringToBase64(string stringToBase64)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringToBase64))
                {
                    var textBytes = Encoding.UTF8.GetBytes(stringToBase64);
                    return Convert.ToBase64String(textBytes);
                }
            }
            catch (Exception ex) { throw ex; }
            return "";
        }


    }
}
