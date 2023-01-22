

using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();

// conncet rabbitmq cloud

factory.Uri = new Uri("amqps://imfunstt:bieg3kNUrb49XGjMrwQ3NUiSt2fYbdsq@shark.rmq.cloudamqp.com/imfunstt");

using var connection=factory.CreateConnection();

// create channel rabbitmq

var channel = connection.CreateModel();

// add queue in channel

channel.QueueDeclare("hello-rabbitmq",true,false,false);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    // write message and has send

    string message = $"Message{x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, "hello-rabbitmq", null, messageBody);

    Console.WriteLine($"Message has sended: {message}");

});


Console.ReadLine();
