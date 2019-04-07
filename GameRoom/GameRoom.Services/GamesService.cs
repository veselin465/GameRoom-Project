using System;
using GameRoom.Services.Contracts;
using System.Collections.Generic;
using System.Text;
using GameRoom.Data;
using GameRoom.Data.Models;
using GameRoom.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameRoom.Services
{
    public class GamesService : IGamesService
    {

        private GameRoomDbContext context;

        public GamesService(GameRoomDbContext context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Method which inserts records in table Game
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requiredAge"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public int CreateGame(string name, int requiredAge, string description)
        {

            foreach (var dbGame in context.Games)
            {
                if (dbGame.Name.ToLower() == name.ToLower())
                    throw new InvalidOperationException("Dublicate game name");

            }

            var game = new Game()
            {
                Name = name,
                RequiredAge = requiredAge,
                Description = description
            };

            context.Games.Add(game);
            context.SaveChanges();

            return game.Id;
        }

        /// <summary>
        /// Method which marks records in table Game as removed by given id
        /// </summary>
        /// <param name="id"></param>
        public int RemoveGame(int id)
        {
            context.Games.Find(id).RemovedOn = DateTime.UtcNow;
            context.SaveChanges();
            return id;
        }

        /// <summary>
        /// Method which calculates the sum of all players' points in table Game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int SumGameScore(int id)
        {
            int score = 0;
            foreach (var sb in context.Scoreboards.Where(x => x.GameId == id))
            {
                int plId = sb.PlayerId;
                if (context.Players.Find(plId).BeingBannedOn == null && context.Players.Find(sb.PlayerId).PlayerOptedOutOn == null)
                {
                    score += sb.Score;
                }
            }
            return score;
        }

        /// <summary>
        /// Finds all games and returns ViewModel to share that data with users
        /// </summary>
        /// <returns>ViewModel used to display information to the user</returns>
        public IndexGamesViewModel GetAllGames()
        {

            ICollection<GameSingleViewModel> collection = new List<GameSingleViewModel>();

            var games = this.context.Games.Where(g => g.RemovedOn == null);

            foreach (var game in games)
            {
                GameSingleViewModel gameVM = new GameSingleViewModel()
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    RequiredAge = game.RequiredAge
                };
                int score = SumGameScore(game.Id);
                gameVM.Score = score;
                collection.Add(gameVM);
            }

            var model = new IndexGamesViewModel() { Games = collection };
            return model;
        }


        /// <summary>
        /// Finds information about game by given id and returns ViewModel to share that data
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameSingleExpandedViewModel GetGame(int gameId)
        {
            Game gameContext = context.Games.Find(gameId);

            if (gameContext == null) throw new IndexOutOfRangeException("Game[index] - indexOutOfRange");

            ICollection<PlayerSingleViewModel> players = new List<PlayerSingleViewModel>();

            List<Scoreboard> certainGameBestPlayers = context.Scoreboards.Where(x => x.GameId == gameId).OrderBy(x => x.Score).ToList();

            foreach (var playerGame in certainGameBestPlayers)
            {
                if (context.Players.Find(playerGame.PlayerId).BeingBannedOn != null ||
                    context.Players.Find(playerGame.PlayerId).PlayerOptedOutOn != null ||
                    context.Games.Find(playerGame.GameId).RemovedOn != null)
                {
                    continue;
                }
                players.Add(new PlayerSingleViewModel() {
                    UserName = context.Players.Find(playerGame.PlayerId).UserName,
                    FirstName = context.Players.Find(playerGame.PlayerId).UserName,
                    LastName = context.Players.Find(playerGame.PlayerId).UserName,
                    Score = playerGame.Score,
                    Id = playerGame.PlayerId
                });
            }

            var game = new GameSingleViewModel()
            {
                Id = gameContext.Id,
                Name = gameContext.Name,
                Description = gameContext.Description,
                RequiredAge = gameContext.RequiredAge,
                IsRemoved = context.Games.Find(gameId).RemovedOn != null
            };


            game.Score = SumGameScore(gameId);


            int signedId = CurrentSignedPlayer.PlayerId;
            var detailGame = new GameSingleExpandedViewModel()
            {
                Players = players,
                Game = game,
                SignedInPlayerScore = (signedId != 0 && context.Scoreboards.Find(signedId, gameId)!=null)? context.Scoreboards.Find(signedId, gameId).Score : 0,
                CanSignedInPlayerCompete = /*signedId != 0 ? (context.Players.Find(signedId).BeingBannedOn == null && context.Players.Find(signedId).PlayerOptedOutOn == null) :*/ true
            };

            if (context.Scoreboards.Find(CurrentSignedPlayer.PlayerId, gameId) == null)
            {
                detailGame.IsCurrentPlayerAlreadySignedIn = false;
            }
            else
            {
                detailGame.IsCurrentPlayerAlreadySignedIn = true;
            }

            detailGame.Game.LoggedInId = CurrentSignedPlayer.PlayerId;

            return detailGame;
        }
        
    }
}
