using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class Home
    {
        public string name;
        public List<Board> board = new List<Board>();
        public Home(string name, List<Board> board)
        {
            this.name = name;
            this.board = board;
       
       
        }
    }
}
