namespace iTravelerServer.Domain;

public class Error
{
    public Error(){}
    
    public Error(int errorCode, string errorMessage, string errorDetails = null)
    {
        ErrorCode = errorCode;
        this.ErrorMessage = errorMessage;
        ErrorDetails = errorDetails;
    }

    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; }
}