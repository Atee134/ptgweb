using Ptg.Common.Dtos;
using Ptg.Common.Exceptions;
using Ptg.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ptg.DataAccess
{
    public class RepositoryInMemory : IRepository
    {
        private readonly Dictionary<Guid, Heightmap> heightmaps;
        private readonly Dictionary<Guid, Splatmap> splatmaps;
        private readonly List<Player> players;
        private readonly List<Guid> liveSessions;

        public RepositoryInMemory()
        {
            heightmaps = new Dictionary<Guid, Heightmap>();
            splatmaps = new Dictionary<Guid, Splatmap>();
            players = new List<Player>();
            liveSessions = new List<Guid>();
        }

        // TODO use automapper

        public void AddHeightmap(HeightmapDto heightmapDto)
        {
            var heightmap = new Heightmap
            {
                Id = heightmapDto.Id,
                Width = heightmapDto.Width,
                Height = heightmapDto.Height,
                HeightmapFloatArray = heightmapDto.HeightmapFloatArray,
                HeightmapByteArray = heightmapDto.HeightmapByteArray
            };

            heightmaps.Add(heightmapDto.Id, heightmap);
        }

        public void AddSplatmap(SplatmapDto splatmapDto)
        {
            var splatmap = new Splatmap
            {
                Id = splatmapDto.Id,
                Width = splatmapDto.Width,
                Height = splatmapDto.Height,
                SplatmapByteArray = splatmapDto.SplatmapByteArray
            };

            splatmaps.Add(splatmapDto.Id, splatmap);
        }

        public void AddPlayer(PlayerDto playerDto)
        {
            var playersInSession = players.Where(p => p.SessionId == playerDto.SessionId).ToList();
            int playerId = playersInSession.Count == 0 ? 1 : playersInSession.Max(p => p.Id);

            var player = new Player
            {
                Id = playerId,
                Name = playerDto.Name,
                SessionId = playerDto.SessionId
            };

            players.Add(player);
        }

        public void AddSignalrConnectionIdToPlayer(Guid sessionId, string playerName, string connectionId)
        {
            var player = players.FirstOrDefault(p => p.SessionId == sessionId && p.Name == playerName);

            if (player != null)
            {
                player.SignalRConnectionId = connectionId;
            }
            else
            {
                throw new PtgNotFoundException("Player is not found");
            }
        }

        public void RemovePlayer(Guid sessionId, string playerName)
        {
            var player = players.FirstOrDefault(p => p.SessionId == sessionId && p.Name == playerName);

            if (player != null)
            {
                players.Remove(player);
            }
            else
            {
                throw new PtgNotFoundException("Player is not found");
            }
        }

        public PlayerDto GetPlayer(Guid sessionId, string playerName)
        {
            var player = players.FirstOrDefault(p => p.SessionId == sessionId && p.Name == playerName);

            if (player == null) throw new PtgNotFoundException("Player not found");

            return new PlayerDto
            {
                Name = player.Name,
                SessionId = player.SessionId,
                SignalRConnectionId = player.SignalRConnectionId
            };
        }

        public PlayerDto GetPlayer(string signalrConnectionId)
        {
            var player = players.FirstOrDefault(p => p.SignalRConnectionId == signalrConnectionId);

            if (player == null) throw new PtgNotFoundException("Player not found");

            return new PlayerDto
            {
                Name = player.Name,
                SessionId = player.SessionId,
                SignalRConnectionId = player.SignalRConnectionId
            };
        }

        public HeightmapDto GetHeightmap(Guid id)
        {
            var heightmap = heightmaps[id];

            return new HeightmapDto
            {
                Id = heightmap.Id,
                Width = heightmap.Width,
                Height = heightmap.Height,
                HeightmapFloatArray = heightmap.HeightmapFloatArray,
                HeightmapByteArray = heightmap.HeightmapByteArray
            };
        }

        public SplatmapDto Getsplatmap(Guid id)
        {
            var splatmap = splatmaps[id];

            return new SplatmapDto
            {
                Id = splatmap.Id,
                Width = splatmap.Width,
                Height = splatmap.Height,
                SplatmapByteArray = splatmap.SplatmapByteArray
            };
        }

        public List<PlayerDto> GetPlayers(Guid sessionId)
        {
            var playersInSession = players.Where(p => p.SessionId == sessionId);

            List<PlayerDto> playerDtos = new List<PlayerDto>();
            foreach (var player in playersInSession)
            {
                playerDtos.Add(new PlayerDto
                {
                    Name = player.Name,
                    SessionId = player.SessionId
                });
            }

            return playerDtos;
        }

        public bool HeightmapExists(Guid id)
        {
            return heightmaps.ContainsKey(id);
        }

        public bool SplatmapExists(Guid id)
        {
            return splatmaps.ContainsKey(id);
        }

        public void AddSession(Guid sessionId)
        {
            liveSessions.Add(sessionId);
        }

        public bool SessionExists(Guid sessionId)
        {
            return liveSessions.Contains(sessionId);
        }

        public int PlayerCountInSession(Guid sessionId)
        {
            return players.Count(p => p.SessionId == sessionId);
        }

        public void RemoveSession(Guid sessionId)
        {
            if (liveSessions.Contains(sessionId))
            {
                liveSessions.Remove(sessionId);
            }
        }

        public void SaveChanges()
        {
            return;
        }


    }
}
