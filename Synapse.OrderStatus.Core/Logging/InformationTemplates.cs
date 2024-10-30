namespace Synapse.OrderStatus.Domain.Logging;

// Information Templates
public static class InformationTemplates
{
    public static string OperationCompleted =>
        "Operation completed: {OperationName} with ID {EntityId} by User {UserId} in {ElapsedMilliseconds} ms";

    public static string ServiceMethodExecuted =>
        "Service method {MethodName} executed for ID {EntityId}. Request parameters: {Parameters}";

    public static string UserActionLogged =>
        "User action: {UserName} performed {Action} on {TargetEntity} with ID {EntityId}";

    public static string DatabaseQueryExecuted =>
        "Database query executed for {EntityType} with parameters {QueryParameters}. Execution time: {ElapsedMilliseconds} ms";

    public static string ResourceCreated =>
        "Resource created: {ResourceType} with ID {ResourceId} by User {UserId}";

    public static string ApiRequestReceived =>
        "API request received at {Endpoint} by User {UserId} with parameters {RequestParameters}";

    public static string OrderUpdated =>
        "Order {OrderId} has been successfully updated.";



    public static string StartingMigrationCheck =>
        "Starting migration check for {OperationName} in {Component} by {UserId}";

    public static string ApplyingPendingMigrations =>
        "Applying pending migrations for {Database}";

    public static string MigrationsAppliedSuccessfully =>
        "Pending migrations applied successfully for {Database}";

    public static string ModelChangesDetected =>
        "Model changes detected in {ModelName}. Generating new migration.";

    public static string MigrationGeneratedAndApplied =>
        "New migration generated and applied successfully for {ModelName}.";

    public static string AlertSent =>
        "Delivery alert sent successfully for Order {OrderId}, Item {ItemId}.";

    public static string NoMigrationsNeeded =>
        "No migrations needed. Database is up-to-date for {Database}";

    public static string GeneratingMigration =>
        "Generating new migration with name {MigrationName}";
}
