namespace TestTemplate6.Common.Paging
{
    public class Paging
    {
        public Paging(int currentPage, int totalPages, int totaItems, int currentPageSize)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            TotaItems = totaItems;
            CurrentPageSize = currentPageSize;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotaItems { get; }
        public int CurrentPageSize { get; set; }
    }
}
