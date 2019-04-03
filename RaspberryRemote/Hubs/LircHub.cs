using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;

namespace RaspberryRemote.Hubs
{
    public class LircHub : Hub
    {
        public void ButtonPressed(string key)
        {
            if (Startup.AllowedKeys != null && Startup.AllowedKeys.Contains(key))
            {
                string command = $"-c \"irsend send_once {Constants.REMOTE_NAME} {key}\"";
                Console.WriteLine(command);

                ProcessStartInfo procInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = command,
                };

                Process proc = Process.Start(procInfo);
            }
        }
    }
}
