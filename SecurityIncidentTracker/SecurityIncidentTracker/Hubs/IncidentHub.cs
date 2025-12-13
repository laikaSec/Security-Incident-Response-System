using Microsoft.AspNetCore.SignalR;

namespace SecurityIncidentTracker.Hubs
{
    // This Hub manages real-time connections.
    // Clients (browsers) connect to this hub to receive updates.
    public class IncidentHub : Hub
    {
        // We don't need any special methods here yet.
        // The server (IncidentService) will use this Hub to send messages OUT to clients.
        // Clients just listen.
    }
}
