using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.OrderStatus.Domain.Common;
public class OperationResult<T>
{
    // Properties
    public bool Success { get; private set; }
    public T? Data { get; private set; }
    public string? Message { get; private set; }
    public int? StatusCode { get; private set; }
    public bool HasData => Data is not null;

    // Non-static constructor for flexibility, such as dependency injection or instantiation in certain patterns
    public OperationResult(bool success, T? data = default, string? message = null, int? statusCode = null)
    {
        Success = success;
        Data = data;
        Message = message;
        StatusCode = statusCode;
    }

    // Static Factory Methods for creating common success and failure results
    public static OperationResult<T> SuccessResult(T data) =>
        new OperationResult<T>(true, data);

    public static OperationResult<T> SuccessResult(T data, string message) =>
        new OperationResult<T>(true, data, message);

    public static OperationResult<T> Failure(string errorMessage, int statusCode = 500) =>
        new OperationResult<T>(false, default, errorMessage, statusCode);

    public static OperationResult<T> Failure(Exception ex) =>
        new OperationResult<T>(false, default, ex.Message, ex.HResult);

    // Generic Factory Method for custom success or failure results
    public static OperationResult<T> From(bool success, T? data = default, string? message = null, int? statusCode = null) =>
        new OperationResult<T>(success, data, message, statusCode);
}
