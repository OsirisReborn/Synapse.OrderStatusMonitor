namespace Synapse.OrderStatus.Domain.Logging;

// Error Templates
public static class ErrorTemplates
{
    public static string GeneralOperationError =>
        "Error in {OperationName} with ID {EntityId}. Error Message: {ErrorMessage}";

    public static string ServiceExecutionFailure =>
        "Error: Service {ServiceName} failed to execute method {MethodName} for ID {EntityId}. Exception: {ExceptionMessage}";

    public static string DatabaseError =>
        "Database error encountered for {EntityType} with ID {EntityId}. SQL State: {SqlState}. Error: {SqlErrorMessage}";

    public static string ApiRequestFailure =>
        "API error: Request to {Endpoint} failed with status code {StatusCode}. Error Message: {ErrorMessage}";

    public static string FetchOrdersFailed =>
        "Failed to fetch orders: {ErrorMessage}";

    public static string AlertSendFailed =>
        "Failed to send delivery alert for Order {OrderId}, Item {ItemId}: {ErrorMessage}";

    public static string UserAuthenticationFailure =>
        "Error: Authentication failed for User {UserId}. Reason: {FailureReason}";

    public static string ExternalServiceFailure =>
        "Error: External service {ServiceName} returned error for operation {OperationName}. Error Message: {ServiceErrorMessage}";

    public static string PermissionDenied =>
        "Error: Permission denied for User {UserId} on {EntityType} with ID {EntityId}. Required role: {RequiredRole}";

    public static string FileOperationError =>
        "Error: File operation failed. Operation: {FileOperation}, File Path: {FilePath}. Error: {ErrorMessage}";

    public static string ErrorGeneratingMigration =>
        "Error generating migration. Check command output for details.";
}
