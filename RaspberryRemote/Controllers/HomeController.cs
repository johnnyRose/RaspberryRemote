using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RaspberryRemote.Hubs;
using RaspberryRemote.Models;
using System;
using System.Diagnostics;

namespace RaspberryRemote.Controllers
{
    public class HomeController : Controller
    {
        private static IHubContext<LircHub> _hubContext;
        private static volatile bool startedIrw = false;

        public HomeController(IHubContext<LircHub> hubContext)
        {
            if (_hubContext == null)
            {
                _hubContext = hubContext;
            }

            if (!startedIrw)
            {
                startedIrw = true;

                ProcessStartInfo procInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = "-c \"irw\"",
                };

                Process proc = Process.Start(procInfo);

                Console.WriteLine("Registering process callback...");
                proc.OutputDataReceived += (sender, args) =>
                {
                    string[] data = args.Data.Split(' ');
                    string sequence = data[1];
                    string key = data[2];

                    if (sequence == "00")
                    {
                        Console.WriteLine("Debug: got " + args.Data);
                        _hubContext.Clients.All.SendAsync("InfaredHandler", key);
                    }
                };

                proc.BeginOutputReadLine();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
