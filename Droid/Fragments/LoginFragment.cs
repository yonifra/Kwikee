
using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FiveMin.Droid.Fragments
{
    public class LoginFragment : Android.Support.V4.App.Fragment, View.IOnClickListener,
        GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        const string TAG = "LoginFragment";
        const int RC_SIGN_IN = 9001;
        const string KEY_IS_RESOLVING = "is_resolving";
        const string KEY_SHOULD_RESOLVE = "should_resolve";

        View _view;

        GoogleApiClient mGoogleApiClient;
        TextView mStatus;

        bool mIsResolving;
        bool mShouldResolve;

        public LoginFragment ()
        {
            RetainInstance = true;
        }

        public void OnConnected (Bundle connectionHint)
        {
            Log.Debug (TAG, "onConnected:" + connectionHint);

            UpdateUI (true);

            //GoogleSignInResult result = mGoogleApiClient.GetConnectionResult(PlusClass.API).
            //GoogleSignInAccount acct = result.SignInAccount;
            //var personName = acct.DisplayName;
            //var personEmail = acct.Email;
            //var personId = acct.Id;
            //var personPhoto = acct.PhotoUrl;
        }

        public void OnConnectionFailed (ConnectionResult result)
        {
            Log.Debug (TAG, "onConnectionFailed:" + result);

            if (!mIsResolving && mShouldResolve)
            {
                if (result.HasResolution)
                {
                    try
                    {
                        result.StartResolutionForResult (Activity, RC_SIGN_IN);
                        mIsResolving = true;
                    }
                    catch (IntentSender.SendIntentException e)
                    {
                        Log.Error (TAG, "Could not resolve ConnectionResult.", e);
                        mIsResolving = false;
                        mGoogleApiClient.Connect ();
                    }
                }
                else {
                    ShowErrorDialog (result);
                }
            }
            else {
                UpdateUI (false);
            }
        }

        class DialogInterfaceOnCancelListener : Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            public Action<IDialogInterface> OnCancelImpl { get; set; }

            public void OnCancel (IDialogInterface dialog)
            {
                OnCancelImpl (dialog);
            }
        }

        void ShowErrorDialog (ConnectionResult connectionResult)
        {
            int errorCode = connectionResult.ErrorCode;

            if (GooglePlayServicesUtil.IsUserRecoverableError (errorCode))
            {
                var listener = new DialogInterfaceOnCancelListener ();
                listener.OnCancelImpl = (dialog) =>
                {
                    mShouldResolve = false;
                    UpdateUI (false);
                };
                GooglePlayServicesUtil.GetErrorDialog (errorCode, Activity, RC_SIGN_IN, listener).Show ();
            }
            else {
                Snackbar.Make (_view, Resource.String.play_services_error_fmt, Snackbar.LengthShort).Show ();

                mShouldResolve = false;
                UpdateUI (false);
            }
        }

        public async void OnClick (View v)
        {
            switch (v.Id)
            {
            case Resource.Id.sign_in_button:
                mStatus.Text = GetString (Resource.String.signing_in);
                mShouldResolve = true;
                mGoogleApiClient.Connect ();
                break;
            case Resource.Id.sign_out_button:
                if (mGoogleApiClient.IsConnected)
                {
                    PlusClass.AccountApi.ClearDefaultAccount (mGoogleApiClient);
                    mGoogleApiClient.Disconnect ();
                }
                UpdateUI (false);
                break;
            case Resource.Id.disconnect_button:
                if (mGoogleApiClient.IsConnected)
                {
                    PlusClass.AccountApi.ClearDefaultAccount (mGoogleApiClient);
                    await PlusClass.AccountApi.RevokeAccessAndDisconnect (mGoogleApiClient);
                    mGoogleApiClient.Disconnect ();
                }
                UpdateUI (false);
                break;
            }
        }

        public override void OnStart ()
        {
            base.OnStart ();
            mGoogleApiClient.Connect ();
        }

        public override void OnStop ()
        {
            base.OnStop ();
            mGoogleApiClient.Disconnect ();
        }

        public override void OnSaveInstanceState (Bundle outState)
        {
            base.OnSaveInstanceState (outState);
            outState.PutBoolean (KEY_IS_RESOLVING, mIsResolving);
            outState.PutBoolean (KEY_SHOULD_RESOLVE, mIsResolving);
        }

        public override void OnActivityResult (int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult (requestCode, resultCode, data);
            Log.Debug (TAG, "onActivityResult:" + requestCode + ":" + resultCode + ":" + data);

            if (requestCode == RC_SIGN_IN)
            {
                if (resultCode != (int)Result.Ok)
                {
                    mShouldResolve = false;
                }

                mIsResolving = false;
                mGoogleApiClient.Connect ();
            }
        }

        public void OnConnectionSuspended (int cause)
        {
            Log.Warn (TAG, "onConnectionSuspended:" + cause);
        }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView (inflater, container, savedInstanceState);

            _view = inflater.Inflate (Resource.Layout.login_fragment, null);

            if (savedInstanceState != null)
            {
                mIsResolving = savedInstanceState.GetBoolean (KEY_IS_RESOLVING);
                mShouldResolve = savedInstanceState.GetBoolean (KEY_SHOULD_RESOLVE);
            }

            _view.FindViewById (Resource.Id.sign_in_button).SetOnClickListener (this);
            _view.FindViewById (Resource.Id.sign_out_button).SetOnClickListener (this);
            _view.FindViewById (Resource.Id.disconnect_button).SetOnClickListener (this);

            _view.FindViewById<SignInButton> (Resource.Id.sign_in_button).SetSize (SignInButton.SizeWide);
            _view.FindViewById (Resource.Id.sign_in_button).Enabled = false;

            mStatus = _view.FindViewById<TextView> (Resource.Id.status);

            mGoogleApiClient = new GoogleApiClient.Builder (Activity)
                .AddConnectionCallbacks (this)
                .AddOnConnectionFailedListener (this)
                .AddApi (PlusClass.API)
                .AddScope (new Scope (Scopes.Profile))
                .Build ();

            return _view;
        }

        void UpdateUI (bool isSignedIn)
        {
            if (_view == null) return;

            if (isSignedIn)
            {
                var person = PlusClass.PeopleApi.GetCurrentPerson (mGoogleApiClient);
                var name = string.Empty;
                if (person != null)
                    name = person.DisplayName;
                mStatus.Text = string.Format (GetString (Resource.String.signed_in_fmt), name);

                _view.FindViewById (Resource.Id.sign_in_button).Visibility = ViewStates.Gone;
                _view.FindViewById (Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Visible;
            }
            else {
                mStatus.Text = GetString (Resource.String.signed_out);

                _view.FindViewById (Resource.Id.sign_in_button).Enabled = true;
                _view.FindViewById (Resource.Id.sign_in_button).Visibility = ViewStates.Visible;
                _view.FindViewById (Resource.Id.sign_out_and_disconnect).Visibility = ViewStates.Gone;
            }
        }
    }
}

