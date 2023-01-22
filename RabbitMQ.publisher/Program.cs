

using RabbitMQ.Client;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection = factory.CreateConnection();

// create channel rabbitmq

var channel = connection.CreateModel();

// add queue in channel

channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    // write message and has send

    string message = $"log {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout","", null, messageBody);

    Console.WriteLine($"Message has sended: {message}");

});


Console.ReadLine();
