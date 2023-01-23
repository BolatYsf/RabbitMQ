﻿

using RabbitMQ.Client;
using RabbitMQ.publisher;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

// conncet rabbitmq cloud

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection=factory.CreateConnection();


var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

Random rnd = new Random();

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    LogNames log1 = (LogNames)rnd.Next(1, 5);
    LogNames log2 = (LogNames)rnd.Next(1, 5);
    LogNames log3 = (LogNames)rnd.Next(1, 5);
    
    // create routekey
    var routeKey = $"{log1}.{log2}.{log3}";
    
    string message = $"log-type:{log1}-{log2}-{log3}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-topic",routeKey, null, messageBody);

    Console.WriteLine($"Log has sended: {message}");
    
});


Console.ReadLine();
