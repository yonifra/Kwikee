using System;
using System.Collections.Generic;
using System.Linq;

namespace Kwikee.Portable.Helpers
{
    public static class StringHelper
    {
        public static string TimeSpanFormatter (TimeSpan ts)
        {
            return string.Format ("{0:D}:{1:D2}", ts.Hours, ts.Minutes);
        }

        public static string TagsFormatter (List<string> stringsList)
        {
            const int maxCount = 3;

            // we want to show the list as "#Dev #Programming #Development #Coding"
            // if the list contains more than 3 keywords, we'll show the first three, and the rest will be shown as such:
            // #Dev #Programming #Development (+1)

            var newList = new List<string> ();

            if (stringsList.Count > maxCount)
            {
                for (var i = 0; i < maxCount; i++)
                {
                    newList.Add ("#" + stringsList [i]);
                }

                newList.Add ("(+" + (stringsList.Count - maxCount) + ")");
            }
            else
            {
                newList = stringsList.Select (s => "#" + s).ToList ();
            }

            return string.Join (" ", newList);
        }

        public static string TrimText (string original, int length)
        {
            return original.Length < length ? original : original.Substring (0, length) + "...";
        }
    }
}

