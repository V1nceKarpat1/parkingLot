namespace parkingLotAPI.Utils
{

    public enum ResponseStatus
    {
        OK,
        NOT_FOUND,
        BAD_REQUEST,
        NO_CONTENT
    }
    public class ServiceResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public class ServiceResponse<T> : ServiceResponse
    {
        public T? ResponseData { get; set; }
    }

}
