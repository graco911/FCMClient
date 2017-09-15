using Android.App;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace FCMClient
{
    [Activity(Label = "FCMClient", MainLauncher = true)]
    public class MainActivity : Activity
    {
        TextView msgText;
        const string TAG = "MainActivity";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            Log.Debug(TAG, "google app id: " + Resource.String.google_app_id);

            SetContentView(Resource.Layout.Main);

            msgText = FindViewById<TextView>(Resource.Id.msgText);

            if(Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }


            IsPlayServicesAvailable();
            var logTokenButton = FindViewById<Button>(Resource.Id.logTokenButton);
            var subscribebutton = FindViewById<Button>(Resource.Id.subscribeButton);
            logTokenButton.Click += delegate
            {
                Log.Debug(TAG, "Instance ID Token: " + FirebaseInstanceId.Instance.Token);
            };

            subscribebutton.Click += delegate
            {
                FirebaseMessaging.Instance.SubscribeToTopic("news");
                Log.Debug(TAG, "Suscrito a las notificaciones remotas");
            };
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if(resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    msgText.Text = "This Device is not suported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText.Text = "Google Play Services is available.";
                return true;
            }
        }
    }
}

