using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

using Android.Content;
using System.Net;
using Xamarin.Essentials;

using Location = Android.Locations.Location;
using Android.Util;

using Environment = Android.OS.Environment;
using Java.IO;
using System.Threading.Tasks;
using Java.Net;
using static Java.Util.ResourceBundle;
using Xamarin.Forms.Platform.Android;
using BeeHome.Class;
using BeeSmart.Droid.Class;

namespace BeeSmart.Droid 
{

    [Activity(Label = "BeeHome", Icon = "@drawable/bee", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
   
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ILocationListener
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
        && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
            {
                var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                RequestPermissions(permissions, 1);

            }
            DependencyService.Register<NativeHelper>();
            //  InitializeLocationManager();

            // ImageCircleRenderer.Init();
            Rg.Plugins.Popup.Popup.Init(this);

           // global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        // UpdateApp();


        }
     

        Location currentLocation;
        float[] distance = new float[1];
        double distances = 0;
       
        
        LocationManager locationManager;
        string locationProvider;
        protected override void OnStart()
        {
            base.OnStart();
           
           
        }
      public static   Intent intent=new Intent();
       
        protected override void OnStop()
        {
            base.OnStop();

        }
      
            private void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);
            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }

        }
        protected override void OnResume()
        {
            base.OnResume();
           
        }
      
        protected override void OnPause()
        {
            base.OnPause();
           
        }
        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
           
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
     
        public void OnProviderDisabled(string provider)
        {
           // throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
           // throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }
    }
}