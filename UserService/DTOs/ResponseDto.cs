namespace UserService.DTOs;

public class ResponseDto
{
    public bool Success { get; set; }       // Indicates if the operation was successful
    public string Message { get; set; }     // Descriptive message (e.g., "User created successfully")
}