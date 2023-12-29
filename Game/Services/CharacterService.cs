using System.Collections.Generic;
using System.Threading.Tasks;
using Navislamia.Game.DataAccess.Repositories.Interfaces;
using Navislamia.Game.Models.Telecaster;

namespace Navislamia.Game.Services;

public class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IStarterItemsRepository _starterItemsRepository;

    public CharacterService(IStarterItemsRepository starterItemsRepository, ICharacterRepository characterRepository)
    {
        _starterItemsRepository = starterItemsRepository;
        _characterRepository = characterRepository;
    }

    public async Task<IEnumerable<CharacterEntity>> GetCharactersByAccountNameAsync(string accountName, bool withItems = false)
    {
        return await _characterRepository.GetCharactersByAccountNameAsync(accountName, withItems);
    }

    public async Task CreateCharacterAsync(CharacterEntity character, bool withStarterItems = false)
    {
        if (withStarterItems)
        {
            character.Items ??= new List<ItemEntity>();
            
            var starterItems = await _starterItemsRepository.GetStarterItemsByJobAsync(character.CurrentJob);
            foreach (var starterItem in starterItems)
            {
                character.Items.Add(new ItemEntity
                {
                    ItemResourceId = starterItem.ResourceId,
                    Level = starterItem.Level,
                    Enhance = starterItem.Enhancement,
                    Amount = starterItem.Amount,
                    RemainingTime = starterItem.ValidForSeconds
                });
            }
        }
        
        await _characterRepository.CreateCharacterAsync(character);
        await _characterRepository.SaveChangesAsync();
    }

    public bool CharacterExists(string characterName)
    {
        return _characterRepository.CharacterExists(characterName);
    }
}