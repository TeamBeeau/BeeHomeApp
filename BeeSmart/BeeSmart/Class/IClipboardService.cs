using System;
using System.Collections.Generic;
using System.Text;

namespace BeeSmart.Class
{
    public interface IClipboardService
    {
        string GetTextFromClipboard();
        void SendTextToClipboard(string text);
    }
}
