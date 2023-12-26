using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class GPIO
    {
        public string Mac;
        public string name;
        public int state;
        public int iDelay;
        public string type;
        public string sAlarm;
        public int idx;
        public GPIO(string name, int state)
        {
            this.name = name;
            this.state = state;
        }
        public GPIO(string name, int state, string mac)
        {
            this.name = name;
            this.state = state;
            this.Mac = mac;
        }
        public GPIO(string name, int state, string mac, int idx)
        {
            this.Mac = mac;
            this.name = name;
            this.state = state;
            this.idx = idx;
        }

        public GPIO()
        {

        }
    }
}
