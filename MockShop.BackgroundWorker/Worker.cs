using Microsoft.EntityFrameworkCore.Metadata;
using MockShop.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MockShop.BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private RabbitMQ.Client.IModel _channel;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            // Connect RabbitMQ while service is constructed
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "orders_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Dinleyicisi Ba?lat?ld?... Mesaj bekleniyor.");

            var consumer = new EventingBasicConsumer(_channel);

            // Event that triggers when a message is received
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"[RabbitMQ] Yeni Mesaj Yakaland?: {message}");

                try
                {
                    // Read recieved JSON message
                    var orderData = JsonSerializer.Deserialize<OrderMessage>(message);

                    // Open a new scope to get scoped services like DbContext
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                        // Veritaban?n? güncelle
                        await orderRepo.UpdateOrderStatusAsync(orderData.OrderId, "Shipped");

                        _logger.LogInformation($"[DB] Sipari? {orderData.OrderId} durumu 'Shipped' yap?ld?.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Hata olu?tu: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "orders_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        // Dispose override to close RabbitMQ connection
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    
    public class OrderMessage
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
    }

}
