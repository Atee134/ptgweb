using Ptg.Common.Dtos;
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

        public int AddPlayer(PlayerDto playerDto)
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

            return playerId;
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
