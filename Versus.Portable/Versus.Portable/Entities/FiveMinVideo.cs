using System;
using System.Collections.Generic;

namespace FiveMin.Portable.Entities
{
    public class FiveMinVideo : BaseEntity
    {
        public TimeSpan Length { get; set; }
        public string VideoUrl { get; set; }
        public uint Likes { get; set; }
        public uint Dislikes { get; set; }
        public uint WatchCount { get; set; }
        public List<string> Categories { get; set; }
    }
}
