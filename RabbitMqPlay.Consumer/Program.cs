using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

builder.Services.AddHostedService<RabbitMqConsumer>();

var host = builder.Build();
host.Run();

public class RabbitMqConsumer : IHostedService
{
    private readonly IConnection _rabbitConnection;
    private readonly ILogger<RabbitMqConsumer> _logger;
    private IModel? _channel;

    public RabbitMqConsumer(IConnection rabbitConnection, ILogger<RabbitMqConsumer> logger)
    {
        _rabbitConnection = rabbitConnection;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = _rabbitConnection.CreateModel();

        const string queueName = "hello";
        _channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        _logger.LogInformation("[*] Waiting for messages.");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("[x] Received {Message}", message);
        };

        _channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        return Task.CompletedTask;
    }
}