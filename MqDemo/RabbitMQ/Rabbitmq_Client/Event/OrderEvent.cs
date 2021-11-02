using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Rabbitmq_Client.Event
{
    public class OrderEvent
    {
        static IConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "39.105.136.203",
            UserName = "admin",
            Password = "ad456ad",
            VirtualHost = "/",
            Port = 5672
        };

        public void Subscriber(string exchanges, string queueName, TimeSpan timeout, CancellationToken cancellationToken)
        {
            string dlxQueueName = "dlx.queue";

            var connection = factory.CreateConnection();
            {
                //创建信道
                var channel = connection.CreateModel();
                {
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: true);
                    consumer.Received += (model, ea) =>
                    {
                        //处理业务
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine($"{DateTime.Now}队列{dlxQueueName}消费消息:{message}");
                        //不ack(BasicNack),且不把消息放回队列(requeue:false)
                        System.Threading.Thread.Sleep(20);
                        channel.BasicAck(ea.DeliveryTag, false);
                        //channel.BasicNack(ea.DeliveryTag, false, requeue: true);
                    };
                    channel.BasicConsume(dlxQueueName, autoAck: false, consumer);
                }
            }
        }
    }
}
