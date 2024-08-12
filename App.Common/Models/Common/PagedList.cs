namespace App.Common.Models.Common
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
