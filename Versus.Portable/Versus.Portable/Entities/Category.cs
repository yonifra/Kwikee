using System.Collections.Generic;

namespace FiveMin.Portable.Entities
{
    public class Category : BaseEntity
    {
        public string BackdropUrl { get; set; }
        public string SmallIconUrl { get; set; }
        public List<FiveMinVideo> Videos { get; set; }
        public uint NumOfVideos
        {
            get
            {
                return Videos != null ? (uint)Videos.Count : 0;
            }
        }
    }
}
