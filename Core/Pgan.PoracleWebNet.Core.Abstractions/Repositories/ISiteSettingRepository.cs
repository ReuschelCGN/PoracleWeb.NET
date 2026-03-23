using Pgan.PoracleWebNet.Core.Models;

namespace Pgan.PoracleWebNet.Core.Abstractions.Repositories;

public interface ISiteSettingRepository
{
    public Task<IEnumerable<SiteSetting>> GetAllAsync();
    public Task<IEnumerable<SiteSetting>> GetByCategoryAsync(string category);
    public Task<SiteSetting?> GetByKeyAsync(string key);
    public Task<SiteSetting> CreateOrUpdateAsync(SiteSetting setting);
    public Task<bool> DeleteAsync(string key);
}
