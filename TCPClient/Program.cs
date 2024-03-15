using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;

namespace TCPClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new TcpClient();
            client.Connect("127.0.0.1", 27001);

            var stream = client.GetStream();
            var br = new BinaryReader(stream);
            var bw = new BinaryWriter(stream);

            while (true)
            {
                var str = Console.ReadLine().ToUpper();
                if (str == "HELP")
                {
                    Console.WriteLine(Command.ProcList);
                    Console.WriteLine(Command.Run);
                    Console.WriteLine("HELP");
                    continue;
                }
                Command cmd = null;
                string response = null;

                var input = str.Split(' ');
                Console.WriteLine(input[0]);
                switch (input[0])
                {
                    case Command.ProcList:
                        cmd = new Command { Text = input[0] };
                        bw.Write(JsonSerializer.Serialize(cmd));

                        response = br.ReadString();

                        var processes = JsonSerializer.Deserialize<string[]>(response);

                        foreach (var process in processes)
                        {
                            Console.WriteLine(process);
                        }
                        break;
                    case Command.Run:
                        //realization 
                        break;
                    case Command.Start:


                        if (input[1] != null) {

                            cmd = new Command { Text = input[0] };
                            bw.Write(JsonSerializer.Serialize(cmd));
                            Process.Start(input[1] + ".exe");
                        
                        
                        }
                        break;
                    default:
                        break;
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