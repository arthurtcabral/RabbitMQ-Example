using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Consumer
namespace ConsoleApp2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configuração da conexão com o RabbitMQ
            var factory = new ConnectionFactory { HostName = "localhost" };

            // Usando a nova API assíncrona do RabbitMQ 7+
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            // Declaração da fila
            await channel.QueueDeclareAsync(queue: "hello",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            Console.WriteLine(" [*] Aguardando mensagens. Pressione [enter] para sair.");

            // Configuração do consumidor assíncrono
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Recebido: {message}");
                return Task.CompletedTask;
            };

            // Inicia o consumo das mensagens da fila
            await channel.BasicConsumeAsync(queue: "hello",
                                            autoAck: true,
                                            consumer: consumer);

            Console.ReadLine();
        }
    }
}
