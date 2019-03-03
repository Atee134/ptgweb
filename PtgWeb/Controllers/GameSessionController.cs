using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ptg.Common.Dtos.Request;
using Ptg.DataAccess;
using Ptg.Services.Interfaces;
using PtgWeb.Hubs;

namespace PtgWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSessionController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly IGameManagerService gameManagerService;
        private readonly IHubContext<GameManagerHub> gameManagerHubContext;

        public GameSessionController(IRepository repository, IGameManagerService gameManagerService, IHubContext<GameManagerHub> gameManagerHubContext)
        {
            this.repository = repository;
            this.gameManagerService = gameManagerService;
            this.gameManagerHubContext = gameManagerHubContext;
        }

        [HttpPost("create")]
        public IActionResult CreateGameSession([FromBody] CreateGameSessionRequestDto requestDto)
        {
            // TODO if sessionId and playerId both exist, remove player from players, if all players are removed from a session, remove session from sessions

            var sessionId = gameManagerService.CreateGameSession();

            int playerId = gameManagerService.AddPlayer(sessionId, requestDto.PlayerName);

            HttpContext.Session.SetString("SessionId", sessionId.ToString());
            HttpContext.Session.SetInt32("PlayerId", playerId);
            HttpContext.Session.SetString("SessionCreator", "true");

            return Ok(sessionId);
        }

        [HttpPost("join")]
        public IActionResult JoinGameSession([FromBody] JoinGameSessionRequestDto requestDto)
        {
            if (HttpContext.Session.GetString("SessionId") != null)
            {
                return NoContent();
            }

            int playerId = gameManagerService.AddPlayer(requestDto.SessionId, requestDto.PlayerName); // TODO add error handling middleware, if session is not found return 404 to client

            HttpContext.Session.SetString("SessionId", requestDto.SessionId.ToString());
            HttpContext.Session.SetInt32("PlayerId", playerId);
            HttpContext.Session.SetString("SessionCreator", "false");

            return Ok(requestDto.SessionId);
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGameSession([FromBody] StartGameSesionRequestDto requestDto)
        {
            if (bool.TryParse(HttpContext.Session.GetString("SessionCreator"), out bool creator) && creator)
            {
                gameManagerService.ValidateGameSessionStart(requestDto.SessionId, requestDto.TerrainDataId);

                await gameManagerHubContext.Clients.Group(requestDto.SessionId.ToString()).SendAsync("receiveTerrainDataGuid", requestDto.TerrainDataId);
            }
            return NoContent();
            // TODO register event on client for this
        }

        [HttpGet("{sessionId}/players")]
        public IActionResult GetPlayersOfSession(Guid sessionId)
        {
            var players = repository.GetPlayers(sessionId);

            return Ok(players);
        }
    }
}