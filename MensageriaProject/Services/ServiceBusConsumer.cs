using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace MensageriaProject.Services
{
    public class ServiceBusConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private ServiceBusClient _client;
        private ServiceBusProcessor _processor;
        private string _lastMessageBody;
        private readonly List<string> _emails = new();

        public ServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connectionString = _configuration["AzureServiceBus:ConnectionString"];
            var queueName = _configuration["AzureServiceBus:QueueName"];

            _client = new ServiceBusClient(connectionString);
            _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await _processor.StartProcessingAsync(cancellationToken);
            Console.WriteLine("🟢 Consumidor da fila iniciado...");
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var email = args.Message.Body.ToString();
            _emails.Add(email); // Adiciona o e-mail à lista interna
            await args.CompleteMessageAsync(args.Message);
        }

        public List<string> GetEmails()
        {
            return _emails;
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Erro: {args.Exception.Message}");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("🛑 Consumidor da fila finalizando...");

            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            if (_client != null)
            {
                await _client.DisposeAsync();
            }
        }
    }
}
