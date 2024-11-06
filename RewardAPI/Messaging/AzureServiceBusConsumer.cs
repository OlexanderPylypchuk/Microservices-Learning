
using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using RewardAPI.Models.DTOs;
using RewardAPI.Services.IServices;

namespace RewardAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string _serviceBusConnectionString;
		private readonly string _orderCreatedTopic;
		private readonly string _orderCreatedRewardsUpdateSubscription;
		private readonly IConfiguration _configuration;

		private ServiceBusProcessor _rewardProcessor;

		private readonly IRewardService _rewardService;

		public AzureServiceBusConsumer(IConfiguration configuration, IRewardService rewardService)
		{
			_configuration = configuration;

			_serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

			_orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
			_orderCreatedRewardsUpdateSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

			_rewardService = rewardService;

			var client = new ServiceBusClient(_serviceBusConnectionString);

			_rewardProcessor = client.CreateProcessor(_orderCreatedTopic, _orderCreatedRewardsUpdateSubscription);
		}

		private async Task OnRewardUpdateRecieved(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = UTF8Encoding.UTF8.GetString(message.Body);

			RewardsDTO rewardsDTO = JsonConvert.DeserializeObject<RewardsDTO>(body);
			try
			{
				await _rewardService.UpdateRewards(rewardsDTO);
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
			_rewardProcessor.ProcessMessageAsync += OnRewardUpdateRecieved;
			_rewardProcessor.ProcessErrorAsync += ErrorHandler;

			await _rewardProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await _rewardProcessor.StopProcessingAsync();
			await _rewardProcessor.DisposeAsync();
		}
	}
}
