using System.Collections.Generic;
using System.Threading.Tasks;
using Navislamia.Game.DataAccess.Entities.Telecaster;

namespace Navislamia.Game.DataAccess.Repositories.Interfaces;

public interface ICharacterRepository
{
    CharacterEntity GetCharacterByName(string characterName);

    Task<IEnumerable<CharacterEntity>> GetCharactersByAccountNameAsync(string accountName, bool withItems = false);

    Task<CharacterEntity> CreateCharacterAsync(CharacterEntity character);

    bool CharacterExists(string characterName);

    int CharacterCount(int accountId);

    /// <summary>
    /// Avoid using SaveChanges directly from context as it applies modifications directly to the database.
    /// Finish all required operations for a step then call this method
    /// </summary>
    Task SaveChangesAsync();
}