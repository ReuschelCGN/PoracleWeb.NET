using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Pgan.PoracleWebNet.Data;

/// <summary>
/// Workaround for MySql.EntityFrameworkCore bug with MariaDB:
/// GET_LOCK('__EFMigrationsLock', -1) returns NULL on MariaDB, causing
/// System.InvalidCastException in AcquireDatabaseLockAsync.
/// This override uses a large positive timeout (3600s) instead of -1.
/// </summary>
public class MariaDbHistoryRepository(HistoryRepositoryDependencies dependencies)
    : HistoryRepository(dependencies)
{
    public override LockReleaseBehavior LockReleaseBehavior => LockReleaseBehavior.Explicit;

    protected override bool InterpretExistsResult(object? value) => value is not null and not DBNull;

    protected override string ExistsSql =>
        "SELECT 1 FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = '__EFMigrationsHistory' LIMIT 1;";

    public override string GetCreateIfNotExistsScript() =>
        $"""
         CREATE TABLE IF NOT EXISTS `{TableName}` (
             `MigrationId` varchar(150) NOT NULL,
             `ProductVersion` varchar(32) NOT NULL,
             PRIMARY KEY (`MigrationId`)
         ) CHARACTER SET utf8mb4;
         """;

    public override string GetBeginIfNotExistsScript(string migrationId) => $"""
        -- Migration: {migrationId}
        """;

    public override string GetBeginIfExistsScript(string migrationId) => $"""
        -- Migration exists: {migrationId}
        """;

    public override string GetEndIfScript() => string.Empty;

    public override IMigrationsDatabaseLock AcquireDatabaseLock()
    {
        Dependencies.RawSqlCommandBuilder
            .Build("SELECT GET_LOCK('__EFMigrationsLock', 3600);")
            .ExecuteNonQuery(
                new RelationalCommandParameterObject(
                    Dependencies.Connection, null, null,
                    Dependencies.CurrentContext.Context,
                    Dependencies.CommandLogger));

        return new MariaDbMigrationsDatabaseLock(this);
    }

    public override async Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(
        CancellationToken cancellationToken = default)
    {
        await Dependencies.RawSqlCommandBuilder
            .Build("SELECT GET_LOCK('__EFMigrationsLock', 3600);")
            .ExecuteNonQueryAsync(
                new RelationalCommandParameterObject(
                    Dependencies.Connection, null, null,
                    Dependencies.CurrentContext.Context,
                    Dependencies.CommandLogger),
                cancellationToken);

        return new MariaDbMigrationsDatabaseLock(this);
    }

    private sealed class MariaDbMigrationsDatabaseLock(MariaDbHistoryRepository repository)
        : IMigrationsDatabaseLock
    {
        public IHistoryRepository HistoryRepository => repository;

        public void Dispose() =>
            repository.Dependencies.RawSqlCommandBuilder
                .Build("SELECT RELEASE_LOCK('__EFMigrationsLock');")
                .ExecuteNonQuery(
                    new RelationalCommandParameterObject(
                        repository.Dependencies.Connection, null, null,
                        repository.Dependencies.CurrentContext.Context,
                        repository.Dependencies.CommandLogger));

        public async ValueTask DisposeAsync() =>
            await repository.Dependencies.RawSqlCommandBuilder
                .Build("SELECT RELEASE_LOCK('__EFMigrationsLock');")
                .ExecuteNonQueryAsync(
                    new RelationalCommandParameterObject(
                        repository.Dependencies.Connection, null, null,
                        repository.Dependencies.CurrentContext.Context,
                        repository.Dependencies.CommandLogger));
    }
}
