using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class Board
    {
        public bool IsOnline = true;
        public DateTime lastTime;
        public string name, board,type, last_request;
        public List<int> States = new List<int>();
        public List<GPIO> GPIOs = new List<GPIO>();
        public Board(string name, List<GPIO> GPIOs)
        {
            this.name = name;
            this.GPIOs = GPIOs;

        }
        public Board(string name, string type, List<GPIO> GPIOs)
        {
            this.type= type;
            this.name = name;
            this.GPIOs = GPIOs;

        }

    }
}
