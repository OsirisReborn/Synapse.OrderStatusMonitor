namespace Synapse.OrderStatus.Domain.Logging;
// Warning Templates
public static class WarningTemplates
{
    public static string PotentialIssueInOperation =>
        "Warning: Potential issue in {OperationName} for ID {EntityId}. Details: {WarningDetails}";

    public static string MissingOrInvalidData =>
        "Warning: {FieldName} is missing or invalid for entity {EntityType} with ID {EntityId}";

    public static string ExternalServiceDelay =>
        "Warning: Delay detected in external service {ServiceName}. Expected time: {ExpectedTime} ms, Actual time: {ElapsedMilliseconds} ms";

    public static string DeprecatedApiUsage =>
        "Warning: Deprecated API {ApiEndpoint} used by User {UserId}. Expected retirement date: {RetirementDate}";

    public static string UnusualUserActivity =>
        "Warning: Unusual activity detected for User {UserName} on {TargetEntity} with ID {EntityId}. Details: {ActivityDetails}";
}
