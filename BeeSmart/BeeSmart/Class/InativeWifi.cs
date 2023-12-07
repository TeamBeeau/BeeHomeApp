using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeeHome.Class
{
    public  interface InativeWifi
    {
        Task<bool> ConnectToWifi(string ssid, string password, Action<bool> animation = null);
    }
}
