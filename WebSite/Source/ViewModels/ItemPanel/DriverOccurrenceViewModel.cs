namespace Driver.WebSite.ViewModels.ItemPanel
{
    public class DriverOccurrenceViewModel
    {
        public int Id { set; get; }
        public int DriverId { get; set; }
        public string Plate { set; get; }
        public string Description { set; get; }
        public int StartSecond { set; get; }
        public int EndSecond { set; get; }
        public int DownVotesCount { set; get; }
        public int Rate => -DownVotesCount;

        public int UserVote { set; get; }
    }
}