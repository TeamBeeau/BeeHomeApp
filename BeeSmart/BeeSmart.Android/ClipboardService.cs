using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BeeSmart.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BeeSmart.Droid
{
    public class ClipboardService : IClipboardService
    {
        public string GetTextFromClipboard()
        {
            var clipboardmanager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            var item = clipboardmanager.PrimaryClip.GetItemAt(0);
            var text = item.Text;
            return text;
        }

        public void SendTextToClipboard(string text)
        {
            // Get the Clipboard Manager
            var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);

            // Create a new Clip
            var clip = ClipData.NewPlainText("YOUR_TITLE_HERE", text);

            // Copy the text
            clipboardManager.PrimaryClip = clip;
        }
    }
}