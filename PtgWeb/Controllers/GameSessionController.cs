using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ptg.DataAccess;
using Ptg.Services.Interfaces;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IGameManagerService gameManagerService;

        public GameSessionController(IRepository repository, IGameManagerService gameManagerService)
        {
            this.repository = repository;
            this.gameManagerService = gameManagerService;
        }

        [HttpPost("create")]
        public IActionResult CreateGameSession([FromBody] string playerName)
        {
            // TODO if sessionId and playerId both exist, remove player from players, if all players are removed from a session, remove session from sessions

            var sessionId = gameManagerService.CreateGameSession();

            int playerId = gameManagerService.AddPlayer(sessionId, playerName);

            HttpContext.Session.SetString("SessionId", sessionId.ToString());
            HttpContext.Session.SetInt32("PlayerId", playerId);

            return Ok(sessionId);
        }
    }
}