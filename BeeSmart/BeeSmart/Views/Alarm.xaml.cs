

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
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using Timer = System.Timers.Timer;

namespace BeeSmart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Alarm : PopupPage
    {
        MetaInformation metaInformation;

        public Alarm()
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
        public void loadShowDay()
        {
           
            if (!listDay[0])
                btn2.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn2.BackgroundColor = Color.WhiteSmoke;
            
            if (!listDay[1])
                btn3.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn3.BackgroundColor = Color.WhiteSmoke;
          
            if (!listDay[2])
                btn4.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn4.BackgroundColor = Color.WhiteSmoke;
         
            if (!listDay[3])
                btn5.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn5.BackgroundColor = Color.WhiteSmoke;
           
            if (!listDay[4])
                btn6.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn6.BackgroundColor = Color.WhiteSmoke;
          
            if (!listDay[5])
                btn7.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn7.BackgroundColor = Color.WhiteSmoke;
           
            if (!listDay[6])
                btn8.BackgroundColor = Color.FromHex("#cccccc");
            else
                btn8.BackgroundColor = Color.WhiteSmoke;
        }
        public async void LoadAlarm()
        {
            dateOn.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute,0);
            dateOFF.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getDelay2&nameBoard=" + G.boardSelect.name + "&name=" + name);
            var response = await client.GetAsync("http://giacongpcb.vn/beehome/action.php?action=getControlsDelays&board=" + G.boardSelect.Mac + "&users=" + G.User);
            string responseString = await response.Content.ReadAsStringAsync();
            responseString = responseString.Replace("\"", "");
            responseString = responseString.Replace("{", "");
            responseString = responseString.Replace("}", "");
            String[] spDelay = responseString.Split(';');
            iDelay = 0;
            try
            {
                iDelay = Convert.ToInt32(spDelay[gpioSelect.idx]);
            }
            catch { }
            gpioSelect.iDelay = iDelay;
            if (iDelay == 0)
            {
                btnDelay.BackgroundColor = Color.WhiteSmoke; btnDelay.Text = "OFF";
            }

            else
            {
                btnDelay.BackgroundColor = Color.FromHex("#f9d667");
                btnDelay.Text = iDelay + " phút";
            }


            return;

            response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getAlarm2&board=" + G.boardSelect.name + "&users=" + G.User + "&name=" + name );
            responseString = await response.Content.ReadAsStringAsync();

            responseString = responseString.Replace("\"", "");
            responseString = responseString.Replace("{", "");
            responseString = responseString.Replace("}", "");
            String[] S5 = responseString.Split(',');

            foreach (String s6 in S5)
            {
                try
                {
                    String[] sp3 = s6.Split(':');

                    String nameGPIO = sp3[0];
                    sp3[1] = sp3[1].Replace("\\n", "\n");
                    String[] Alarms = sp3[1].Split('\n');
                    gridOn.Children.Clear();
                 
                    listHourOff = new List<string>();
                    listHourOn = new List<string>();

                    if (sp3[1].Trim() == "")
                    {
                      
                      //  stackAlarm.IsVisible = false;
                        listDay = new List<bool>()
                        {
                            true,true,true,true,true,true,true
                        };
                        loadShowDay();
                        break;

                    }
                  
                    gpioSelect.sAlarm = sp3[1];
                   // stackAlarm.IsVisible = true;
                 //   stackAlarm.IsEnabled = true;
                    listDay = new List<bool>();
                    foreach (string day in Alarms[0].Split('_'))
                    {
                        if (day == "") continue;
                        listDay.Add(Convert.ToBoolean(day));
                    }
                    loadShowDay();
                    List<string> listAlarm = new List<string>();
                
                    foreach (string dayOn in Alarms[1].Split('_'))
                    {
                        if (dayOn == "") continue;
                        string[] sp = dayOn.Split('-');
                        int Hour = Convert.ToInt32(sp[0]);
                        int Min = Convert.ToInt32(sp[1]);
                        DateTime date = new DateTime(2000, 8, 1, Hour, Min, 0); ;
                        String tm = date.ToString("HH-mm");
                        if (listHourOn.Count > 0)
                            if (listHourOn.FindIndex(a => a == tm) > -1) return;
                        listHourOn.Add(tm);
                        listAlarm.Add(date.ToString("HH:mm"));
                    }
                    int i = 0;
                    foreach (string dayOff in Alarms[2].Split('_'))
                    {
                        if (dayOff == "") continue;
                        string[] sp = dayOff.Split('-');
                        int Hour = Convert.ToInt32(sp[0]);
                        int Min = Convert.ToInt32(sp[1]);

                        DateTime date = new DateTime(2000, 8, 1, Hour, Min, 0); ;
                        String tm = date.ToString("HH-mm");
                        if (listHourOff.Count > 0)
                            if (listHourOff.FindIndex(a => a == tm) > -1) return;
                        listHourOff.Add(tm);
                        listAlarm[i] += " - " + date.ToString("HH:mm");
                        i++;
                    }
                    buttonList = new List<Button>();
                    foreach (string timer in listAlarm)
                    {
                        Button btn = new Button
                        {
                            CornerRadius = 5,
                            Margin = 5,
                            HeightRequest = 40,
                            Text = timer,
                            FontSize = 10,
                            TextColor = Color.Black,
                            BackgroundColor = Color.White,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,

                        };
                        buttonList.Add(btn);
                        btn.Clicked += Btn_Clicked;
                        int c = gridOn.Children.Count();

                        Grid.SetRow(btn, 0);
                        Grid.SetColumn(btn, c);
                        gridOn.Children.Add(btn);
                    }
                
                }
                catch(Exception)
                { 
                }
            }
        }
   
        private void LoadPara()
        {
            if (G.boardSelect.type != "F1")
            {
                gridIO.IsVisible = true;
                gridIO.Children.Clear();
                gridIO.RowDefinitions.Clear();
                int col2 = 0;
                RowDefinition row = new RowDefinition();
                row.Height = 60;
                gridIO.RowDefinitions.Add(row);
                if (G.boardSelect.GPIOs.FindIndex(a => a == gpioSelect) == -1)
                {
                    gpioSelect = G.boardSelect.GPIOs[0];
                }
                foreach (GPIO gpio in G.boardSelect.GPIOs)
                {
                    Color cl = Color.FromHex("#6e6e6e");

                    if (!Is)
                    {
                        cl = Color.FromHex("#f9d667");

                        Type = gpio.type;
                        gpioSelect = gpio;
                        name = gpio.name;
                    }
                    else
                    {
                        if (gpioSelect != null)
                        {
                           
                            if (gpioSelect.name == gpio.name)
                            {
                                cl = Color.FromHex("#f9d667");
                                name = gpioSelect.name;
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


                    listBtnGPIOs.Add(new ListBtnGPIO(G.boardSelect.Mac, G.boardSelect.name, gpio.name, imgBtn));

                    Grid.SetRow(imgBtn, 0);
                    Grid.SetColumn(imgBtn, col2);

                    col2++;

                    gridIO.Children.Add(imgBtn);

                }
                if(gpioSelect== null)
                {
                    List<GPIO> gpios = G.boardSelect.GPIOs;
                    int indexGPIO = 0;

                    name = gpios[0].name;

                    Type = gpios[indexGPIO].type;
                    int state = gpios[indexGPIO].state;
                    gpioSelect = gpios[indexGPIO];
                    
                }
            }
         else
            {
                name = "Power";
                gridIO.IsVisible = false;


                List<GPIO> gpios = G.boardSelect.GPIOs;
                int indexGPIO = 0;

                name = gpios[0].name;

                Type = gpios[indexGPIO].type;
                int state = gpios[indexGPIO].state;
                gpioSelect = gpios[indexGPIO];
            }    
            LoadAlarm();
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

        String name, Type;
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

            name = gpios[indexGPIO].name;
           
            Type = gpios[indexGPIO].type;
            int state = gpios[indexGPIO].state;
            gpioSelect = gpios[indexGPIO];
            gpioSelect.idx = indexGPIO;
            name = gpioSelect.name;
            LoadAlarm();
        }


        private static readonly HttpClient client = new HttpClient();

      
      
  
        List<String> listHourOn = new List<string>();
        List<String> listHourOff = new List<string>();
        List<String> listStringOnOff = new List<string>();
        List<Button> buttonList = new List<Button>();
        public async void addDate()
        {
            int HourOn=dateOn.Time.Hours; int MinOn = dateOn.Time.Minutes;
            int HourOff = dateOFF.Time.Hours; int MinOff = dateOFF.Time.Minutes;
            if (HourOn!=HourOff|| MinOn!=MinOff)
                {
                DateTime dateOn = new DateTime(2000, 8, 1, HourOn, MinOn, 0); ;
                DateTime dateOff = new DateTime(2000, 8, 1, HourOff, MinOff, 0); ;
                String tm = dateOn.ToString("HH-mm");
                if (listHourOn.Count > 0)
                    if (listHourOn.FindIndex(a => a == tm) > -1) return;
                listHourOn.Add(tm);
                 tm = dateOff.ToString("HH-mm");
                if (listHourOff.Count > 0)
                    if (listHourOff.FindIndex(a => a == tm) > -1) return;
                listHourOff.Add(tm);
                listStringOnOff.Add(dateOn.ToString("HH:mm") + "-" + dateOff.ToString("HH:mm"));
                Button btn = new Button
                {
                    CornerRadius = 5,
                    Margin = 5,
                    HeightRequest = 40,
                    Text = listStringOnOff[listStringOnOff.Count()-1],
                    FontSize = 10,
                    TextColor = Color.Black,
                    BackgroundColor = Color.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,

                };
                buttonList.Add(btn);
                btn.Clicked += Btn_Clicked;
                int c = gridOn.Children.Count();

                Grid.SetRow(btn, 0);
                Grid.SetColumn(btn, c);
                gridOn.Children.Add(btn);
            }
           
            else
            {
                await DisplayAlert("Alarm", "Trùng giờ tắt mở", "OK");
            }    
           
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
          
            int index = buttonList.FindIndex(a => a == btn);
            if (index < 0) return;
            String[] sp = btn.Text.Split('-');
            String tmOn = sp[0].Replace(":","-");
            String tmOff = sp[1].Replace(":", "-");
            listHourOn.Remove( tmOn.Trim());
            listHourOff.Remove(tmOff.Trim());
            gridOn.Children.Remove(btn);
        }

        private void btnPlusOn_Clicked(object sender, EventArgs e)
        {

            addDate();
        }
        List<bool> listDay = new List<bool>()
        {
            false,false,false,false,false,false,false
        };
        private void btn2_Clicked(object sender, EventArgs e)
        {
            listDay[0] = !listDay[0];
            if (!listDay[0])
                btn2.BackgroundColor = Color.Gray;
            else
                btn2.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn3_Clicked(object sender, EventArgs e)
        {
            listDay[1] = !listDay[1];
            if (!listDay[1])
                btn3.BackgroundColor = Color.Gray;
            else
                btn3.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn4_Clicked(object sender, EventArgs e)
        {
            listDay[2]= !listDay[2];
            if (!listDay[2])
                btn4.BackgroundColor = Color.Gray;
            else
                btn4.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn5_Clicked(object sender, EventArgs e)
        {
            listDay[3] = !listDay[3];
            if (!listDay[3])
                btn5.BackgroundColor = Color.Gray;
            else
                btn5.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn6_Clicked(object sender, EventArgs e)
        {
            listDay[4] = !listDay[4];
            if (!listDay[4])
                btn6.BackgroundColor = Color.Gray;
            else
                btn6.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn7_Clicked(object sender, EventArgs e)
        {
            listDay[5]= !listDay[5];
            if (!listDay[5])
                btn7.BackgroundColor = Color.Gray;
            else
                btn7.BackgroundColor = Color.WhiteSmoke;
        }

        private void btn8_Clicked(object sender, EventArgs e)
        {
            listDay[6]= !listDay[6];
            if (!listDay[6])
                btn8.BackgroundColor = Color.Gray;
            else
                btn8.BackgroundColor = Color.WhiteSmoke;
        }
        int iDelay = 0;

        private void btnDelay_Clicked(object sender, EventArgs e)
        {
            iDelay += 30;
            if (iDelay > 180) iDelay = 0;
            if (iDelay == 0)
            {
                btnDelay.BackgroundColor=Color.WhiteSmoke; btnDelay.Text = "OFF";
            }    
             
            else
            {
                btnDelay.BackgroundColor = Color.FromHex("#f9d667");
                btnDelay.Text = iDelay + " phút";
            }    
                
        }

        private async void btnApply_Clicked(object sender, EventArgs e)
        {

            var responseString2 = "";

            if (iDelay != gpioSelect.iDelay)
            {


                var response = await client.GetAsync("http://giacongpcb.vn/beehome/action.php?action=getControlsDelays&board=" + G.boardSelect.Mac + "&users=" + G.User);
                responseString2 = await response.Content.ReadAsStringAsync();

                responseString2 = responseString2.Replace("\"", "");
                responseString2 = responseString2.Replace("{", "");
                responseString2 = responseString2.Replace("}", "");
                String[] S4 = responseString2.Split(';');


                int state = 0;
                string type = gpioSelect.type;
                if (iDelay != 0)
                {
                    if (type.Trim().Contains("key"))
                    {
                        state++;
                        if (state > 3) state = 1;

                    }

                    else
                    {
                        if (state == 0) state = 1;
                        else state = 0;
                        if (type.Trim().Contains("door") || type.Trim().Contains("click")) state = 1;
                        if (type.Trim().Contains("nc")) state = 0;
                    }
                }
                gpioSelect.state = state;
                G.boardSelect.GPIOs[gpioSelect.idx].state = state;
                string states = "";
                string delays = "";
                for (int i = 0; i < G.boardSelect.GPIOs.Count; i++)
                {
                    states += G.boardSelect.GPIOs[i].state.ToString() + ";";
                    if (i == gpioSelect.idx)
                        delays += Convert.ToString(iDelay) + ";";
                    else delays += S4[i] + ";";
                }




                //String s = "https://giacongpcb.vn/esp-outputs-action.php?action=output_status2&name=" + gpioSelect.name + "&board=" + G.boardSelect.name + "&users=" + G.User + "&state=1";
                string s = "http://giacongpcb.vn/beehome/action.php?action=set_Controls&board=" + gpioSelect.Mac + "&States=" + states + "&Delays=" + delays + "&users=" + G.User;
                var response2 = await client.GetAsync(s);


                responseString2 = await response2.Content.ReadAsStringAsync();
            }


            //check set alarm
            bool isAlarm = false;
            for (int i = 0; i < listDay.Count; i++)
            {
                if (listDay[i] == true)
                {
                    isAlarm = true;
                    break;
                }
            }

            //list ngày
            if (isAlarm)
            {
                gpioSelect.sAlarm = "";
                foreach (bool s1 in listDay)
                {
                    gpioSelect.sAlarm += s1 + "_";
                }
                gpioSelect.sAlarm += Environment.NewLine;
                if (listHourOn.Count == 0)
                    gpioSelect.sAlarm += "_";
                foreach (string s2 in listHourOn)
                {
                    gpioSelect.sAlarm += s2 + "_";
                }
                gpioSelect.sAlarm += Environment.NewLine;
                if (listHourOff.Count == 0)
                    gpioSelect.sAlarm += "_";
                foreach (string s3 in listHourOff)
                {
                    gpioSelect.sAlarm += s3 + "_";
                }
                gpioSelect.sAlarm += Environment.NewLine;


                var response = await client.GetAsync("http://giacongpcb.vn/beehome/action.php?action=getControlsEvents&board=" + G.boardSelect.Mac + "&users=" + G.User);
                responseString2 = await response.Content.ReadAsStringAsync();

                responseString2 = responseString2.Replace("\"", "");
                responseString2 = responseString2.Replace("{", "");
                responseString2 = responseString2.Replace("}", "");
                String[] S4 = responseString2.Split(';');

                //check tung event -> event availabal -> changed;
                int inAlarm = -1;
                List<String> S5 = new List<String>();
                for (int i = 0; i < S4.Length; i++)
                {
                    if (S4[i] == "") S5.Add(S4[i]);
                }

                if (S5 != null)
                {
                    for (int i = 0; i < S5.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(S5[0]))
                        {
                            String[] S6 = S5[i].Split('_');
                            if (S6.Count() > 2)
                            {
                                if (!string.IsNullOrEmpty(S6[0]) && !string.IsNullOrEmpty(S6[1]))
                                {
                                    if ((Convert.ToInt32(S6[0]) == 1) && (Convert.ToInt32(S6[1]) == gpioSelect.idx))
                                    {
                                        inAlarm = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }


                if (inAlarm > -1)
                {

                }

                /*
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=updateAlarm&board=" + G.boardSelect.name + "&users=" + G.User + "&name=" + name + "&alarm=" + gpioSelect.sAlarm + "&Delay=" + iDelay);

                var responseString = await response.Content.ReadAsStringAsync();
                */
                /*
                if (responseString.Length > 0)
                {
                    await DisplayAlert("Thông báo", responseString, "OK");
                }*/
                

            }

            LoadPara();
            G.history.LoadIO();

            
            /*else
            {
                await DisplayAlert("Lỗi", "Thử lại", "OK");
            }*/



        }
    }
}