namespace Driver.WebSite.ViewModels
{
    public class PaginationViewModel
    {
        public PaginationViewModel(int currentPageId, bool areThereMoreItems)
        {
            IsPreviousPage = currentPageId > 1;
            IsNextPage = areThereMoreItems;
            PrevPageId = currentPageId - 1;
            NextPageId = currentPageId + 1;
        }

        public bool IsPreviousPage { get; }
        public bool IsNextPage { get; }
        public int PrevPageId { get; }
        public int NextPageId { get; }
    }
}
