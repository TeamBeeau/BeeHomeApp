using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BeeHome.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeSmart.Droid.Class;
using static Android.Net.ConnectivityManager;
using Xamarin.Forms;
using static Android.Provider.SyncStateContract;
using static BeeSmart.Droid.Class.NativeWifi;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;
using Application = Xamarin.Forms.Application;

[assembly: Xamarin.Forms.Dependency(typeof(NativeWifi))]
namespace BeeSmart.Droid.Class
{

    public class NativeWifi : InativeWifi
    {


        private bool _requested;
        private bool _statusConnect;

        private NetworkCallback _callback;
        private Context _context = null;
        private Version _version;
        private WifiManager _wifiManager = null;
        private ConnectivityManager _connectivityManager;
        private WifiConfiguration _config;
        private int _temp = -1;

        public NativeWifi()
        {
            this._context = Android.App.Application.Context;
            _version = DeviceInfo.Version;
            _wifiManager = _context.GetSystemService(Context.WifiService) as WifiManager;
        }

        [Obsolete]
        public async Task<bool> ConnectToWifi(string ssid, string password, Action<bool> animation = null)
        {
          
                if (!_wifiManager.IsWifiEnabled)
            {
                if (_version.Major >= 9)
                {
                    bool result = await Device.InvokeOnMainThreadAsync(async () => await Application.Current.MainPage.DisplayAlert("", "The program requires accesss to Wi-Fi. Turn on Wi-fi?", "Ok", "Cancel"));

                    if (!result)
                    {
                        return false;
                    }

                    Intent intent;

                    if (_version.Major == 9)
                    {
                        intent = new Intent(Android.Provider.Settings.ActionWifiSettings);
                    }
                    else
                    {
                        intent = new Intent(Android.Provider.Settings.Panel.ActionInternetConnectivity);
                    }

                    intent.AddFlags(ActivityFlags.NewTask);
                    Android.App.Application.Context.StartActivity(intent);
                }
                else
                {
                    _wifiManager.SetWifiEnabled(true);
                }
            }
            else
            {

                if (_version.Major <= 9 && _version.Major >= 8)
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Geolocation.GetLastKnownLocationAsync());
                    JoinToWifiLessAndroidQAsync(ssid, password, animation);
                }
                else if (_version.Major < 8)
                {
                    JoinToWifiLessAndroidQAsync(ssid, password, animation);
                }
                else
                {
                    await Device.InvokeOnMainThreadAsync(async () => await Geolocation.GetLastKnownLocationAsync());
                    await JoinToWifiMoreAndroidPie(ssid, password);
                }
            }

