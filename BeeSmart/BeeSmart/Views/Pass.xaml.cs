

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

    public partial class Pass : PopupPage
    {
        MetaInformation metaInformation;

        public Pass()
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
            editUser.Text = Preferences.Get("User", "");
            editPass.Text = Preferences.Get("Pas", "");
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
            if (IsCreate)
            {
                if(editAgain.Text!=editPass.Text)
                {
                    await DisplayAlert("Register", "Mật khẩu không trùng khớp", "OK");
                    return;
                }    
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=register&Name=" + editUser.Text + "&Pass=" + editPass.Text);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Length > 0)
                {

                    Preferences.Set("User", editUser.Text);
                    Preferences.Set("Pas", editPass.Text);
                    DisplayAlert("Register", "Đăng ký thành công ", "OK");
                    G.User = editUser.Text;
                    G.Pass = editPass.Text;
                    G.IsLogin = true;
                    G.history.Load();
                    gridLogin.Children.RemoveAt(4);
                    gridLogin.Children.RemoveAt(4);
                    btnRegister.IsVisible = true;
                    btnLogin.Text = "Login";
                    IsCreate = false;
                 //   await PopupNavigation.Instance.PopAsync();
                   
                }
                else
                {
                    await DisplayAlert("Login", "Đăng ký thất bại ", "OK");
                }
            }
            else
            {
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=login&name=" + editUser.Text + "&pass=" + editPass.Text);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString != "null")
                {

                    Preferences.Set("User", editUser.Text);
                    Preferences.Set("Pas", editPass.Text);

                    G.User = editUser.Text;
                    G.Pass = editPass.Text;
                    G.IsLogin = true;
                    G.history.Load();
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    Preferences.Set("User", editUser.Text);
                    Preferences.Set("Pas", editPass.Text);
                    await DisplayAlert("Login", "Đăng nhập thất bại ", "OK");
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
        Editor editAgain;
        bool IsCreate=false;
        private void btnRegister_Clicked(object sender, EventArgs e)
        {
            if (!IsCreate)
            {
                IsCreate = true;
                Label lb = new Label
                {

                    Text = "Nhập lại mật khẩu",
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 12,
                    FontAttributes = FontAttributes.None,
                    TextColor = Color.Black,

                };
                editAgain = new Editor()
                {
                    BackgroundColor = Color.FromHex( "#e0c05c"),
                 HorizontalOptions = LayoutOptions.Fill,
                 VerticalOptions= LayoutOptions.Center,
                    Text = ""
                };
                Grid.SetRow(lb, 2);
                Grid.SetColumn(lb, 0);
                Grid.SetRow(editAgain, 2);
                Grid.SetColumn(editAgain, 1);
                btnRegister.IsVisible = false;
                btnLogin.Text = "Register";
                gridLogin.Children.Add(lb);
                gridLogin.Children.Add(editAgain);

            }
        }
    }
}