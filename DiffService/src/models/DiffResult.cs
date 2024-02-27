namespace DiffService.src.models
{
    public class DiffResult
    {
        public bool IsSuccess { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public List<Difference> Differences { get; set;}
    }
}
