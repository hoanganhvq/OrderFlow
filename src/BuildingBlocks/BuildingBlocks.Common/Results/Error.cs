namespace BuildingBlocks.Common.Results;

public record Error (string Code, string Description, ErrorType ErrorType)
{
    
    public string Code { get; }
    public string Description { get; }  
    public ErrorType ErrorType { get; }
    
    public static Error None =>
        new Error(string.Empty, string.Empty, ErrorType.Failure);
    
    public static Error NotFound(string code, string description) 
        => new Error(code, description, ErrorType.NotFound);
    
    public static Error Failure(string code, string description) 
        => new Error(code, description, ErrorType.Failure); 
    
    public static Error Validation(string code, string description) 
        => new Error(code, description, ErrorType.Validation);
    
    public static Error AccessForbidden(string code, string description) 
        => new Error(code, description, ErrorType.AccessForbidden);
    
    public static Error Conflict(string code, string description) 
        => new Error(code, description, ErrorType.Conflict);
    
    public static Error AccessUnAuthorized(string code, string description) 
        => new Error(code, description, ErrorType.AccessUnAuthorized);
}