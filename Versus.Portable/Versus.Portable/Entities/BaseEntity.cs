using System;
using System.Collections.Generic;

namespace Kwikee.Portable.Entities
{
    public class BaseEntity
    {
        public string Name
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool IsWatched
        {
            get;
            set;
        }

        public List<string> Keywords
        {
            get;
            set;
        }

        public string ImageUrl { get; set; }

        public DateTime DateAdded { get; set; }
    }
}

