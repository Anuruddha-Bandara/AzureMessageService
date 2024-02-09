using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private static IQueueClient queueClient;
        private const string ServiceBusConnectionString = "";
        private const string QueueName = "testingqueue";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAsyncData();
        }
        private void LoadAsyncData()
        {
            Task.Run(async () => await MainAsync());
        }
        private static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);

            MessageBox.Show("Press ctrl-c to stop receiving messages.");

            ReceiveMessages();

            Console.ReadKey();
            // Close the client after the ReceiveMessages method has exited. 
            await queueClient.CloseAsync();
        }

        // Receives messages from the queue in a loop 
        private static void ReceiveMessages()
        {
            try
            {
                // Register a OnMessage callback 
                queueClient.RegisterMessageHandler(
                    async (message, token) =>
                    {
                        // Process the message 
                        MessageBox.Show($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

                        // Complete the message so that it is not received again. 
                        // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode. 
                        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                    },
                    new MessageHandlerOptions(exceptionReceivedEventArgs =>
                    {
                        MessageBox.Show($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
                        return Task.CompletedTask;
                    })
                    { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{DateTime.Now} > Exception: {exception.Message}");
            }
        }

    }
}
