using System.Text.Json;
using Azure.Storage.Queues;
using ImageProcessing.API.Services.Interfaces;

namespace ImageProcessing.API.Services;
public class QueuesManagement : IQueuesManagement
{
    public async Task<bool> SendMessage<T>(T serviceMessage, string queue, string connectionString)
    {
        // create a queue client
        var queueClient = new QueueClient(connectionString, queue);

        var msgBody = JsonSerializer.Serialize(serviceMessage);

        await queueClient.SendMessageAsync(msgBody);

        return true;
    }
}
