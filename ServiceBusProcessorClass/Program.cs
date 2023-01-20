
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using ServiceBusProcessorClass;

string connectionString = "Endpoint=sb://anishservicebus.servicebus.windows.net/;SharedAccessKeyName=anishpolicy;SharedAccessKey=KvCEOy95D+Ia5lHWvObCGw1+OYvetwt2blx1D+d8DFI=;EntityPath=anishqueue";

string queueName = "anishqueue";

string[] Importance = new string[] { "High", "Medium", "Low" };

ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
ServiceBusProcessor serviceBusProcessor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());

serviceBusProcessor.ProcessMessageAsync += ProcessMessage;
serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

await serviceBusProcessor.StartProcessingAsync();
Console.WriteLine("Waiting for messages");
Console.ReadKey();

await serviceBusProcessor.StopProcessingAsync();

await serviceBusProcessor.DisposeAsync();
await serviceBusClient.DisposeAsync();

static async Task ProcessMessage(ProcessMessageEventArgs messageEvent)
{
    Order order = JsonConvert.DeserializeObject<Order>(messageEvent.Message.Body.ToString());
    Console.WriteLine("Order Id {0}", order.OrderID);
    Console.WriteLine("Quantity {0}", order.Quantity);
    Console.WriteLine("Unit Price {0}", order.UnitPrice);

}

static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
