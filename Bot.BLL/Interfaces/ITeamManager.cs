using Bot.BLL.GameLogic;
using Bot.BLL.Models;

namespace Bot.BLL.Interfaces;

public interface ITeamManager
{
    string CreateNewTeam(string teamName, string password, TeamStatus teamStatus = TeamStatus.Players);
    string GetTeams();
    Task AddToOutputMessages(OutputMessageToPlayers messageToPlayers);
}