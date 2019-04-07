using GameRoom.Data.Models;
using GameRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Services.Contracts
{
    public interface IGamesService
    {
        int CreateGame(string name, int requiredAge, string description);
        IndexGamesViewModel GetAllGames();
        GameSingleExpandedViewModel GetGame(int id);
        int RemoveGame(int id);
    }
}
