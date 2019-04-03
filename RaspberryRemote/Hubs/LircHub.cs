using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RaspberryRemote.Hubs
{
    public class LircHub : Hub
    {
        public async Task ButtonPressed(string button)
        {
            if (/*Startup.AllowedKeys != null && Startup.AllowedKeys.Contains(button)*/true)
            {
                await Clients.All.SendAsync("InfaredHandler", Startup.AllowedKeys);

            }
            //await Clients.All.SendAsync("InfaredHandler", arg);
        }
    }
}
