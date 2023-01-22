

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();

factory.Uri = new Uri("amqps://imfunstt:bieg3kNUrb49XGjMrwQ3NUiSt2fYbdsq@shark.rmq.cloudamqp.com/imfunstt");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

//if u create queue in publisher . u would delete queue here!

//channel.QueueDeclare("hello-rabbitmq", true, false, false);

// create consumer


channel.BasicQos(0,1,false);


var consumer=new EventingBasicConsumer(channel);

channel.BasicConsume("hello-rabbitmq",false,consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1500);

    Console.WriteLine("Message:" + message);

    // u can delete sending message queue
    channel.BasicAck(e.DeliveryTag, false);
};





Console.ReadLine();