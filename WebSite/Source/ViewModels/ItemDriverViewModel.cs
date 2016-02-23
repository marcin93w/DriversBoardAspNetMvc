namespace Driver.WebSite.ViewModels
{
    public class ItemDriverViewModel
    {
        public int Id { set; get; }
        public string Plate { set; get; }
        public string Description { set; get; }
        public int StartSecond { set; get; }
        public int EndSecond { set; get; }
        public int DownVotesCount { set; get; }
    }
}