
using BeeHome.Class;
using BeeSmart.Class;
using BeeSmart.Services;
using BeeSmart.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeeSmart
{
    public partial class App : Application
    {


       
        public App()
        {
          //  DependencyService.Register<MockDataStore>();
            Device.SetFlags(new string[] { "CollectionView_Experimental", "SwipeView_Experimental" });
           
           
            MainPage = G.history;
           

        }

 
 
protected void OnUserLeaveHint()
        {
           OnUserLeaveHint();
           
        }

     

        protected override void OnStart()
        {
            G.IsInternet = G.history.CheckNet();

        }
       
        protected override void OnSleep()
        {
            G.IsSleep = true;
          //  DependencyService.Get<IAndroidService>().StartService();
        }
        public static event EventHandler Resumed;
        protected override void OnResume()
        {
           
            base.OnResume();
            G.IsSleep = false;
            G.IsInternet = G.history.CheckNet();
            Resumed?.Invoke(this, EventArgs.Empty);

        }


    }
}
