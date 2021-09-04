using System;
using System.Collections.Generic;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ComandReadDto>> GetCommandsByPlatform(int platformId)
        {
            Console.WriteLine("--> GetCommandsByPlatform with id " + platformId);

            if (!_repository.ExistsPlatform(platformId))
                return NotFound();

            var commands = _repository.GetCommandsByPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<ComandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandById")]
        public ActionResult<ComandReadDto> GetCommandById(int platformId, int commandId)
        {
            Console.WriteLine("--> GetCommandById with id " + commandId + " and platform id " + platformId);

            if (!_repository.ExistsPlatform(platformId))
                return NotFound();

            var command = _repository.GetCommand(platformId, commandId);
            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<ComandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<ComandReadDto> CreateCommand(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine("--> CreateCommand with platform id " + platformId);

            if (!_repository.ExistsPlatform(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<ComandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandById),
                new { Id = commandReadDto.Id }, commandReadDto
            );
        }
    }
}