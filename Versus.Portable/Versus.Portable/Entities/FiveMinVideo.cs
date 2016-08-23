using System;
using System.Collections.Generic;

namespace Kwikee.Portable.Entities
{
    public class FiveMinVideo : BaseEntity
    {
        public TimeSpan Length { get; set; }
        public string VideoId { get; set; }
        public uint Likes { get; set; }
        public uint Dislikes { get; set; }
        public uint WatchCount { get; set; }
        public List<string> Categories { get; set; }
    }
}
