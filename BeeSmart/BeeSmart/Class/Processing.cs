using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeFactory
{
   public class Processing
    {
        public String User;
        public List<String> listID = new List<String>();

        public String name,type;
        public Processing(String name,String type, String User,  List<string> listID)
        {
            this.User = User;
            this.type = type;
            this.name = name;
            this.listID = listID;
        }
    }
}
