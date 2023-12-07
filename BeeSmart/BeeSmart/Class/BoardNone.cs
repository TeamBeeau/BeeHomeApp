using System;
using System.Collections.Generic;
using System.Text;

namespace SFS_HPT.Class
{
    public class BoardNone
    {
        public String board, last_request;
        public BoardNone (String board, String last_request)
        {
            this.board = board;
            this.last_request = last_request;
        }
    }
}
