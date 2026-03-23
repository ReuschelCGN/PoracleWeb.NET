using Pgan.PoracleWebNet.Core.Models;

namespace Pgan.PoracleWebNet.Core.Abstractions.Repositories;

public interface IQuickPickDefinitionRepository
{
    public Task<List<QuickPickDefinition>> GetAllGlobalAsync();
    public Task<List<QuickPickDefinition>> GetByOwnerAsync(string userId);
    public Task<QuickPickDefinition?> GetByIdAsync(string id);
    public Task<QuickPickDefinition?> GetByIdAndOwnerAsync(string id, string userId);
    public Task CreateOrUpdateAsync(QuickPickDefinition definition);
    public Task DeleteAsync(string id);
    public Task DeleteByIdAndOwnerAsync(string id, string userId);
    public Task DeleteAllGlobalAsync();
}
