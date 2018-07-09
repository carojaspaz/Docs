﻿#define HandleStopStart // or ServiceOnly ServiceOrConsole
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreService
{
    public class Program
    {
#if ServiceOnly
        #region ServiceOnly
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().RunAsService();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>();
        }
        #endregion
#endif
#if ServiceOrConsole
        #region ServiceOrConsole
        public static void Main(string[] args)
        {
            var isService = true;

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            var host = CreateWebHostBuilder(args).Build();

            if (isService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var isService = true;

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            var pathToContentRoot = Directory.GetCurrentDirectory();

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            WebHost.CreateDefaultBuilder(webHostArgs)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>();
        }
        #endregion
#endif
#if HandleStopStart
        #region HandleStopStart
        public static void Main(string[] args)
        {
            var isService = true;

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            var host = CreateWebHostBuilder(args).Build();

            if (isService)
            {
                host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var isService = true;

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                isService = false;
            }

            var pathToContentRoot = Directory.GetCurrentDirectory();

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var webHostArgs = args.Where(arg => arg != "--console").ToArray();

            return WebHost.CreateDefaultBuilder(webHostArgs)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>();
        }
        #endregion
#endif
    }
}
