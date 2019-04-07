using GameRoom.Data;
using GameRoom.Data.Models;
using GameRoom.Services.Contracts;
using GameRoom.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameRoom.Services
{
    public class ScoreboardService : IScoreboardService
    {

        private const int startingDefaultScore = 100;

        private GameRoomDbContext context;

        public ScoreboardService(GameRoomDbContext context)
        {
            this.context = context;
        }

        
        /// <summary>
        /// Changes every player's score who signed for certain game by given id simulating that a tournament was held
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Game id (if found)</returns>
        public int GameTournament(int id)
        {
            if (context.Games.Find(id).RemovedOn != null) throw new InvalidOperationException("Cannot create tournament on removed game.");
            Random random = new Random();
            foreach (var sb in context.Scoreboards.Where(x=>x.GameId==id && x.Score > 0))
            {
                sb.Score += (random.Next(250) - 100);
            }
            context.SaveChanges();
            return id;
        }


        /// <summary>
        /// Changes player's score for every game by random values simulating that he played these games
        /// </summary>
        /// <returns>Player id (if found)</returns>
        public int PlayerImprovement()
        {
            if (CurrentSignedPlayer.PlayerId == 0) return 0;

            if (context.Players.Find(CurrentSignedPlayer.PlayerId).BeingBannedOn != null || context.Players.Find(CurrentSignedPlayer.PlayerId).PlayerOptedOutOn != null) throw new InvalidOperationException("Player cant compete in games.");
            Random random = new Random();
            foreach (var sb in context.Scoreboards.Where(x => x.PlayerId == CurrentSignedPlayer.PlayerId && x.Score > 0))
            {
                sb.Score += (random.Next(500) - 200);
            }
            context.SaveChanges();
            return CurrentSignedPlayer.PlayerId;
        }

        /// <summary>
        /// Method used to validate if player is old enough to play and compete certain game by given player and game IDs
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private bool ValidatePermision(int playerId, int gameId)
        {
            return context.Players.Find(playerId).Age >= context.Games.Find(gameId).RequiredAge;
        }

        /// <summary>
        /// Method which inserts records into table Scoreboard connecting 2 objects of types Player and Game 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player id (if found)</returns>
        public int SignUpPlayerForAGame(int id)
        {
            Scoreboard scoreboard = new Scoreboard()
            {
                GameId = id,
                PlayerId = CurrentSignedPlayer.PlayerId,
                Score = startingDefaultScore
            };
            if (!ValidatePermision(scoreboard.PlayerId, scoreboard.GameId))
            {
                throw new InvalidOperationException("Player is not old enough for the chosen game");
            }
            context.Scoreboards.Add(scoreboard);
            context.SaveChanges();
            return id;
        }

        /// <summary>
        /// Method which returns if there is player who had successfully logged-in during current session
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn()
        {
            return CurrentSignedPlayer.PlayerId != 0;
        }

        /// <summary>
        /// Returns ViewModel used to display information about games tournament form
        /// </summary>
        /// <returns></returns>
        public GameTournamentViewModel GetGameTournament()
        {
            GameTournamentViewModel gt = new GameTournamentViewModel()
            {
                LastGameId = context.Games.Last().Id
            };
            return gt;
        }

        /// <summary>
        /// Returns ViewModel used to display scoreboard which shows every player-game pair and the earned points
        /// </summary>
        /// <returns></returns>
        public IndexScoreboardViewModel DisplayScoreboard()
        {

            List<Scoreboard> scoreboard = context.Scoreboards.ToList();
            scoreboard.OrderBy(x => x.Score);
            ICollection<ScoreboardViewModel> colection = new List<ScoreboardViewModel>();
            foreach (var playerGame in scoreboard.Where(x=>x.Score>0))
            {
                if (context.Players.Find(playerGame.PlayerId).BeingBannedOn != null ||
                    context.Players.Find(playerGame.PlayerId).PlayerOptedOutOn != null ||
                    context.Games.Find(playerGame.GameId).RemovedOn != null)
                {
                    continue;
                }
                if (playerGame != null)
                {
                    colection.Add(new ScoreboardViewModel()
                    {

                        PlayerId = playerGame.PlayerId,
                        PlayerFirstName = context.Players.Find(playerGame.PlayerId)
                        .FirstName,
                        PlayerLastName = context.Players.Find(playerGame.PlayerId).LastName,
                        PlayerUserName = context.Players.Find(playerGame.PlayerId).UserName,

                        GameId = playerGame.GameId,
                        GameName = context.Games.Find(playerGame.GameId).Name,

                        Score = playerGame.Score

                    });
                }
               
            }

            var model = new IndexScoreboardViewModel() { Scoreboard = colection };
            return model;

        }
    }

}
