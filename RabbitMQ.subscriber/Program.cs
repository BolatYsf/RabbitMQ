

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0,1,false);

var consumer=new EventingBasicConsumer(channel);

var queuename=channel.QueueDeclare().QueueName;

// ll only listen to the route that write Error in the middle

//var routeKey = "*.Error.*";

// ending with warring

//var routeKey = "*.*.Warning";

// starts with Info then could be any one

//var routeKey = "Info.#";

var routeKey = "*.Info.Error";

channel.QueueBind(queuename,"logs-topic",routeKey);

channel.BasicConsume(queuename,false,consumer);

Console.WriteLine("Logs loadin...");

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);

    Console.WriteLine("Message:" + message);

    // u can delete sending message queue
    channel.BasicAck(e.DeliveryTag, false);
};


Console.ReadLine();