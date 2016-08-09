
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace FiveMin.Droid.Activities
{
    [Activity (Label = "Kwikee", LaunchMode = LaunchMode.SingleTop, MainLauncher = true, Icon = "@mipmap/icon", Theme= "@style/SplashTheme")]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            var intent = new Intent (this, typeof (MainActivity));
            StartActivity (intent);
            Finish ();
        }
    }
}

