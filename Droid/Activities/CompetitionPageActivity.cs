using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Square.Picasso;
using Versus.Droid.Helpers;
using Versus.Portable.Data;
using Versus.Portable.Entities;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Versus.Droid.Activities
{
    [Activity(Label = "Competition Page")]
    public class CompetitionPageActivity : BaseActivity
    {
        private VsEntity _selectedEntity;

        protected override int LayoutResource => Resource.Layout.fragment_competitionDetails;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }


    }
}