            return await Task.FromResult(_statusConnect);
        }

        [Obsolete]
        public async Task<IEnumerable<string>> GetAvailableNetworksAsync()
        {

            IEnumerable<string> availableNetworks = null;

            // Get a handle to the Wifi
            if (!_wifiManager.IsWifiEnabled)
                _wifiManager.SetWifiEnabled(true);
            var wifiReceiver = new WifiReceiver(_wifiManager);

            await Task.Run(() =>
            {
                // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
                _context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                availableNetworks = wifiReceiver.Scan();
            });

            return availableNetworks;
        }

        private class NetworkCallback : ConnectivityManager.NetworkCallback
        {
            private ConnectivityManager _connectivityManager;

            public NetworkCallback(ConnectivityManager connectivityManager)
            {
                _connectivityManager = connectivityManager;
            }
            public Action<Network> NetworkAvailable { get; set; }
            public Action NetworkUnavailable { get; set; }

            public override void OnAvailable(Network network)
            {
                _connectivityManager.BindProcessToNetwork(network);
                base.OnAvailable(network);
                NetworkAvailable?.Invoke(network);
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();
                NetworkUnavailable?.Invoke();
            }
        }

        [BroadcastReceiver(Enabled = true, Exported = false)]
        class WifiReceiver : BroadcastReceiver
        {
            private WifiManager _wifi;
            private List<string> _wifiNetworks;
            private AutoResetEvent _receiverARE;
            private Timer _tmr;
            private const int TIMEOUT_MILLIS = 20000; // 20 seconds timeout

            public WifiReceiver()
            {

            }

            public WifiReceiver(WifiManager wifi)
            {
                this._wifi = wifi;
                _wifiNetworks = new List<string>();
                _receiverARE = new AutoResetEvent(false);
            }

            [Obsolete]
            public IEnumerable<string> Scan()
            {
                _tmr = new Timer(Timeout, null, TIMEOUT_MILLIS, System.Threading.Timeout.Infinite);
                _wifi.StartScan();
                _receiverARE.WaitOne();
                return _wifiNetworks;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanwifinetworks = _wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    _wifiNetworks.Add(wifinetwork.Ssid);
                }

                _receiverARE.Set();
            }

            private void Timeout(object sender)
            {
                // NOTE release scan, which we are using now, or we throw an error?
                _receiverARE.Set();
            }
        }

        [Obsolete]
        private void JoinToWifiLessAndroidQAsync(string ssid, string password, Action<bool> animation)
        {
            animation?.Invoke(true);

            _config = new WifiConfiguration
            {
                Ssid = "\"" + ssid + "\"",
                PreSharedKey = "\"" + password + "\""
            };

            try
            {
                _temp = _wifiManager.AddNetwork(_config);
                _wifiManager.Disconnect();
                var result = _wifiManager.EnableNetwork(_temp, true);
                _wifiManager.Reconnect();

                int i = 0;

                do
                {
                    Thread.Sleep(2000);
                    //wait connection
                    i++;
                    if (i == 7)
                        break;

                } while (GetCurrentConnectName() != ssid);

                Thread.Sleep(6000);

                if (i == 7)
                {
                    throw new Exception("Connect to PC failed. Long time connect(14000ms)");
                }
                else
                {
                    _statusConnect = true;
                }
            }
            catch (Exception ex)
            {
                //  Helpers.Logger.Error($"{nameof(WifiService)}||JoinToWifiLessAndroidQ||{ex.Message}");
                _statusConnect = false;
            }
        }

        [Obsolete]
        private async Task<bool> JoinToWifiMoreAndroidPie(string ssid, string password)
        {
            var specifier = new WifiNetworkSpecifier.Builder()
                           .SetSsid(ssid)
                           .SetWpa2Passphrase(password)
                           .Build();

            var request = new NetworkRequest.Builder()
                           .AddTransportType(TransportType.Wifi)
                           .RemoveCapability(NetCapability.Internet)
                           .SetNetworkSpecifier(specifier)
                           .Build();

            _connectivityManager = _context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;

            if (_requested)
            {
                _connectivityManager.UnregisterNetworkCallback(_callback);
            }

            bool confirmConnect = false;

            _callback = new NetworkCallback(_connectivityManager)
            {
                NetworkAvailable = network =>
                {
                    // we are connected!
                    _statusConnect = true;
                    confirmConnect = true;
                },
                NetworkUnavailable = () =>
                {
                    _statusConnect = false;
                    confirmConnect = true;
                }
            };

            _connectivityManager.RequestNetwork(request, _callback);
            _requested = true;

            do
            {
                //wait callback
                await Task.Delay(TimeSpan.FromSeconds(5));
                //    Helpers.Logger.Info($"{nameof(WifiService)}||JoinToWifiMoreAndroidPie||Waiting callback....");

            } while (!confirmConnect);

            return await Task.FromResult(true);
        }

        public string GetCurrentConnectName()
        {
            WifiInfo wifiInfo = _wifiManager.ConnectionInfo;
            if (wifiInfo.SupplicantState == SupplicantState.Completed)
            {
                char[] chars = { '\"' };
                var masChar = wifiInfo.SSID.Trim(chars);
                return masChar;
            }
            else
            {
                return null;
            }
        }

       // [Obsolete]
        //public async Task ReconnectToWifi()
        //{
        //    if (_version.Major > 9)
        //    {
        //        _connectivityManager.UnregisterNetworkCallback(_callback);
        //        await Task.Delay(10000);
        //        var network = _connectivityManager.ActiveNetwork;

        //        if (network == null)
        //        {
        //            var dataNetwork = await ManagerSecureStorage.GetConnectedNetworkInfo();
        //            await JoinToWifiMoreAndroidPie(dataNetwork["NetName"], dataNetwork["Password"]);
        //        }
        //        else
        //        {
        //            _connectivityManager.BindProcessToNetwork(network);
        //        }
        //    }
        //    else
        //    {
        //        if (_temp == -1)
        //        {
        //            var temp = _wifiManager.ConfiguredNetworks;
        //            _temp = temp.Last().NetworkId;
        //        }

        //        _wifiManager.RemoveNetwork(_temp);
        //        _wifiManager.Reconnect();
        //        await Task.Delay(10000);
        //    }
        //}
    

        }
    
   
}