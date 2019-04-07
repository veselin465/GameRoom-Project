using GameRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Services.Contracts
{
    public interface IScoreboardService
    {
        IndexScoreboardViewModel DisplayScoreboard();
        bool IsLoggedIn();
        int SignUpPlayerForAGame(int id);
        GameTournamentViewModel GetGameTournament();
        int GameTournament(int id);
        int PlayerImprovement();
    }
}
