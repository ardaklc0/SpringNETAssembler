namespace BussMaker.WebUI.Models
{
    public class DataTransferObjectViewModel
    {
        public List<Dictionary<string, string>> DataTransferObjectCreate { get; set; }
        public List<Dictionary<string, string>> DataTransferObjectUpdateAndGet { get; set; }
        public List<Dictionary<string, string>> RepositoryList { get; set; }
        public List<Dictionary<string, string>> ServiceList { get; set; }
    }
}
