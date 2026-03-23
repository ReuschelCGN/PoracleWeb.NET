using Pgan.PoracleWebNet.Core.Models;

namespace Pgan.PoracleWebNet.Core.Abstractions.Services;

public interface ISiteSettingService
{
    public Task<IEnumerable<SiteSetting>> GetAllAsync();
    public Task<IEnumerable<SiteSetting>> GetByCategoryAsync(string category);
    public Task<IEnumerable<SiteSetting>> GetPublicAsync();
    public Task<SiteSetting?> GetByKeyAsync(string key);
    public Task<string?> GetValueAsync(string key);
    public Task<bool> GetBoolAsync(string key);
    public Task<SiteSetting> CreateOrUpdateAsync(SiteSetting setting);
    public Task<bool> DeleteAsync(string key);
}
