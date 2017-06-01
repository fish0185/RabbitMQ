using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace StandardQueue
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _model;

        private const string QueueName = "StandardQueue_ExampleQueue";

        static void Main(string[] args)
        {
            var payment1 = new Payment
            {
                AmountToPay = 25.0m, CardNumber = "1111222233334444", Name = "Mr s Gary"
            };
            var payment2 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment3 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment4 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment5 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment6 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            }; var payment7 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            }; var payment8 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment9 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };
            var payment10 = new Payment
            {
                AmountToPay = 25.0m,
                CardNumber = "1111222233334444",
                Name = "Mr s Gary"
            };

            CreateQueue();

            SendMessage(payment1);

            Recieve();

            Console.ReadLine();
        }

        private static void Recieve()
        {
            var consumer = new QueueingBasicConsumer(_model);
            var msgCount = GetMessageCount(_model, QueueName);

            _model.BasicConsume(QueueName, true, consumer);
            var count = 0;

            while (count<msgCount)
            {
                var message = (Payment) consumer.Queue.Dequeue().Body.DeSerialize(typeof(Payment));
                Console.WriteLine("------ Received {0} : {1} : {2}",
                    message.CardNumber, message.AmountToPay, message.Name);
                count--;
            }
        }

        private static uint GetMessageCount(IModel channel, string queueName)
        {
            var results = channel.QueueDeclare(queueName, true, false, false, null);
            return results.MessageCount;
        }

        private static void SendMessage(Payment message)
        {
            _model.BasicPublish("", QueueName, null, message.Serialize());
            Console.WriteLine("[x] Payment Message Sent: {0} : {1} : {2}", message.CardNumber, message.AmountToPay, message.Name);
        }

        private static void CreateQueue()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();

            _model.QueueDeclare(QueueName, true, false, false, null);
        }
    }
}
