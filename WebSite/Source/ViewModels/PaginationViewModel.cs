namespace Driver.WebSite.ViewModels
{
    public class PaginationViewModel
    {
        public PaginationViewModel(string basePageHref, int currentPageId, bool areThereMoreItems)
        {
            IsPreviousPage = currentPageId > 1;
            IsNextPage = areThereMoreItems;
            BasePageHref = basePageHref;
            PrevPageId = currentPageId - 1;
            NextPageId = currentPageId + 1;
        }

        public string BasePageHref { get; }

        public bool IsPreviousPage { get; }
        public bool IsNextPage { get; }
        public int PrevPageId { get; }
        public int NextPageId { get; }
    }
}
