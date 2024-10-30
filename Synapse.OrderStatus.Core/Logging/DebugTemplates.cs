namespace Synapse.OrderStatus.Domain.Logging;

// Debug Templates
public static class DebugTemplates
{
    public static string DetailedOperationDebug =>
        "Debugging {OperationName} for ID {EntityId}. Parameters: {ParameterValues}";

    public static string ApiRequestPayload =>
        "Debug: API request to {Endpoint} with payload: {Payload}";

    public static string DatabaseQueryDebug =>
        "Debug: Executing database query for {EntityType}. Query: {QueryText}, Parameters: {QueryParameters}";

    public static string ExternalApiResponse =>
        "Debug: Response from external API {ApiName}. Status: {StatusCode}, Payload: {ResponsePayload}";

    public static string StateChangeTracking =>
        "Debug: State changed for {EntityType} with ID {EntityId}. Old State: {OldState}, New State: {NewState}";

    public static string ConfigurationValueLoaded =>
        "Debug: Configuration value loaded. Key: {ConfigKey}, Value: {ConfigValue}";

    public static string CacheOperation =>
        "Debug: Cache {CacheAction} operation performed for key {CacheKey}. Cache hit: {IsCacheHit}";

    public static string DependencyResolution =>
        "Debug: Dependency {DependencyName} resolved for service {ServiceName} in {ElapsedMilliseconds} ms";

    public static string ComparingModelWithDatabase =>
        "Comparing current model with database schema for {ModelName}";

    public static string ExecutingCommand =>
        "Executing command: {Command} with arguments {Arguments}";
}
