using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SFS_HPT.Class
{
    public class ListBtnGPIO
    {
        public String nameBoard;
        public String GPIO;
        public ImageButton btnGPIO;
        public ListBtnGPIO (String nameBoard, String GPIO, ImageButton btnGPIO)
        {
            this.nameBoard = nameBoard;
            this.btnGPIO = btnGPIO;
            this.GPIO = GPIO;
        }
        public ListBtnGPIO()
        {
          
        }
    }
}
