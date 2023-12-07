using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class State
    {
        public string gpio1;
        public bool state1;
        public string gpio2;
        public bool state2;
        public string gpio3;
        public bool state3;
        public string gpio4;
        public bool state4;

        public State(string gpio1 ,bool state1,string gpio2,bool state2, string gpio3, bool state3, string gpio4, bool state4)
        {
            this.gpio1 = gpio1;
            this.gpio2 = gpio2;
            this.gpio3 = gpio3;
            this.gpio4 = gpio4;
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
            this.state4 = state4;
        }
    }
}
