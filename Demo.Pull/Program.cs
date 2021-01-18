using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.Pull
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var connection = Connection.GetConnection();


            var app = connection.GetAppInstance();


            connection.AddRequestCount(app.SessionId);
            app.FazAlgo();
            connection.ResetCall(app.SessionId);




            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
