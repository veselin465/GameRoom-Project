using GameRoom.Services;
using Microsoft.AspNetCore.Mvc;
using GameRoom.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameRoom.ViewModels;

namespace GameRoom.Controllers
{
    public class PlayerController : Controller
    {

        private IPlayersService service;
        public PlayerController(IPlayersService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Create an object of type Player and adds to DB
        /// </summary>
        public IActionResult Create()
        {
            return this.View();
        }

        /// <summary>
        /// Tries to find username and password combination in DB
        /// </summary>
        public IActionResult LogIn()
        {
            return this.View();
        }

        /// <summary>
        /// Displays information if LogIn successfully found the user
        /// </summary>
        /// <returns></returns>
        public IActionResult SuccessfulLogIn()
        {
            try
            {
                var model = service.GetSignedPlayer();
                return this.View(model);
            }
            catch (InvalidOperationException e)
            {
                return this.RedirectToAction("RequiredLogIn", "Player");
            }


        }

        /// <summary>
        /// Displays information if LogIn didnt find the user
        /// </summary>
        /// <returns></returns>
        public IActionResult FailedLogInAttempt()
        {
            return this.View();
        }

        /// <summary>
        /// Displays information if user's last action required successful LogIn
        /// </summary>
        /// <returns></returns>
        public IActionResult RequiredLogIn()
        {
            return this.View();
        }

        /// <summary>
        /// By given id, displays information about user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {

            try
            {
                var model = service.GetDetailedPlayer(id);
                return this.View(model);
            }
            catch (InvalidOperationException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
        }

        /// <summary>
        /// Displays all users whose profile status is acceptable 
        /// </summary>
        /// <returns></returns>
        public IActionResult DisplayAllPlayers()
        {
            var model = service.GetAllPlayers();
            return this.View(model);
        }

        /// <summary>
        /// Displays information about last successfully logged in user
        /// </summary>
        /// <returns></returns>
        public IActionResult Profile()
        {
            int id = service.GetSignedPlayerId();
            if (id == 0)
            {
                return this.RedirectToAction("RequiredLogIn", "Player");
            }
            string redirectStr = "Detail/" + id;
            return this.RedirectToAction(redirectStr, "Player");
        }

        /// <summary>
        /// Displays form used by last successfully logged in user to change his data
        /// </summary>
        /// <returns></returns>
        public IActionResult Change()
        {
            var model = service.GetChangeViewModel();
            if (model == null)
            {
                return this.RedirectToAction("LogIn", "Player");
            }
            return this.View(model);
        }

        /// <summary>
        /// Sends request to change last user's successfully logged in data
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="additionalInformation"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Change(string userName, string password, string firstName, string lastName, int age, string additionalInformation, string oldPassword)
        {
            try
            {
                int id = this.service.ChangePlayer(userName, password, firstName, lastName, age, additionalInformation, oldPassword);
                if (id == 0)
                {
                    return this.RedirectToAction("LogIn", "Player");
                }

            }
            catch (InvalidOperationException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }

            try
            {
                int id = this.service.ChangePlayer(userName, password, firstName, lastName, age, additionalInformation, oldPassword);
                if (id == 0)
                {
                    return this.RedirectToAction("LogIn", "Player");
                }

            }
            catch (InvalidOperationException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }

            return this.RedirectToAction("Profile", "Player");
        }

        /// <summary>
        /// Displays form used to ban players by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult BanPlayer(int id)
        {

            var model = service.GetBanPlayerViewModel(id);
            return this.View(model);
        }

        /// <summary>
        /// Sends request to ban player according to given id and reason
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BanPlayer(int id, string reason)
        {
            try
            {
                service.BanPlayer(id, reason);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
            string redirectStr = "Detail/" + id;
            return this.RedirectToAction(redirectStr, "Player");
        }

        /// <summary>
        /// Sends request to add to DB new object of type Player
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="additionalInformation"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(string userName, string password, string firstName, string lastName, int age, string additionalInformation)
        {
            try
            {
                this.service.CreatePlayer(userName, password, firstName, lastName, age, additionalInformation);
            }
            catch (Exception e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
            return this.RedirectToAction("LogIn", "Player");
        }

        /// <summary>
        /// Method used to detect when last successfully logged player 
        /// decides to opt out (delete his account)
        /// </summary>
        /// <returns></returns>
        public IActionResult OptOut()
        {
            this.service.OptOut();
            return this.RedirectToAction("Profile", "Player");
        }

        [HttpPost]
        public IActionResult LogIn(string userName, string password)
        {

            int userId = this.service.LogIn(userName, password);
            if (userId == 0)
            {
                return this.RedirectToAction("FailedLogInAttempt", "Player");

            }
            string redirect = "SuccessfulLogIn/" + userId;
            return this.RedirectToAction("SuccessfulLogIn", "Player");
        }

        [HttpPost]
        public IActionResult FailedLogInAttempt(string userName, string password)
        {
            return this.LogIn(userName, password);
        }

        [HttpPost]
        public IActionResult BackToHome()
        {
            return this.View();
        }

    }
}
