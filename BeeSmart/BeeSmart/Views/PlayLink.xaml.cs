

using CodeShare.Library.Models.MetaData;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
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

    public partial class PlayLink : PopupPage
    {
        MetaInformation metaInformation;

        public PlayLink()
        {
            InitializeComponent();
            App.Resumed += App_Resumed;
            Clipboard.ClipboardContentChanged += OnClipboardContentChanged;


            G.worker.DoWork += Worker_DoWork;
            G.worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            // this.SizeChanged += LayoutFrame_SizeChanged;
        }


        private void App_Resumed(object sender, EventArgs e)
        {
            this.OnAppearing();
            //  await PopupNavigation.PushAsync(this);

        }

        public Frame CreateFrameLink(String Title, String Des, String imgUrl)
        {
            ImageButton imgLink = new ImageButton
            {
                Source = imgUrl,

                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White

            };
            ImageButton imgPlay = new ImageButton
            {
                Source = "youtube2.png",
                Margin = new Thickness(0, 30, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Transparent

            };

            G.metaInformations[G.metaInformations.Count - 1].imgLink = imgPlay;

            imgPlay.Clicked += ImgLink_Clicked;



            var lbTitle = new Label
            {

                Text = Title,
                Margin = new Thickness(10, 5, 10, 0),

                MaxLines = 1,
                TextColor = Color.WhiteSmoke,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start
            };


            var lbDes = new Label
            {

                Text = Des,
                Margin = new Thickness(10, 10, 10, 0),
                TextColor = Color.WhiteSmoke,
                FontSize = 10,

                VerticalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start
            };

            RelativeLayout stackLayout = new RelativeLayout
            {

                BackgroundColor = Color.WhiteSmoke,
                Margin = new Thickness(-25, -25, -25, -25),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,

            };
            stackLayout.Children.Add(imgLink, Constraint.Constant(1));
            stackLayout.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                       {
                         lbTitle,
                         imgPlay,
                          lbDes
                       }
            }, Constraint.Constant(1));

            return new Frame


            {

                WidthRequest = 250,
                HeightRequest = 180,
                HasShadow = true,
                CornerRadius = 10,
                Margin = new Thickness(0, 5, 0, 10),

                HorizontalOptions = LayoutOptions.Start,
                Content = stackLayout



            };
        }

        private void ImgLink_Clicked(object sender, EventArgs e)
        {
            ImageButton imageButton = (ImageButton)sender;
            int index = G.metaInformations.FindIndex(a => a.imgLink == imageButton);
            if (index >= 0)
            {
                

            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsLink == true)
            {

                Frame frame = CreateFrameLink(G.metaInformations[G.metaInformations.Count - 1].Title, "Mô tả: " + G.metaInformations[G.metaInformations.Count - 1].Description, G.metaInformations[G.metaInformations.Count - 1].ImageUrl);
                G.metaInformations[G.metaInformations.Count - 1].Frame = frame;
                layoutFrame.Children.Add(frame);


            }

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
          
        }

        bool IsLink = false;
    
        public void Complete()
        {

        }

        private async void OnClipboardContentChanged(object sender, EventArgs e)
        {



        }

        Timer tmLoad;
        // Handle when your app resumes

        public void RefreshLink()
        {
            if (!G.worker.IsBusy)
                G.worker.RunWorkerAsync();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
      
            editHost.Text = Preferences.Get("Host", "sensor.hungphuthai.vn");
            editPort.Text = Preferences.Get("Port", "5055");
            editData.Text = Preferences.Get("Data", "HPTSENSORTHUDUC");
            editUser.Text = Preferences.Get("User2", "sensor");
            editPass.Text = Preferences.Get("Pass", "HPT@232316#");
            /* Action action = () => RefreshLink();

             Device.StartTimer(TimeSpan.FromSeconds(1), () =>
             {
                 Device.BeginInvokeOnMainThread(action);

                 return true;
             });*/

        }

        private void TmLoad_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmLoad.Enabled = false;
           
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }
        bool IsPlay = false, IsMute;







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

        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("Host", editHost.Text);
            Preferences.Set("Port", editPort.Text);
            Preferences.Set("Data", editData.Text);
            Preferences.Set("User2", editUser.Text);
            Preferences.Set("Pass", editPass.Text);
          
            G.IsConnectAgain = true;
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnMute_Clicked(object sender, EventArgs e)
        {
            
               
        }
    }
}