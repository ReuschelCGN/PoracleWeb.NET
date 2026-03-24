# Troubleshooting

## MySQL provider incompatibility

**Problem**: Build errors or runtime exceptions related to `Pomelo.EntityFrameworkCore.MySql`.

**Solution**: This project uses `MySql.EntityFrameworkCore` (Oracle's official provider), **not** Pomelo. Pomelo is incompatible with EF Core 10. Connection setup uses `options.UseMySQL(connectionString)` (capital SQL).

---

## NULL string constraint violations

**Problem**: MySQL errors like `Column 'X' cannot be null` when saving entities.

**Solution**: Call `EnsureNotNullDefaults()` before saving. Many Poracle DB columns are `NOT NULL` with empty-string defaults, but EF Core maps them as `string?`. The method sets null strings to `""`.

---

## Discord API calls failing

**Problem**: Discord API calls return errors or time out.

**Solution**: Use `discordapp.com` (not `discord.com`) for API calls. The `discord.com` domain is blocked by Cloudflare in some server environments. PoracleWeb is already configured to use `discordapp.com`.

Also note: Use API **v9** (not v10) — v10 is not supported on the `discordapp.com` domain.

---

## Poracle config defaultTemplateName errors

**Problem**: Deserialization errors when parsing Poracle config.

**Solution**: `defaultTemplateName` can be a number (e.g., `1`) or a string (e.g., `"default"`). Use `JsonElement` or handle both types during deserialization.

---

## Scanner DB connection errors

**Problem**: Errors about missing scanner service or database connection.

**Solution**: The `ScannerDb` connection string is optional. If not configured, `IScannerService` is not registered and scanner endpoints return appropriate responses. This is expected behavior.

---

## Bulk update zeroing out alarm fields

**Problem**: After bulk updating alarms, fields like `clean`, `template`, and filter settings are reset to 0.

**Solution**: Never send partial objects to `PUT /{uid}`. AutoMapper maps all fields — `int` properties default to `0` when absent from JSON. Use the dedicated `PUT /distance/bulk` endpoint for distance changes, or spread the full alarm object:

```typescript
// ✅ Correct
this.http.put(`/api/pokemon/${uid}`, { ...alarm, distance });

// ❌ Wrong
this.http.put(`/api/pokemon/${uid}`, { distance });
```

---

## Geofence names not matching in Poracle

**Problem**: Custom geofences don't trigger alerts even though they're in the user's area list.

**Solution**: Poracle does **case-sensitive** area matching. Geofence names must always be lowercase. The `kojiName` field in `user_geofences` and entries in `humans.area` must match exactly. `UserGeofenceService.CreateAsync()` enforces this with `ToLowerInvariant()`.

---

## Koji displayInMatches not working

**Problem**: User geofence names appear in DMs even though `displayInMatches` is set to `false`.

**Solution**: Koji's `displayInMatches` custom property is not reliably honored by all Poracle format serializers. Serve user geofences from the PoracleWeb feed endpoint (`/api/geofence-feed`) instead of pushing them to Koji. Only promote to Koji when an admin approves a geofence for public use.

---

## Rate limiting locking out all users

**Problem**: Multiple users report being unable to log in simultaneously.

**Solution**: Auth rate limiting must be **per-IP** (partitioned), not global. Check that `Program.cs` uses `RateLimitPartition.GetFixedWindowLimiter` keyed by `RemoteIpAddress`, not `AddFixedWindowLimiter`.

---

## Monster filter defaults (size, max_level, etc.)

**Problem**: New monster alarms created via the web UI may silently filter out pokemon if model defaults don't match PoracleJS expectations. For example, `max_size=0` causes all pokemon with size data to be rejected, and `size=0` instead of `size=-1` shows incorrectly in the old PHP UI as "-XXL".

**Solution**: All Create model defaults are aligned with the PHP PoracleWeb `include/defaults.php`. Key values:

- `size=-1` means "no size filter" (not `0`)
- `max_size=5` means "up to XXL"
- `max_level=55` (not 40 or 50)
- Raid/Egg `team=4` means "all teams"
- Raid `move=9000` and `evolution=9000` mean "no filter"

If users report missing alerts, check the `monsters` table for rows where max fields are `0` when they should have defaults:

```sql
-- Find alarms with broken size filter (rejects all pokemon with size data)
SELECT * FROM monsters WHERE max_size = 0;

-- Find alarms with incorrect "no size filter" value (shows as "-XXL" in PHP UI)
SELECT * FROM monsters WHERE size = 0;
```
