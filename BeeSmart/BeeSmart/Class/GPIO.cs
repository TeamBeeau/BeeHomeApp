using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class GPIO
    {
        public string name;
        public int state;
        public int iDelay;
        public string type;
        public string sAlarm;
        public GPIO(string name, int state)
        {
            this.name = name;
            this.state = state;
       

        }
        public GPIO()
        {

        }
    }
}
