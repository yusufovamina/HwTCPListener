using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace HwTCPListener
{
    internal class Program
    {
        static void Main(string[] args)
        {



            var ip = IPAddress.Parse("127.0.0.1");
            var listener = new TcpListener(ip, 27001);
            listener.Start(100);

            while (true)
            {
                var client = listener.AcceptTcpClient();
                var stream = client.GetStream();
                var br = new BinaryReader(stream); //binary reader
                var bw = new BinaryWriter(stream); //binary writer

                while (true)
                {
                    var input = br.ReadString();
                    var command = JsonSerializer.Deserialize<Command>(input);

                    if (command == null)
                    {
                        continue;
                    }
                    Console.WriteLine(command.Text);
                    Console.WriteLine(command.Param);
                    string[] arr=command.Text.Split(" ");

                    switch (command.Text)
                    {
                        case Command.ProcList:
                            var processes = Process.GetProcesses();
                            bw.Write(JsonSerializer.Serialize(processes.Select(p => p.ProcessName)));
                            break;
                        case Command.Run:
                            //realization 
                            break;
                        case Command.Start:
                          
                              
                            bw.Write(JsonSerializer.Serialize(arr[1] + " is opened"));
                            break;
                            
                        default:
                            break;
                    }

                }

            }


        }

        public class Command
        {
            public const string ProcList = "PROCLIST";
            public const string Run = "RUN";
            public const string Start = "START";

            public string Text { get; set; }
            public string Param { get; set; }

        }
    }
}