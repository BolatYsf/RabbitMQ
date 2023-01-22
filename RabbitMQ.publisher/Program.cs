

using RabbitMQ.Client;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

// conncet rabbitmq cloud

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();


factory.Uri = new Uri(conStr);




using var connection=factory.CreateConnection();


var channel = connection.CreateModel();

// create fanoutexchange

//channel.ExchangeDeclare("logs-fanout")

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    // write message and has send

    string message = $"Message{x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-rabbitmq", null, messageBody);

    Console.WriteLine($"Message has sended: {message}");
    
});


Console.ReadLine();
