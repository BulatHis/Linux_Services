
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await StartServerAsync();
    }

    private static async Task StartServerAsync()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 1303);
        listener.Start();
        Console.WriteLine("timeserver started. Listening on port 1303...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");

            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    string response = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    byte[] data = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Close();
                Console.WriteLine("Connection closed.");
            }
        }
    }
}