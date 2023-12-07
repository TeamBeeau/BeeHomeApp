

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

    public partial class Edits : PopupPage
    {
        MetaInformation metaInformation;

        public Edits()
        {
            InitializeComponent();
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
            "lamp","fan","door","pump","click","socket","nc","none"
        };
        int index = -1; bool Is = false;
   
       
        private void LoadType()
        {
            gridType.Children.Clear();
            int c = 0;
            btnTypes = new List<ImageButton>();
            foreach (String name in listType)
            {
                Color cl = Color.Gray;
                if (name == Type)
                    cl = Color.FromHex("#f9d667");

                ImageButton imgType = new ImageButton
                {
                    CornerRadius = 5,
                 Margin=new Thickness(5,0,5,0),
                    HeightRequest = 40,
                    Source = name,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    BackgroundColor = cl
                    
                };
                Label lb = new Label
                {

                    Text = name,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    FontSize = 12,
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest=20,
                    TextColor = Color.White,

                };
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Children = { imgType, lb }
                };
                
                imgType.Clicked += ImgType_Clicked;
                btnTypes.Add(imgType);
                Grid.SetRow(stack, 0);
                Grid.SetColumn(stack, c);
                c++;
                gridType.Children.Add(stack);
            }
        }
        private void LoadPara()
        {
            gridIO.Children.Clear();
            gridIO.RowDefinitions.Clear();
            int col2 = 0;
            RowDefinition row = new RowDefinition();
            row.Height = 60;
            gridIO.RowDefinitions.Add(row);

         
            foreach (GPIO gpio in G.boardSelect.GPIOs)
            {
                Color cl = Color.FromHex("#6e6e6e");
                
                if (!Is)
                {  
                    cl = Color.FromHex("#f9d667");
                    editName.Text = gpio.name;
                 nameOld = editName.Text;
                    Type = gpio.type;
                    gpioSelect = gpio;
                }
                else
                {
                    if(gpioSelect!=null)
                    {
                        if (gpioSelect.name == gpio.name)
                        {
                            cl = Color.Gray;
                            editName.Text = gpio.name;
                             nameOld = editName.Text;
                            Type = gpio.type;
                        }
                    }    
                } 
                    
                ImageButton imgBtn = new ImageButton
                {
                    CornerRadius = 5,
                    Margin = 5,
                    HeightRequest = 50,
                    Source = gpio.type,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = cl

                };
                if (!Is)
                {
                    btnGPIO = imgBtn;

              
                }
                Is = true;
                imgBtn.Clicked += ImgBtn_Clicked;
               
                
                listBtnGPIOs.Add(new ListBtnGPIO(G.boardSelect.name, gpio.name, imgBtn));

                Grid.SetRow(imgBtn, 0);
                Grid.SetColumn(imgBtn, col2);
               
                col2++;

                gridIO.Children.Add(imgBtn);
               
            }
            LoadType();
            
        }
        List<ImageButton> btnTypes=new List<ImageButton>();
        protected async override void OnAppearing()
        {
            lbBoard.Text = G.boardSelect.name;
            base.OnAppearing();
          
         //   pickType.ItemsSource = listType;
            LoadPara();


        }

        private void ImgType_Clicked(object sender, EventArgs e)
        {
            ImageButton imgType= (ImageButton)sender;
            foreach(ImageButton img in btnTypes)
                img.BackgroundColor = Color.Gray;
            int index=btnTypes.IndexOf(imgType);
            if(index>-1)
            {
                Type = listType[index]; btnGPIO.Source = Type + ".png";

            }    
            imgType.BackgroundColor = Color.FromHex("#f9d667");
        }

        String nameOld, Type;
        ImageButton btnGPIO;
        GPIO gpioSelect = null;
        private void ImgBtn_Clicked(object sender, EventArgs e)
        {
            foreach (ListBtnGPIO btn in listBtnGPIOs)
            {
                btn.btnGPIO.BackgroundColor = Color.FromHex("#6e6e6e"); ;
                // btn.BackgroundColor = Color.WhiteSmoke;
            }
          
             btnGPIO = (ImageButton)sender;
            btnGPIO.BackgroundColor = Color.FromHex("#f9d667");
            int index = listBtnGPIOs.FindIndex(a => a.btnGPIO == btnGPIO);
            String nameBoard = listBtnGPIOs[index].nameBoard;
            List<Board> boards = G.listHome[G.indexHome].board;
            int indexBoard = boards.FindIndex(a => a.name.Contains(nameBoard));
            List<GPIO> gpios = G.listHome[G.indexHome].board[indexBoard].GPIOs;
            int indexGPIO = gpios.FindIndex(a => a.name.Contains(listBtnGPIOs[index].GPIO));
          
            editName.Text = gpios[indexGPIO].name;
            nameOld = editName.Text;
            Type = gpios[indexGPIO].type;
            int state = gpios[indexGPIO].state;
            gpioSelect = gpios[indexGPIO];
            LoadType();
           
        }


        private static readonly HttpClient client = new HttpClient();

      
     
  
        List<String> listHourOn = new List<string>();
        List<String> listHourOff = new List<string>();
        List<String> listStringOnOff = new List<string>();
        List<Button> buttonList = new List<Button>();
       

       
       
       

        

        private async void btnApply_Clicked(object sender, EventArgs e)
        {
            var choice = await DisplayAlert("Thay đổi ngõ ra", "Bạn muốn thay đổi", "YES", "NO");
            if (choice)
            {
              
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=updateGPIO&board=" + G.boardSelect.name + "&users=" + G.User + "&nameOld=" + nameOld + "&name=" + editName.Text.Trim() + "&type=" + Type);
                String responseString = await response.Content.ReadAsStringAsync();
                 G.boardSelect.GPIOs[G.boardSelect.GPIOs.FindIndex(a => a.name == gpioSelect.name)].type = Type;
                G.boardSelect.GPIOs[G.boardSelect.GPIOs.FindIndex(a => a.name == gpioSelect.name)].name = editName.Text ;
               
                if (responseString.Length > 0)
                {
                    await DisplayAlert("Thông báo", responseString, "OK");
                }
                LoadPara();
             G.history.LoadIO();
            }

        }
    }
}