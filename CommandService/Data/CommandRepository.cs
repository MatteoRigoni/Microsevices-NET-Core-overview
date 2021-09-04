using System;
using System.Collections.Generic;
using System.Linq;
using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _dbContenxt;
        public CommandRepository(AppDbContext dbContenxt)
        {
            _dbContenxt = dbContenxt;

        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
                throw new ArgumentNullException("Command is null!");

            command.PlatformId = platformId;    
            _dbContenxt.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
                throw new ArgumentNullException("Platform is null!");

            _dbContenxt.Platforms.Add(plat);
        }

        public bool ExistsExternalPlatform(int extrnalPlatformId)
        {
            return _dbContenxt.Platforms.Any(p => p.ExternalId == extrnalPlatformId);
        }

        public bool ExistsPlatform(int platformId)
        {
            return _dbContenxt.Platforms.Any(p => p.Id == platformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContenxt.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _dbContenxt.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsByPlatform(int platformId)
        {
            return _dbContenxt.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool SaveChanges()
        {
            return _dbContenxt.SaveChanges() > 0;
        }
    }
}