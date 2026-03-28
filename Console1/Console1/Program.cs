using RabbitMQ.Client;
using System.Text;

//Producer
namespace ConsoleApp1
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

            // Mensagem a ser enviada
            string message = "Hello World! Esta é uma mensagem de teste.";
            var body = Encoding.UTF8.GetBytes(message);

            // Publicação da mensagem na fila
            await channel.BasicPublishAsync(exchange: string.Empty,
                                            routingKey: "hello",
                                            body: body);

            Console.WriteLine($" [x] Enviado: {message}");

            Console.WriteLine(" Pressione [enter] para sair.");
            Console.ReadLine();
        }
    }
}
