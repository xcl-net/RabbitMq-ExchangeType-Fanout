using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace 队列queue2绑定到交换机并创建消费者consumer2
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.HostName = "127.0.0.1";
            factory.UserName = "guest";
            factory.Password = "guest";
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            //声明一个交换机
            channel.ExchangeDeclare("myFanoutExchange", ExchangeType.Fanout, true, false, null);
            Console.WriteLine("交换机创建完成：myFanoutExchange ...");
            channel.QueueDeclare("myFanoutQueue2", true, false, false, null);
            Console.WriteLine("队列创建完成：myFanoutQueue2 ...");
            channel.QueueBind("myFanoutQueue2", "myFanoutExchange", string.Empty, null);
            var consumer = new EventingBasicConsumer(channel);
            Console.WriteLine("队列：myFanoutQueue2的消费者，创建完成 ...");
            consumer.Received += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine(msg);
            };
            channel.BasicConsume("myFanoutQueue2", true, consumer);
            Console.WriteLine("consumer 启动启动完成。");
            Console.Read();
        }
    }
}
