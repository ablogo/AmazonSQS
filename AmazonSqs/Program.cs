using Amazon.SQS;
using AmazonSqs.Dtos;
using AmazonSqs.Interfaces;
using AmazonSqs.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AmazonSqs
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            // Set message to send
            string message = "";

            // Calls the Run method in App, which is replacing Main
            var sendMessageResponse = serviceProvider.GetService<IMessage>().Send(message).Result;
            var receiveMessageResponse = serviceProvider.GetService<IMessage>().Receive().Result;

            if (receiveMessageResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var item in receiveMessageResponse.Messages)
                {
                    var deleteResponse = serviceProvider.GetService<IMessage>().Delete(item.ReceiptHandle).Result;
                }
            }
            var createQueueResponse = serviceProvider.GetService<IMessage>().CreateQueue("test").Result;


        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            try
            {
                var config = LoadConfiguration();
                services.AddSingleton(config);
                services.AddSingleton<IAmazonClient<AmazonSQSClient>, AmazonClientService>();
                services.AddSingleton<IMessage, MessageService>();
                var x = config.GetSection("Aws");
                services.Configure<AmazonConfiguration>(config.GetSection("Aws"));
            }
            catch (Exception ex) { }
            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
