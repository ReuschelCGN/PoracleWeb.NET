using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pgan.PoracleWebNet.Core.Abstractions.Repositories;
using Pgan.PoracleWebNet.Core.Models;
using Pgan.PoracleWebNet.Data;
using Pgan.PoracleWebNet.Data.Entities;

namespace Pgan.PoracleWebNet.Core.Repositories;

public class SiteSettingRepository(PoracleWebContext context, IMapper mapper) : ISiteSettingRepository
{
    private readonly PoracleWebContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<SiteSetting>> GetAllAsync()
    {
        var entities = await this._context.SiteSettings
            .AsNoTracking()
            .ToListAsync();

        return this._mapper.Map<IEnumerable<SiteSetting>>(entities);
    }

    public async Task<IEnumerable<SiteSetting>> GetByCategoryAsync(string category)
    {
        var entities = await this._context.SiteSettings
            .AsNoTracking()
            .Where(s => s.Category == category)
            .ToListAsync();

        return this._mapper.Map<IEnumerable<SiteSetting>>(entities);
    }

    public async Task<SiteSetting?> GetByKeyAsync(string key)
    {
        var entity = await this._context.SiteSettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == key);

        return entity is null ? null : this._mapper.Map<SiteSetting>(entity);
    }

    public async Task<SiteSetting> CreateOrUpdateAsync(SiteSetting setting)
    {
        var entity = await this._context.SiteSettings
            .FirstOrDefaultAsync(s => s.Key == setting.Key);

        if (entity is null)
        {
            entity = this._mapper.Map<SiteSettingEntity>(setting);
            this._context.SiteSettings.Add(entity);
        }
        else
        {
            this._mapper.Map(setting, entity);
        }

        await this._context.SaveChangesAsync();
        return this._mapper.Map<SiteSetting>(entity);
    }

    public async Task<bool> DeleteAsync(string key)
    {
        var entity = await this._context.SiteSettings
            .FirstOrDefaultAsync(s => s.Key == key);

        if (entity is null)
        {
            return false;
        }

        this._context.SiteSettings.Remove(entity);
        await this._context.SaveChangesAsync();
        return true;
    }
}
