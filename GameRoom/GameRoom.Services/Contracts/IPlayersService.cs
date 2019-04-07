using GameRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRoom.Services.Contracts
{
    public interface IPlayersService
    {
        int CreatePlayer(string userName, string password,string firstName, string lastName, int age, string additionalInformation);
        int ChangePlayer(string userName, string password, string firstName, string lastName, int age, string additionalInformation, string oldPassword);
        int LogIn(string userName, string password);
        int OptOut();
        IndexPlayersViewModel GetAllPlayers();
        PlayerSingleExpandedViewModel GetDetailedPlayer(int id);
        PlayerSingleExpandedViewModel GetExpandedSignedPlayer();
        ChangePlayerViewModel GetChangeViewModel();

        BanPlayerViewModel GetBanPlayerViewModel(int id);
        int BanPlayer(int id, string reason);
        int GetSignedPlayerId();
        PlayerSingleViewModel GetSignedPlayer();
    }
}
