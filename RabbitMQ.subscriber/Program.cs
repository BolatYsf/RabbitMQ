

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

// ll connect critical queue
var queueName = "direct-queue-Info";

channel.BasicConsume(queueName,false,consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);

    Console.WriteLine("Message:" + message);

    // ll write messages in txt file
    // File.AppendAllText("log-info.txt", message + "\n");

    // u can delete sending message queue
    channel.BasicAck(e.DeliveryTag, false);
};





Console.ReadLine();