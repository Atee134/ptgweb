using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ptg.Common.Dtos.Request;
using Ptg.Common.Exceptions;
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
        public async Task<IActionResult> CreateGameSession([FromBody] CreateGameSessionRequestDto requestDto)
        {
            await CheckSession();

            var sessionId = gameManagerService.CreateGameSession();

            gameManagerService.AddPlayer(sessionId, requestDto.PlayerName);

            HttpContext.Session.SetString("SessionId", sessionId.ToString());
            HttpContext.Session.SetString("PlayerName", requestDto.PlayerName);
            HttpContext.Session.SetString("SessionCreator", "true");

            return Ok(sessionId);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinGameSession([FromBody] JoinGameSessionRequestDto requestDto)
        {
            await CheckSession();

            if (!Guid.TryParse(requestDto.SessionId, out Guid sessionGuid))
            {
                throw new PtgInvalidActionException("The provided Game Lobby ID is not valid.");
            }

            gameManagerService.AddPlayer(sessionGuid, requestDto.PlayerName); // TODO add error handling middleware, if session is not found return 404 to client

            HttpContext.Session.SetString("SessionId", requestDto.SessionId.ToString());
            HttpContext.Session.SetString("PlayerName", requestDto.PlayerName);
            HttpContext.Session.SetString("SessionCreator", "false");

            return Ok(sessionGuid);
        }

        [HttpDelete("leave")]
        public async Task<IActionResult> LeaveGameSession()
        {
            await CheckSession();

            return NoContent();
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGameSession([FromBody] StartGameSesionRequestDto requestDto)
        {
            if (bool.TryParse(HttpContext.Session.GetString("SessionCreator"), out bool creator) && creator)
            {
                gameManagerService.ValidateGameSessionStart(requestDto.SessionId, requestDto.TerrainDataId);

                await gameManagerHubContext.Clients.Group(requestDto.SessionId.ToString()).SendAsync("receiveTerrainDataId", requestDto.TerrainDataId);
            }
            return NoContent();
            // TODO register event on client for this
        }

        private async Task CheckSession()
        {
            string existingSessionId = HttpContext.Session.GetString("SessionId");
            string existingPlayerName = HttpContext.Session.GetString("PlayerName");
            if (existingSessionId != null && existingPlayerName != null)
            {
                HttpContext.Session.Clear();

                var player = gameManagerService.GetPlayer(Guid.Parse(existingSessionId), existingPlayerName);
                if (player != null)
                {
                    await gameManagerHubContext.Clients.Group(player.SessionId.ToString()).SendAsync("playerLeft", player.Name);
                    if (player.SignalRConnectionId != null)
                    {
                        await gameManagerHubContext.Groups.RemoveFromGroupAsync(player.SignalRConnectionId, existingSessionId);
                    }
                    gameManagerService.RemovePlayer(Guid.Parse(existingSessionId), existingPlayerName);
                }
            }
        }

        [HttpGet("{sessionId}/players")]
        public IActionResult GetPlayersOfSession(Guid sessionId)
        {
            var players = gameManagerService.GetPlayerNamesInSession(sessionId);

            return Ok(players);
        }
    }
}