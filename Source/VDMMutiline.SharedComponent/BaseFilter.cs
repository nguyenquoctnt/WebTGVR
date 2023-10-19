namespace VDMMutiline.SharedComponent
{
    public abstract class BaseFilter
    {
        public string Search { get; set; }
        public int? ItemId { get; set; }

        public PagingInfo Paging { get; set; }
    }
}
