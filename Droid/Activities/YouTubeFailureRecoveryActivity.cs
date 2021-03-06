﻿using Android.App;
using Android.Content;
using Android.Widget;
using Google.YouTube.Player;

namespace Kwikee.Droid.Activities
{  
    public abstract class YouTubeFailureRecoveryActivity : YouTubeBaseActivity, IYouTubePlayerOnInitializedListener
    {
        private static int RECOVERY_DIALOG_REQUEST = 1;

        public void OnInitializationFailure (IYouTubePlayerProvider provider, YouTubeInitializationResult errorReason)
        {
            if (errorReason.IsUserRecoverableError)
            {
                errorReason.GetErrorDialog (this, RECOVERY_DIALOG_REQUEST).Show ();
            }
            else
            {
                var errorMessage = string.Format (GetString (Resource.String.error_player), errorReason);
                Toast.MakeText (this, errorMessage, ToastLength.Long).Show ();
            }
        }

        public virtual void OnInitializationSuccess (IYouTubePlayerProvider provider, IYouTubePlayer player, bool wasRestored)
        {
        }

        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RECOVERY_DIALOG_REQUEST)
            {
                GetYouTubePlayerProvider ().Initialize (DeveloperKey.Key, this);
            }
        }

        protected abstract IYouTubePlayerProvider GetYouTubePlayerProvider ();
    }
}

