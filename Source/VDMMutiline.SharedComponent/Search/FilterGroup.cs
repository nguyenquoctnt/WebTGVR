using System.Collections.Generic;

namespace VDMMutiline.SharedComponent.Search
{
    public class FilterGroup<T> : List<IFilter<T>>
    {
        public string InnerLogical { get; set; }
    }
}
