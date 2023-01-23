

using RabbitMQ.Client;
using RabbitMQ.publisher;
using System.Configuration;
using System.Text;



var factory = new ConnectionFactory();

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection=factory.CreateConnection();

var channel = connection.CreateModel();

// create directexchange

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

var logList = Enum.GetNames(typeof(LogNames)).ToList();

foreach (var item in logList)
{
    var queueName = $"direct-queue-{item}";

    channel.QueueDeclare(queueName,true,false,false);

    var routeKey = $"route-{item}";
    // bind que

    channel.QueueBind(queueName,"logs-direct",routeKey,null);
}


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log =(LogNames) new Random().Next(1, 5);


    // ll write log type

    string message = $"log-type:{log}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    // add route 

    var routeKey=$"route-{log}";

    channel.BasicPublish("logs-direct", routeKey, null, messageBody);

    Console.WriteLine($"Log has sended: {message}");
    
});


Console.ReadLine();
