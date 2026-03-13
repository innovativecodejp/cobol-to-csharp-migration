namespace CobolToCsharpMigration.Tests;

/// <summary>
/// Collection to run MigrationTrace-dependent tests sequentially.
/// MigrationTrace is a static singleton; parallel execution causes cross-test interference.
/// </summary>
[CollectionDefinition("MigrationTrace")]
public class MigrationTraceCollection : ICollectionFixture<object>
{
}
