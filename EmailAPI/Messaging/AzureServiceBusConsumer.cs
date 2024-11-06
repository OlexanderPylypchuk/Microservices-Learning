using System.Text;
using Azure.Messaging.ServiceBus;
using EmailAPI.Models.Dto;
using EmailAPI.Models.DTOs;
using EmailAPI.Services.IServices;
using Newtonsoft.Json;

namespace EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _emailRegisterQueue;
		private readonly string _orderCreatedTopic;
		private readonly string _orderCreatedRewardsEmailSubscription;
		private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        private ServiceBusProcessor _emailCartProcessor; 
        private ServiceBusProcessor _emailRegisterProcessor;
		private ServiceBusProcessor _orderCreatedProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, IEmailSender emailSender)
        {
            _configuration = configuration;

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _emailRegisterQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegisterQueue");
			_orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
			_orderCreatedRewardsEmailSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

			var client = new ServiceBusClient(_serviceBusConnectionString);

            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
			_emailRegisterProcessor = client.CreateProcessor(_emailRegisterQueue);
			_orderCreatedProcessor = client.CreateProcessor(_orderCreatedTopic, _orderCreatedRewardsEmailSubscription);

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

		private async Task OnUserRegisterRecieved(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = UTF8Encoding.UTF8.GetString(message.Body);

			string email = JsonConvert.DeserializeObject<string>(body);
			try
			{
				await _emailSender.EmailRegisterAndLog(email);
				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private async Task OnOrderCreatedRecieved(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = UTF8Encoding.UTF8.GetString(message.Body);

			RewardsDTO rewardsDTO = JsonConvert.DeserializeObject<RewardsDTO>(body);
			try
			{
				await _emailSender.LogOrderPlaced(rewardsDTO);
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

			_emailRegisterProcessor.ProcessMessageAsync += OnUserRegisterRecieved;
			_emailRegisterProcessor.ProcessErrorAsync += ErrorHandler;

			await _emailRegisterProcessor.StartProcessingAsync();

			_orderCreatedProcessor.ProcessMessageAsync += OnOrderCreatedRecieved;
			_orderCreatedProcessor.ProcessErrorAsync += ErrorHandler;

			await _orderCreatedProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

			await _emailRegisterProcessor.StopProcessingAsync();
			await _emailRegisterProcessor.DisposeAsync();

			await _orderCreatedProcessor.StopProcessingAsync();
			await _orderCreatedProcessor.DisposeAsync();
		}
	}
}
