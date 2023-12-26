

using CodeShare.Library.Models.MetaData;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Animations;
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

    public partial class AddBoard : PopupPage
    {
        MetaInformation metaInformation;

        public AddBoard()
        {
            InitializeComponent();
            web.Source = "http://192.168.4.1/user=" + G.User;
            App.Resumed += App_Resumed;
        
        }


        private void App_Resumed(object sender, EventArgs e)
        {
            this.OnAppearing();
            //  await PopupNavigation.PushAsync(this);

        }

        List<ListBtnGPIO> listBtnGPIOs = new List<ListBtnGPIO>();

        List<string> listType = new List<string>()
        {
            "lamp","fan","door","pump"
        };
     
        protected async override void OnAppearing()
        {
          
            base.OnAppearing();

           
               // BoardNone boardNone = Newtonsoft.Json.JsonConvert.DeserializeObject<BoardNone>(G.sBoardNone);
             //   editMac.Text = boardNone.board;
               // lbDate.Text = "Last Request time " + boardNone.last_request;

            


        }
        String nameOld, Type;
        private async void ImgBtn_Clicked(object sender, EventArgs e)
        {
         
        }

        private async void btnApply_Clicked(object sender, EventArgs e)
        {
            // if (G.listBoardRoom == null) G.listBoardRoom = "";
            // String listnameBoard = G.listBoardRoom.Trim() + "_" + editName.Text;

            //  listnameBoard = listnameBoard.Replace("__", "_");
            //var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getTypeMac&board=" + editMac.Text);
            var response = await client.GetAsync("https://giacongpcb.vn/beehome/action.php?action=get_boardNew&users=" + G.User);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString = responseString.Replace("\"", "");
            responseString = responseString.Replace("{", "");
            responseString = responseString.Replace("}", "");
            String[] S3 = responseString.Split(':');
            String nMac = "";
            String nType = "";
            String defname = "";
            if (responseString.Length > 8 && S3.Length >= 3)
            {
                nMac = S3[0];
                nType = S3[1];
                defname = S3[2];
            }
            else
            {
                await DisplayAlert("Error", "Không tìm thấy thiết bị", "OK");
                return;
            }
            foreach (Button btn in G.history.btnsHome)
            {
                if (btn.TextColor != Color.Gray)
                {
                    G.nameRoom = btn.Text;
                }
            }
            String url = "";
            /*if (responseString.Contains("F1"))
            {
                url = "https://giacongpcb.vn/esp-outputs-action.php?action=InsertFan1&name=" + editName.Text + "&board=" + editMac.Text + "&users=" + G.User + "&home=" + G.nameRoom;
            }
            else
            {
                url = "https://giacongpcb.vn/esp-outputs-action.php?action=InsertNewBoard&name=" + editName.Text + "&board=" + editMac.Text + "&users=" + G.User + "&home=" + G.nameRoom;
                
            }*/
            url = "https://giacongpcb.vn/beehome/action.php?action=InsertNewBoard&name=" + editName.Text + "&board=" + nMac + "&users=" + G.User + "&home=" + G.nameRoom + "&type=" + nType + "&defname=" + defname;
            response = await client.GetAsync(url);
            responseString = await response.Content.ReadAsStringAsync();
            if (responseString.Length > 0)
            {

                G.history.LoadItem();
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                //await DisplayAlert("Error", "Trùng tên Board", "OK");
                await DisplayAlert("Error", "Trùng tên Device", "OK");
            }
        }

        private static readonly HttpClient client = new HttpClient();

       

     
    }
}