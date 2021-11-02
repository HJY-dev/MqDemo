using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitmq_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        static IConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "39.105.136.203",
            UserName = "admin",
            Password = "ad456ad",
            VirtualHost = "/",
            Port = 5672
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            //延时交换机
            string delayExchange = "dlx.exchange";
            //延时队列
            string delayQueueName = "dlx.queue";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //创建延时交换机
                    channel.ExchangeDeclare(delayExchange, type: "x-delayed-message", durable: true, autoDelete: false, new Dictionary<string, object> {
                        { "x-delayed-type","direct"}
                    });
                    //创建死信队列
                    channel.QueueDeclare(delayQueueName, durable: true, exclusive: false, autoDelete: false);
                    //死信队列绑定死信交换机
                    channel.QueueBind(delayQueueName, delayExchange, routingKey: delayQueueName);

                    string message = "hello rabbitmq message 10s后处理";
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.Headers = new Dictionary<string, object> { { "x-delay", "10000" } };
                    //发布消息
                    channel.BasicPublish(exchange: delayExchange,
                                     routingKey: delayQueueName,
                                     basicProperties: properties,
                                     body: Encoding.UTF8.GetBytes(message));
                    Console.WriteLine($"{DateTime.Now}向队列:{delayQueueName}发送消息:{message}");
                }
            }

            return "OK";
        }
    }
}
