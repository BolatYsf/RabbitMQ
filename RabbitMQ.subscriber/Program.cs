 

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

//channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

//var randomQueueName = channel.QueueDeclare().QueueName;

var randomQueueName = "log-db-queue";

channel.QueueDeclare(randomQueueName,true,false,false);

// bind fanout

channel.QueueBind(randomQueueName,"logs-fanout","",null);

channel.BasicQos(0,1,false);


var consumer=new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueueName,false,consumer);

Console.WriteLine("logs listening...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);

    Console.WriteLine("Message:" + message);

    // u can delete sending message queue
    channel.BasicAck(e.DeliveryTag, false);
};





Console.ReadLine();