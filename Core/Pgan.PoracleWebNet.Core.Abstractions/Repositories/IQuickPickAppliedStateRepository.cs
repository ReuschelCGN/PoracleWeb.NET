using Pgan.PoracleWebNet.Core.Models;

namespace Pgan.PoracleWebNet.Core.Abstractions.Repositories;

public interface IQuickPickAppliedStateRepository
{
    public Task<QuickPickAppliedState?> GetAsync(string userId, int profileNo, string quickPickId);
    public Task<List<QuickPickAppliedState>> GetByUserAndProfileAsync(string userId, int profileNo);
    public Task CreateOrUpdateAsync(QuickPickAppliedState state);
    public Task DeleteAsync(string userId, int profileNo, string quickPickId);
}
