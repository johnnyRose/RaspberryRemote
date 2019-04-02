using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RaspberryRemote.Hubs
{
    public class LircHub : Hub
    {
        public async Task ButtonPressed(string arg)
        {
            await Clients.All.SendAsync("InfaredHandler", arg);
        }
    }
}
