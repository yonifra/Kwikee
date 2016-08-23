using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;

namespace Kwikee.Droid.Helpers
{
    public static class FontsHelper
    {
        private const string PATH_TO_FONT = "Fonts/Slabo.ttf";

        public static void ApplyTypeface (AssetManager assetManager, List<TextView> textViews)
        {
            var tf = Typeface.CreateFromAsset (assetManager, PATH_TO_FONT);

            foreach (var textview in textViews) 
            {
                textview.Typeface = tf;
            }
        }
    }
}

