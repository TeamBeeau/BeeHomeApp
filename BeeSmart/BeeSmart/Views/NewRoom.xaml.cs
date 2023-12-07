

using CodeShare.Library.Models.MetaData;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SFS_HPT.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Timer = System.Timers.Timer;

namespace BeeSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NewRoom : PopupPage
    {
        MetaInformation metaInformation;

        public NewRoom()
        {
            InitializeComponent();
            App.Resumed += App_Resumed;
        
        }


        private void App_Resumed(object sender, EventArgs e)
        {
            this.OnAppearing();
            //  await PopupNavigation.PushAsync(this);

        }

   

        protected async override void OnAppearing()
        {
            base.OnAppearing();
        
            /*  editHost.Text=   Preferences.Get("Source", string.Empty);
                 Preferences.Get("Source", string.Empty);
                 Preferences.Get("Source", string.Empty);
                 Preferences.Get("Source", string.Empty);
                 Preferences.Get("Source", string.Empty);
                 editHost.Text = Preferences.Get("Host", "sensor.hungphuthai.vn");
                 editPort.Text = Preferences.Get("Port", "1443");
              */

        }

      
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }
       






        private void btnPause_Clicked(object sender, EventArgs e)
        {
            

        }

        private void btnUpVol_Clicked(object sender, EventArgs e)
        {
            


        }

        private void btnDownVol_Clicked(object sender, EventArgs e)
        {
           
        }

        private void btnSpace_Clicked(object sender, EventArgs e)
        {

         
        }


        private static readonly HttpClient client = new HttpClient();
        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=InsertRoom&name=" + editRoom.Text.Trim() + "&users="+ G.User );

            var responseString = await response.Content.ReadAsStringAsync();
            if(responseString.Length>0)
            {

                G.history.LoadItem();
                await PopupNavigation.Instance.PopAsync();
            }    
            else
            {
                await DisplayAlert("Error", "Trùng tên phòng", "OK");
            }    
        /*    if (G.SqlServer.Check("*","Acc","Users=N'"+editUser.Text+"' AND Pass='"+editPass.Text+"'",G.con))
            {
                Preferences.Set("User", editUser.Text);
                Preferences.Set("Pas", editPass.Text);
                G.User = editUser.Text;
                G.Pass = editPass.Text;
                G.Level = G.SqlServer.Table("Level", "Acc", "Users=N'" + editUser.Text + "' AND Pass='" + editPass.Text + "'", G.con).Rows[0]["Level"].ToString();
                G.IsLogin = true;
                G.history.Load();
                await PopupNavigation.Instance.PopAsync();
               
            }  
            else
            {
                await DisplayAlert("Login", "Đăng nhập thất bại " , "OK");

            }*/
        }
    }
}