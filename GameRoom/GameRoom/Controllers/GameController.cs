using GameRoom.Services.Contracts;
using GameRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameRoom.Controllers
{
    public class GameController : Controller
    {

        private IGamesService service;
        public GameController(IGamesService service)
        {
            this.service = service;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult DisplayAllGames()
        {

            var model = service.GetAllGames();
            return this.View(model);
        }

        public IActionResult Detail(int id)
        {

            try
            {
                var model = service.GetGame(id);
                return this.View(model);
            }
            catch (InvalidOperationException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
        }
        
        public IActionResult MyView()
        {

            return this.View();
        }

        public IActionResult RemoveGame(int id)
        {
            service.RemoveGame(id);
            string redirectStr = "Detail/" + id;
            return this.RedirectToAction(redirectStr, "Game");
        }


        [HttpPost]
        public IActionResult Create(string name, int requiredAge, string description)
        {
            try
            {
                this.service.CreateGame(name, requiredAge, description);
                return this.RedirectToAction("Index", "Home");
            }catch(InvalidOperationException e)
            {
                return this.View("Error", new InvalidActionViewModel() { ErrorMessage = e.Message });
            }
            
        }



    }
}
