using Grpc.Core;
using Nodes.Pathing;

namespace Pathing
{
    internal class PathingServer
    {
        public const int Port = 20023;
        private static void Main(string[] args)
        {
            var pathingServiceImpl = new PathingServiceImpl();
            Console.WriteLine("Initializing Pathing Service...");
            pathingServiceImpl.Initialize();
            Console.WriteLine("Pathing Service Successfully Initialized...");
            var server = new Server
            {
                Services = { PathingService.BindService(pathingServiceImpl) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();
            while(true)
            {
                Thread.Sleep(250);
            }
        }
    }
}