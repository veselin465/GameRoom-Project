using GameRoom.Services.Contracts;
using GameRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameRoom.Controllers
{
    public class ScoreboardController : Controller
    {
        private IScoreboardService service;
        public ScoreboardController(IScoreboardService service)
        {
            this.service = service;
        }


        public IActionResult GameTournament()
        {
            var model = service.GetGameTournament();
            return this.View(model);

        }

        [HttpPost]
        public IActionResult GameTournament(int GameId)
        {
            try
            {
                service.GameTournament(GameId);
                string redicrectStr = "Detail/" + GameId;
                return this.RedirectToAction(redicrectStr, "Game");
            }
            catch (Exception e)
            {
                return View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
        }


        public IActionResult PlayerImprovement()
        {
            try
            {
                int id = service.PlayerImprovement();
                if (id == 0)
                {
                    return this.RedirectToAction("RequiredLogIn", "Player");
                }
                string redirectStr = "Detail/" + id;
                return this.RedirectToAction(redirectStr, "Player");
            }
            catch (Exception e)
            {
                return View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
            
        }

        public IActionResult DisplayScoreboard()
        {
            var model = service.DisplayScoreboard();
            return this.View(model);

        }

        public IActionResult Error()
        {
            return this.View();

        }

        public IActionResult SignUpPlayerForAGame(int id)
        {
            if (service.IsLoggedIn())
            {
                try
                {
                    service.SignUpPlayerForAGame(id);
                    string redirectStr = "Detail/" + id;
                    return this.RedirectToAction(redirectStr, "Game");
                }
                catch (Exception e)
                {
                    return View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
                }
                
                
            }
            else
            {
                return this.RedirectToAction("RequiredLogIn", "Player");
            }
        }

    }
}
