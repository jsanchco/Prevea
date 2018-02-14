namespace Prevea.IService.IService
{
    public enum Status { Ok, Error, Warning }

    public class Result
    {
        public string Message { get; set; }
        public object Object { get; set; }
        public Status Status;
    }
}
