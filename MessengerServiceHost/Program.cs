using System.ServiceModel;
using System;

namespace WCFMessengerServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost selfHost = new ServiceHost(typeof(MessengerService.ServiceLogic.MessengerService));

            selfHost.Open();
            Console.WriteLine("The service is ready.");
            Console.WriteLine("Press any key to terminate the service.");
            Console.ReadKey();
            selfHost.Close();
        }
    }
}