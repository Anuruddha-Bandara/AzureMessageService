using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
namespace AzureMessagePublisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
           
                string connectionString = "";
            QueueClient queueClient = new QueueClient(connectionString, "testingqueue");
            for (int i = 0; i < 2; i++)
            {
                string messageBody = "Thisssss hari meeee   Mama Azure Service Bus !  "+i.ToString();
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(message);
        }
            Console.WriteLine("Hello World! AzureMessagePublisher");
        }
    }
}
