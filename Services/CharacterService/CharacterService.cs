using System.Threading.Tasks;
using System.Linq;
using dotnet_rpg.Models;
using System.Collections.Generic;
using dotnet_rpg.Dtos.Character;
using AutoMapper;
using System;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
               Character character = await _context.Characters.FirstAsync(x => x.Id == id);
                _context.Characters.Remove(character);
              await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Characters.Select(x => _mapper.Map<GetCharacterDto>(x))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            List<Character> dbCharacters = await _context.Characters.ToListAsync();
            serviceResponse.Data = (dbCharacters.Select(x => _mapper.Map<GetCharacterDto>(x))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            Character SingleCharacter = await _context.Characters.FirstOrDefaultAsync(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(SingleCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.Class = updateCharacter.Class;
                character.Defense = updateCharacter.Defense;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intelligence = updateCharacter.Intelligence;
                character.Strength = updateCharacter.Strength;

                _context.Characters.Update(character);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }


}