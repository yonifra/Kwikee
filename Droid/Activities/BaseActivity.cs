﻿using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace Kwikee.Droid.Activities
{
    public abstract class BaseActivity : AppCompatActivity
    {
        public Toolbar Toolbar { get; set; }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            //  Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
            base.OnCreate (savedInstanceState);
            SetContentView (LayoutResource);
            Toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);

            if (Toolbar != null)
            {
                SetSupportActionBar (Toolbar);
            //    SupportActionBar.SetDisplayHomeAsUpEnabled (true);
                SupportActionBar.SetHomeButtonEnabled (true);
             //   SupportActionBar.SetShowHideAnimationEnabled (true);
           //     SupportActionBar.SetDefaultDisplayHomeAsUpEnabled (true);
          //      SupportActionBar.SetLogo (Resource.Drawable.logo);
              //  SupportActionBar.SetDisplayShowHomeEnabled (true);
                //     SupportActionBar.SetIcon (Resource.Drawable.menu);
                //     SupportActionBar.SetHomeAsUpIndicator (Resource.Drawable.menu);
              //  SupportActionBar.SetHomeButtonEnabled (true);
           //     SupportActionBar.SetDisplayShowTitleEnabled (true);
                //       SupportActionBar.SetLogo (Resource.Drawable.logo);
            }
        }

        protected abstract int LayoutResource { get; }

        protected int ActionBarIcon { set { Toolbar.SetNavigationIcon (value); } }
    }
}

