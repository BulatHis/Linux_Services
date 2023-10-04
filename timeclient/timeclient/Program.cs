using System;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Это правильный IP адрес  - 127.0.0.1?");
        Console.WriteLine("Если да - нажмите Enter, если нет, то напишите - 'no'");
        string serverIp = "127.0.0.1";
        string answer = Console.ReadLine();
        
        if (answer == "no")
        {
            Console.WriteLine("Введите IP адрес сервера");
            serverIp = Console.ReadLine();
        }
        ConnectToServer(serverIp).Wait();
    }

    private static async Task ConnectToServer(string serverIp)
    {
        TcpClient client = new TcpClient();

        try
        {
            await client.ConnectAsync(serverIp, 1303);

            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Получен ответ от сервера: " + response);
                await Task.Delay(4000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка подключения к серверу: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
