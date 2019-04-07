using GameRoom.Data;
using GameRoom.Data.Models;
using GameRoom.Services.Contracts;
using GameRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameRoom.Services
{
    public class PlayersService : IPlayersService
    {
        private GameRoomDbContext context;

        public PlayersService(GameRoomDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Checks if string contains only letters. Used for validation
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private bool DoesContainOnlyLetters(string ss)
        {
            string s = ss.ToLower();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] >= 'a' && s[i] <= 'z') continue;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if string contains anything else but spaces. Used for validation
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool DoesContainAnythingButSpace(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ') return false;
            }

            return true;
        }
        /// <summary>
        /// Checks if string contains only letters and numbers. Used for validation
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private bool DoesContainOnlyLettersAndNumbers(string ss)
        {

            string s = ss.ToLower();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] >= 'a' && s[i] <= 'z') continue;
                if (s[i] >= '0' && s[i] <= '9') continue;
                return false;
            }
            return true;

        }
        /// <summary>
        /// Method used to validate input data before using it
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="additionalInformation"></param>
        /// <returns></returns>
        private bool Validation(string userName, string password, string firstName, string lastName, int age, string additionalInformation)
        {
            if ((userName.Length < 4 || userName.Length > 30)
                || !DoesContainOnlyLettersAndNumbers(userName))
                throw new InvalidOperationException("Username must contain only letters and numbers (at least 4 symbols).");

            if (password.Length < 4 || password.Length > 40)
                throw new InvalidOperationException("Password must have at least 4 symbols.");

            if ((firstName.Length < 3 || firstName.Length > 30)
                || !DoesContainOnlyLetters(firstName))
                throw new InvalidOperationException("Personal names must contain only letters (at least 3 symbols each).");

            if ((lastName.Length < 3 || lastName.Length > 30)
                || !DoesContainOnlyLetters(lastName))
                throw new InvalidOperationException("Personal names must contain only letters (at least 3 symbols each).");

            if (age < 6 || age > 120)
                throw new InvalidOperationException("You must be at least 6 years old to register in this site.");

            if (additionalInformation != null)
            {
                if (additionalInformation.Length > 240)
                {
                    throw new InvalidOperationException("Description cant contain more than 240 symbols.");
                }
            }
            return true;
        }

        /// <summary>
        /// Method used to insert records into table Player
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="additionalInformation"></param>
        /// <returns>Created player id (if created)</returns>
        public int CreatePlayer(string userName, string password, string firstName, string lastName, int age, string additionalInformation)
        {

            Validation(userName, password, firstName, lastName, age, additionalInformation);

            foreach (var dbPlayer in context.Players)
            {
                if (dbPlayer.UserName == userName)
                    throw new InvalidOperationException("Username already exists.");
            }

            var player = new Player()
            {
                UserName = userName,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                AdditionalInformation = additionalInformation
            };
            context.Players.Add(player);
            context.SaveChanges();

            return player.Id;
        }

        /// <summary>
        /// Method used to change records of type Player
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="additionalInformation"></param>
        /// <param name="oldPassword"></param>
        /// <returns>Changed player id (if changed)</returns>
        public int ChangePlayer(string userName, string password, string firstName, string lastName, int age, string additionalInformation, string oldPassword)
        {
            if (CurrentSignedPlayer.PlayerId == 0) return 0;

            if (oldPassword != context.Players.Find(CurrentSignedPlayer.PlayerId).Password)
            {
                throw new InvalidOperationException("Entered password doesnt match your original one.");
            }

            if (password == null) password = context.Players
                    .Find(CurrentSignedPlayer.PlayerId).Password;

            Validation(userName, password, firstName, lastName, age, additionalInformation);

            context.Players.Find(CurrentSignedPlayer.PlayerId).FirstName = firstName;
            context.Players.Find(CurrentSignedPlayer.PlayerId).LastName = lastName;
            if (password != context.Players.Find(CurrentSignedPlayer.PlayerId).Password)
            {
                context.Players.Find(CurrentSignedPlayer.PlayerId).Password = password;
            }
            context.Players.Find(CurrentSignedPlayer.PlayerId).Age = age;
            context.Players.Find(CurrentSignedPlayer.PlayerId).AdditionalInformation = additionalInformation;
            context.SaveChanges();
            return CurrentSignedPlayer.PlayerId;
        }

        /// <summary>
        /// Method which finds and returns last successfully logged in player id
        /// </summary>
        /// <returns>Logged in player id (if any)</returns>
        public int GetSignedPlayerId()
        {
            return CurrentSignedPlayer.PlayerId;
        }

        /// <summary>
        /// Method which marks records from table Player as 'deleted'. Called by the user itself.
        /// </summary>
        /// <returns>Deleted player id (if deleted)</returns>
        public int OptOut()
        {

            if (CurrentSignedPlayer.PlayerId == 0) return 0;

            context.Players.Find(CurrentSignedPlayer.PlayerId).PlayerOptedOutOn = DateTime.UtcNow;
            context.SaveChanges();

            return CurrentSignedPlayer.PlayerId;
        }

        /// <summary>
        /// Method which marks records from table Player as 'deleted'. Used as debugging/test purpose
        /// </summary>
        public int BanPlayer(int id, string reason)
        {
            if (context.Players.Find(id) == null)
                throw new ArgumentOutOfRangeException("Entered ID for player was out of its allowed values.");
            context.Players.Find(id).BeingBannedOn = DateTime.UtcNow;
            context.Players.Find(id).ReasonForBeingBanned = reason;
            context.SaveChanges();
            return id;
        }

        /// <summary>
        /// Method which checks if input username and password pair exists in table Player
        /// </summary>
        /// <param name="userNameOut"></param>
        /// <param name="passwordOut"></param>
        /// <returns>Found player id (if found)</returns>
        public int LogIn(string userNameOut, string passwordOut)
        {
            foreach (var player in context.Players)
            {
                if (player.UserName == userNameOut && player.Password == passwordOut)
                {
                    CurrentSignedPlayer.PlayerId = player.Id;
                    return player.Id;
                }
            }
            return 0;
        }




        /// <summary>
        /// Returns ViewModel used to create change player form
        /// </summary>
        /// <returns></returns>
        public ChangePlayerViewModel GetChangeViewModel()
        {
            if (CurrentSignedPlayer.PlayerId == 0 || context.Players.Find(CurrentSignedPlayer.PlayerId) == null) return null;

            var model = new ChangePlayerViewModel()
            {
                Id = CurrentSignedPlayer.PlayerId,
                FirstName = context.Players.Find(CurrentSignedPlayer.PlayerId).FirstName,
                LastName = context.Players.Find(CurrentSignedPlayer.PlayerId).LastName,
                Age = context.Players.Find(CurrentSignedPlayer.PlayerId).Age,
                AdditionalInformation = context.Players.Find(CurrentSignedPlayer.PlayerId).AdditionalInformation,
                UserName = context.Players.Find(CurrentSignedPlayer.PlayerId).UserName
            };

            return model;
        }

        /// <summary>
        /// Returns ViewModel used to display all players
        /// </summary>
        /// <returns></returns>
        public IndexPlayersViewModel GetAllPlayers()
        {
            var players = this.context.Players.Where(p => p.PlayerOptedOutOn == null && p.BeingBannedOn == null).Select(p => new PlayerSingleViewModel()
            {
                Id = p.Id,
                Score = context.Scoreboards.Where(x => x.PlayerId == p.Id).Sum(x => x.Score),
                AdditionalInformation = p.AdditionalInformation,
                UserName = p.UserName,
                FirstName = p.FirstName,
                LastName = p.LastName,
                IsBanned = !(p.BeingBannedOn == null),
                IsOptedOut = !(p.PlayerOptedOutOn == null)
            });

            var model = new IndexPlayersViewModel() { Players = players.ToList() };
            return model;
        }

        /// <summary>
        /// Returns ViewModel used to display detail information about user by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlayerSingleExpandedViewModel GetDetailedPlayer(int id)
        {
            Player playerContext = context.Players.Find(id);

            if (playerContext == null) throw new IndexOutOfRangeException("Player[index] - indexOutOfRange");

            ICollection<GameSingleViewModel> games = new List<GameSingleViewModel>();

            List<Scoreboard> certainPlayerBestGames = context.Scoreboards
                .Where(x => x.PlayerId == id&&x.Score>0).OrderByDescending(x => x.Score).ToList();

            foreach (var sb in certainPlayerBestGames)
            {
                if (context.Games.Find(sb.GameId).RemovedOn == null)
                {
                    games.Add(new GameSingleViewModel()
                    {
                        Name = context.Games.Find(sb.GameId).Name,
                        Score = sb.Score,
                        Id = sb.GameId
                    });
                }
            }



            var player = new PlayerSingleViewModel()
            {
                Id = playerContext.Id,
                FirstName = playerContext.FirstName,
                LastName = playerContext.LastName,
                UserName = playerContext.UserName,
                AdditionalInformation = playerContext.AdditionalInformation,

            };

            int score = 0;
            foreach (var sb in context.Scoreboards.Where(x => x.PlayerId == id))
            {
                if (context.Games.Find(sb.GameId).RemovedOn == null)
                {
                    score += sb.Score;
                }
            }

            player.Score = score;
            var detailPlayer = new PlayerSingleExpandedViewModel()
            {
                Player = player,
                Games = games,
                Age = playerContext.Age
            };

            if (playerContext.BeingBannedOn == null)
            {
                detailPlayer.IsBanned = false;
            }
            else
            {
                detailPlayer.IsBanned = true;
                detailPlayer.ReasonForBeingBanned = playerContext.ReasonForBeingBanned;
            }

            if (playerContext.PlayerOptedOutOn == null)
            {
                detailPlayer.IsOptedOut = false;
            }
            else
            {
                detailPlayer.IsOptedOut = true;
            }

            detailPlayer.Player.LoggedInId = CurrentSignedPlayer.PlayerId;

            return detailPlayer;
        }

        /// <summary>
        /// Returns ViewModel used to display information about logged-in user (if any)
        /// </summary>
        /// <returns></returns>
        public PlayerSingleViewModel GetSignedPlayer()
        {
            if (CurrentSignedPlayer.PlayerId == 0) throw new InvalidOperationException("No signed in player");

            var player = this.context.Players.Find(CurrentSignedPlayer.PlayerId);
            var PlayerVM = new PlayerSingleViewModel()
            {
                Id = player.Id,
                UserName = player.UserName,
                FirstName = player.FirstName,
                LastName = player.LastName,
                IsBanned = !(player.BeingBannedOn == null),
                IsOptedOut = !(player.PlayerOptedOutOn == null)
            };


            return PlayerVM;
        }

        /// <summary>
        /// Returns ViewModel used to display detail information about logged-in user (if any)
        /// </summary>
        /// <returns></returns>
        public PlayerSingleExpandedViewModel GetExpandedSignedPlayer()
        {
            if (CurrentSignedPlayer.PlayerId == 0) throw new InvalidOperationException("No signed in player");
            return GetDetailedPlayer(CurrentSignedPlayer.PlayerId);
        }

        /// <summary>
        /// Returns ViewModel used to display information about banned user by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BanPlayerViewModel GetBanPlayerViewModel(int id)
        {
            BanPlayerViewModel model = new BanPlayerViewModel()
            {
                PlayerId = id
            };
            return model;
        }





    }

}
