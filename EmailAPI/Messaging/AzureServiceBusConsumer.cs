using System.Text;
using Azure.Messaging.ServiceBus;
using EmailAPI.Models.Dto;
using EmailAPI.Services.IServices;
using Newtonsoft.Json;

namespace EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        private ServiceBusProcessor _emailCartProcessor; 

        public AzureServiceBusConsumer(IConfiguration configuration, IEmailSender emailSender)
        {
            _configuration = configuration;

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);

            _emailSender = emailSender;
        }

        private async Task OnEmailRequestRecieved(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = UTF8Encoding.UTF8.GetString(message.Body);

            CartDTO cart = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailSender.EmailCartAndLog(cart);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

		public async Task Start()
		{
            _emailCartProcessor.ProcessMessageAsync += OnEmailRequestRecieved;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
		}
	}
}
