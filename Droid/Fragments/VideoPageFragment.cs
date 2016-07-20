using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Square.Picasso;
using FiveMin.Droid.Helpers;
using FiveMin.Portable.Data;
using FiveMin.Portable.Entities;

namespace FiveMin.Droid.Fragments
{
    public class VideoPageFragment : Android.Support.V4.App.Fragment
    {
        private VsEntity _selectedEntity;
        private TextView _leftVotesTextView;
        private TextView _rightVotesTextView;

        public VideoPageFragment ()
        {
            RetainInstance = true;
        }

        private void UpdateUiForSelectedEntity (TextView entityName, TextView entityDescription)
        {
            entityName.Text = _selectedEntity.Name;
            entityDescription.Text = _selectedEntity.Description;
        }

        public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView (inflater, container, savedInstanceState);

            //var view = inflater.Inflate(Resource.Layout.fragment_categories, container, false);
            var view = inflater.Inflate (Resource.Layout.fragment_competitionDetails, null);
            PopulateDataAsync (view);

            return view;
        }

        Intent CreateShareIntent ()
        {
            var setShareIntent = new Intent (Intent.ActionSend);
            setShareIntent.SetType ("text/plain");
            setShareIntent.PutExtra (Intent.ExtraSubject, GetString (Resource.String.share_competition_message_subject));
            setShareIntent.PutExtra (Intent.ExtraText, "I just voted in " + Competition.Name + " for " + _selectedEntity.Name +". How about you? #FiveMin");

            return setShareIntent;
        }

        async void PopulateDataAsync (Android.Views.View view)
        {
            var competition = await FirebaseManager.Instance.GetCompetition (Video.Name);
            var parentView = view.FindViewById<FrameLayout> (Resource.Id.parentLayout);

            if (competition != null)
            {
                var e1ImageButton = view.FindViewById<ImageButton> (Resource.Id.leftEntityButton);
                var e2ImageButton = view.FindViewById<ImageButton> (Resource.Id.rightEntityButton);
                var entityName = view.FindViewById<AppCompatTextView> (Resource.Id.entityName);
                var votingButton = view.FindViewById<FloatingActionButton> (Resource.Id.votingButton);
                var entityDescription = view.FindViewById<AppCompatTextView> (Resource.Id.entityDescription);
                _leftVotesTextView = view.FindViewById<TextView> (Resource.Id.leftEntityVotes);
                _rightVotesTextView = view.FindViewById<TextView> (Resource.Id.rightEntityVotes);

                FontsHelper.ApplyTypeface (view.Context.Assets, new List<TextView> { entityName, entityDescription, _leftVotesTextView, _rightVotesTextView });

                var entities = await FirebaseManager.Instance.GetAllEntities ();

                var entity1 = entities.Values.FirstOrDefault (entity => string.Equals (entity.Name, competition.CompetitorName1, StringComparison.CurrentCultureIgnoreCase));
                var entity2 = entities.Values.FirstOrDefault (entity => string.Equals (entity.Name, competition.CompetitorName2, StringComparison.CurrentCultureIgnoreCase));

                if (entity1 != null && entity2 != null)
                {
                    _selectedEntity = entity1;

                    _leftVotesTextView.Text = competition.CompetitorScore1.ToString ();
                    _rightVotesTextView.Text = competition.CompetitorScore2.ToString ();

                    // Load the image asynchonously
                    Picasso.With (view.Context).Load (entity1.ImageUrl).Into (e1ImageButton);
                    Picasso.With (view.Context).Load (entity2.ImageUrl).Into (e2ImageButton);
                    UpdateUiForSelectedEntity (entityName, entityDescription);

                    votingButton.Click += (sender, args) =>
                    {
                        var index = _selectedEntity == entity1 ? 1 : 2;
                        FirebaseManager.Instance.UpdateVote (index, Competition.Name);
                        switch (index)
                        {
                        case 1:
                            _leftVotesTextView.Text = (competition.CompetitorScore1).ToString ();
                            break;
                        case 2:
                            _rightVotesTextView.Text = (competition.CompetitorScore2).ToString ();
                            break;
                        }
                        Snackbar.Make (parentView, "Casted a vote for " + _selectedEntity.Name, Snackbar.LengthLong)
                                .SetAction("Share", (v) =>
                                {
                                    StartActivity(CreateShareIntent ());
                                })
                                .Show ();
                    };
                }

                e1ImageButton.Click += (sender, args) =>
                {
                    _selectedEntity = entity1;
                    UpdateUiForSelectedEntity (entityName, entityDescription);
                };

                e2ImageButton.Click += (sender, args) =>
                {
                    _selectedEntity = entity2;
                    UpdateUiForSelectedEntity (entityName, entityDescription);
                };

                var progressBar = view.FindViewById<ProgressBar> (Resource.Id.loadingCompetitionProgressBar);
                var mainLayout = view.FindViewById<LinearLayout> (Resource.Id.mainCompetitionLayout);

                progressBar.Visibility = Android.Views.ViewStates.Gone;
                mainLayout.Visibility = Android.Views.ViewStates.Visible;
            }
        }

        public FiveMinVideo Video { get; set; }
    }
}

