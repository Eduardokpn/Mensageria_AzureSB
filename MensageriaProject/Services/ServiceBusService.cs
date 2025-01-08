using Azure.Messaging.ServiceBus;
using MensageriaProject.Interfaces;
using Microsoft.Azure.Amqp.Framing;

namespace MensageriaProject.Services
{
    public class ServiceBusService
    {
        private readonly string _conncetionString;
        private readonly ServiceBusClient _client;
        private readonly string _queueName;
        private readonly ServiceBusReceiver _receiver;
        private readonly IEmailService _emailService;
        public ServiceBusService(IConfiguration configuration, ServiceBusClient client, IEmailService emailService) 
        {
            _conncetionString = configuration["AzureServiceBus:ConnectionString"];
            _queueName = configuration["AzureServiceBus:QueueName"];
            _client = client;
            _receiver = _client.CreateReceiver(configuration["AzureServiceBus:QueueName"]);
            _emailService = emailService;
        }

        public async Task EnviarMensagemAsync(string menssage)
        {
            await using var client = new ServiceBusClient(_conncetionString);
            var sender = client.CreateSender(_queueName);

            try
            {
                var messageBody = new ServiceBusMessage(menssage);
                await sender.SendMessageAsync(messageBody);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }

        public async Task<List<string>> ReceberMensagensAsync()
        {
            var mensagens = new List<string>();

            var receivedMessages = await _receiver.ReceiveMessagesAsync(10); // Recebe até 10 mensagens

            foreach (var message in receivedMessages)
            {
                mensagens.Add(message.Body.ToString());

                foreach (var mensagem in mensagens)
                {
                    var email = mensagem; // Ajuste conforme o formato da mensagem
                    await _emailService.SendEmailResendAsync(email);

                }


                await _receiver.CompleteMessageAsync(message); // Marca como processada
            }

            return mensagens;
        }

    }
}
