using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonSqs.Dtos
{
    public class AmazonConfiguration
    {
        public string AccessKeyId { get; set; }

        public string AccountId { get; set; }

        public string SecretAccessKey { get; set; }
        
        public string ServiceURL { get; set; }
        
        public string DefaultRegion { get; set; }
        
        public string QueueName { get; set; }

        public string QueueUrl { get; set; }

        public string VisibilityTimeout { get; set; }

        public string MaxNumberOfMessages { get; set; }

        public string WaitTimeSeconds { get; set; }

    }
}
