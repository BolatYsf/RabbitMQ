

using RabbitMQ.Client;
using System.Configuration;
using System.Text;

var factory = new ConnectionFactory();

// conncet rabbitmq cloud

string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ToString();

factory.Uri = new Uri(conStr);

using var connection=factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

// using dictionary class

Dictionary<string,object> headers= new Dictionary<string, object>();

headers.Add("format", "png");
headers.Add("width", "200px");

var properties=channel.CreateBasicProperties();

properties.Headers = headers;

// properties become permanent 
properties.Persistent = true;

channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header message"));

Console.WriteLine("Message has sended..");


Console.ReadLine();
