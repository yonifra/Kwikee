using System.Collections.Generic;

namespace FiveMin.Portable.Entities
{
    public class Category : BaseEntity
    {
        public List<FiveMinVideo> Videos { get; set; }
        public uint NumOfVideos => Videos != null ? (uint)Videos.Count : 0;
    }
}
