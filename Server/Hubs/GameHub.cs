using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;

namespace Server.Hubs;

public class GameHub : Hub
{
    // Players list that save the players connection id and group id
    private static Dictionary<string, string> players = new Dictionary<string, string>();

    /// <summary>
    /// When player join in the game
    /// </summary>
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("OnConnectedAsync");
        // Add player to the players list and set the group id to -2
        players.Add(Context.ConnectionId, "-2");

        return base.OnConnectedAsync();
    }



    /// <summary>
    /// When player disconnected
    /// </summary>
    public override Task OnDisconnectedAsync(Exception exception)
    {
        string groupName = players[Context.ConnectionId];

        // Remove player from the players list and send message to other player to tell him the opponent left
        players.Remove(Context.ConnectionId);
        Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Search for an alone opponent to connect them together
    /// </summary>
    public void SearchOpponent()
    {
        Console.WriteLine("SearchOpponent");
        // Set your groupName to -1 and try to find a player that isn't in any group
        players[Context.ConnectionId] = "-1";
        string aloneOpponentId = players.FirstOrDefault(player => (player.Value == "-1" && player.Key != Context.ConnectionId)).Key;

        if (aloneOpponentId != null) // If player found
        {
            // Set groupId to both of players information in players list
            string groupId = Context.ConnectionId + aloneOpponentId;
            players[Context.ConnectionId] = groupId;
            players[aloneOpponentId] = groupId;

       

            // Add both of players to the group
            Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            Groups.AddToGroupAsync(aloneOpponentId, groupId);

            // Send Success message to these players
            Clients.Client(Context.ConnectionId).SendAsync("JoinToOpponent", 1);
            Clients.Client(aloneOpponentId).SendAsync("JoinToOpponent", 2);
        }
        else // If player not found
        {
            // Send Fail message to the caller player
            Clients.Caller.SendAsync("JoinToOpponent", -1);
        }
    }

    /// <summary>
    /// Get the position information of players and send it to opponent
    /// </summary>
    /// <param name="x">The received x position</param>
    /// <param name="y">The received y position</param>
    public void SendTransformation(float x, float y)
    {
        string groupId = players[Context.ConnectionId];
        Clients.OthersInGroup(groupId).SendAsync("OpponentTransformation", x, y);
    }
}
