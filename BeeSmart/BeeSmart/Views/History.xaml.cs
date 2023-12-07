using ArpLookup;
using BeeFactory;
using BeeHome.Class;
using BeeSmart.Class;
using BeeSmart.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using SFS_HPT.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using static System.Net.Mime.MediaTypeNames;
using Button = Xamarin.Forms.Button;
using ImageButton = Xamarin.Forms.ImageButton;
using SwipeView = Xamarin.Forms.SwipeView;
using Timer = System.Timers.Timer;

namespace BeeSmart.Views
{
    public partial class History : ContentPage
    {


        Timer tmBlink = new Timer();
        Timer tmOut = new Timer();
        public History()
        {
            InitializeComponent();
            tmOut.Interval = 200;
            tmOut.Elapsed += TmOut_Elapsed;

        }
        bool IsPressed = false;
        private void TmOut_Elapsed(object sender, ElapsedEventArgs e)
        {
          
                tmOut.Enabled = false;
                IsPressed = false;
          
        }

        bool IsSend = false;
        bool IsSend2 = false;
        private async void Blink()
        {
          
                var response = await client.GetAsync(sSendBlink);


                responseString = await response.Content.ReadAsStringAsync();
                if (responseString != "null")
                {

                    btnGPIO.BackgroundColor = Color.FromRgb(220, 220, 220); ;
            

                    G.listHome[G.indexHome].board[indexBoard].GPIOs[indexGPIO].state = 0;
                }
          
        }

        private async void NC()
        {

            var response = await client.GetAsync(sSendBlink);


            responseString = await response.Content.ReadAsStringAsync();
            if (responseString != "null")
            {

                btnGPIO.BackgroundColor = Color.FromHex("#f9d667");

                G.listHome[G.indexHome].board[indexBoard].GPIOs[indexGPIO].state = 1;
            }

        }
        private async void TmLoad_Elapsed(object sender, ElapsedEventArgs e)
        {
        }

        private void TmStock_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        bool IsLoad = false;
        public static ImageSource ImgFromByte(byte[] byteArrayIn)
        {


            return ImageSource.FromStream(() =>
            {
                return new MemoryStream(byteArrayIn);
            });


        }
        List<ImageButton> listUsers = new List<ImageButton>();
        List<String> listUser = new List<string>();
        public async void LoadOnLine()
        {
            if (IsPressed) return;
            try
            {
                int i = 0;
                foreach (Board board in G.listHome[G.indexHome].board)

                {

                    var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardTime&name=" + board.name + "&users=" + G.User);
                    responseString = await response.Content.ReadAsStringAsync();
                    if (responseString.Length < 10) return;
                    responseString = responseString.Replace("\"", "");
                    responseString = responseString.Replace("{", "");
                    responseString = responseString.Replace("}", "");

                    String[] S4 = responseString.Split(':');
                    if (S4.Count() <= 0) return;
                    String sTimer = responseString.Replace(S4[0]+":", "");
                   
                       
                        DateTime lastTime = DateTime.Parse(sTimer);
                        board.lastTime =  lastTime;
                    TimeSpan timeSpan = DateTime.Now - board.lastTime;
                    if(timeSpan.TotalSeconds>5)
                    {
                        listStackBoard[i].IsEnabled = false;
                        listStackBoard[i].BackgroundColor = Color.WhiteSmoke;
                        listFrame[i].BackgroundColor= Color.WhiteSmoke;
                        listFrame[i].IsEnabled = false;
                        listlbBoard[i].TextColor = Color.LightGray;
                        listlbLastTime[i].Text = "Last time: " + lastTime.ToString("HH:mm dd/MM/yyyy");
                        board.IsOnline = false;
                    }
                    else
                    {
                        board.IsOnline = true;
                        listStackBoard[i].IsEnabled = true;
                        listlbBoard[i].TextColor = Color.Black;
                        listStackBoard[i].BackgroundColor = Color.White;
                        listFrame[i].BackgroundColor = Color.White;
                        listFrame[i].IsEnabled = true;
                        listlbLastTime[i].Text = "";
                       
                    }
                    i++;
                    

                    

                }
            }
            catch (Exception ex)
            {
                //   DisplayAlert("err2", ex.Message, "OK");
            }
        }
     
        public  bool CheckNet()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
              return true;
            }
            else
            {
               return false;
            }

        }
  
        public void Load()
        {
            layMain.IsVisible = true;
            lbName.Text = G.User;
            

            try

            {
              

                    G.dtNow = DateTime.Now;
                    LoadItem();


                    Device.StartTimer(TimeSpan.FromMilliseconds(300), () =>
                    {
                        if (!G.IsSleep)
                        {
                            G.IsInternet = CheckNet();
                            if (G.IsInternet && !IsPressed )
                                LoadStatus();
                          
                        }
                        return true; // True = Repeat again, False = Stop the timer
                    });
                Device.StartTimer(TimeSpan.FromMilliseconds(5000), () =>
                {
                    if (!G.IsSleep)
                    {
                        G.IsInternet = CheckNet();
                        if (G.IsInternet && !IsPressed)
                            LoadOnLine();
                    }
                
                    return true; // True = Repeat again, False = Stop the timer
                });
              
            }
            catch (Exception ex)
            {
               // DisplayAlert("err1", ex.Message, "OK");
            }



        }
        ImageSource imgAvatar;
        bool IsQL = false;

        List<String> listMaterial = new List<string>();
        DateTime dateEnd = DateTime.Now;
        DateTime dateBegin = DateTime.Now.AddDays(-7);

        List<Button> listbtnDay = new List<Button>();

        private void Lbdays_Clicked(object sender, EventArgs e)
        {
            foreach (Button btns in listbtnDay)
            {
                btns.TextColor = Color.Black;
                btns.BackgroundColor = Color.WhiteSmoke;
            }

            Button btn = (Button)sender;
            btn.TextColor = Color.White;
            btn.BackgroundColor = Color.FromHex("#f9d667");
            int ix = listbtnDay.FindIndex(a => a == btn);
            if (ix > -1)
            {
                G.dtNow = DateTime.Now.AddDays(ix);

                //      LoadProcess(10000);
            }
        }

        public List<ImageButton> listBtnWorker = new List<ImageButton>();
        public List<Label> listLbWorker = new List<Label>();
        public List<String> listWorker = new List<String>();

        DataTable dtListWorker;

        DataTable dtProcess;
        DateTime dayProcess = DateTime.Now;



     






        List<Button> listBtnMachine = new List<Button>();
        DataTable dtMachine;
        String sMachine = "";
        int indexMachine = -1;
        DataTable dtHis;

        bool IsLoad2 = false;
        DataTable dtStatus;
        private static HttpClient client = new HttpClient();
        String responseString;
        List<Button> btnsHome = new List<Button>();
        List<ListBtnGPIO> listBtnGPIOs = new List<ListBtnGPIO>();

    
        public async void LoadItem()
        {
            try
            {
                //    var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardUser&users=" + G.User);
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getHome&users=" + G.User);


                var responseString = await response.Content.ReadAsStringAsync();

                responseString = responseString.Replace("\"", "");
                responseString = responseString.Replace("{", "");
                responseString = responseString.Replace("}", "");
                String[] sp = responseString.Split(',');
                gridItem.Children.Clear();
                stackBoard.Children.Clear();
                G.listHome = new List<Home>();
                foreach (String s in sp)
                {
                    String[] sBoard = s.Split(':');
                    G.nameRoom = sBoard[0];
                    try
                    {
                        List<Board> boards = new List<Board>();
                        response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardRoom&home=" + G.nameRoom + "&users=" + G.User);
                        responseString = await response.Content.ReadAsStringAsync();

                        responseString = responseString.Replace("\"", "");
                        responseString = responseString.Replace("{", "");
                        responseString = responseString.Replace("}", "");
                        String[] S5 = responseString.Split(',');

                        foreach (String s6 in S5)
                        {
                            String[] sp3 = s6.Split(':');
                            String board = sp3[0];
                            String typeBoard = sp3[1];
                            if (board.Trim() == "") continue;
                            List<GPIO> gpios = new List<GPIO>();
                            response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardStates&nameBoard=" + board + "&users=" + G.User);
                            responseString = await response.Content.ReadAsStringAsync();

                            responseString = responseString.Replace("\"", "");
                            responseString = responseString.Replace("{", "");
                            responseString = responseString.Replace("}", "");
                            String[] S4 = responseString.Split(',');

                            foreach (String s2 in S4)
                            {
                                if (s2 == "") continue;
                                String[] s3 = s2.Split(':');
                                if (s3.Count() < 2) continue;
                                gpios.Add(new GPIO(s3[0], Convert.ToInt32(s3[1])));
                            }

                            boards.Add(new Board(board, typeBoard, gpios));

                        }
                        G.listHome.Add(new Home(sBoard[0], boards));
                    }
                    catch(Exception ex)
                    {
                        G.listHome.Add(new Home(sBoard[0], null));
                    }
                   
                   
                }
                gridItem.Children.Clear(); int col = 0;
                gridItem.ColumnDefinitions.Clear();
                bool IsLoad = false;
                foreach (Home home in G.listHome)
                {
                    Color cl = Color.Gray;
                    if (!IsLoad)
                    {
                        cl = Color.FromHex("#f9d667"); IsLoad = true;
                    }
                    Button btn = new Button
                    {

                        Text = home.name,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        CornerRadius = 5,
                        FontSize = 22,
                        FontAttributes = FontAttributes.Bold,
                        BackgroundColor = Color.FromHex("#FAFAFA"),
                        TextColor = cl,
                    }; btnsHome.Add(btn);
                    btn.Clicked += Btn_Clicked1;
                    ColumnDefinition colDef = new ColumnDefinition();
                    colDef.Width = 150;
                    Grid.SetRow(btn, 0);
                    Grid.SetColumn(btn, col);
                    col++;
                    gridItem.ColumnDefinitions.Add(colDef);
                    gridItem.Children.Add(btn);




                }
                LoadIO();
            }
            catch (Exception)
            {

            }
        }
       List<SwipeItem> listBtnBoard=new List<SwipeItem>();
        List<SwipeItem> listBtnAlarm = new List<SwipeItem>();
        List<SwipeItem> listBtnBoardDelete = new List<SwipeItem>();
        List<Label> listlbLastTime = new List<Label>();
        List<Label> listlbBoard = new List<Label>();
        List<StackLayout> listStackBoard = new List<StackLayout>();
        List<Frame> listFrame = new List<Frame>();

        public async void LoadIO()
        {
            try
            {
                stackBoard.Children.Clear();
                listBtnAlarm = new List<SwipeItem>();
                listBtnGPIOs = new List<ListBtnGPIO>();
                listBtnBoard = new List<SwipeItem>();
                listBtnBoardDelete = new List<SwipeItem>();
                listlbLastTime = new List<Label>();
                listlbBoard = new List<Label>();
                listFrame = new List<Frame>();
                G.listBoardRoom = "";
                G.listNameBoard = new List<string>();
                foreach (Board board in G.listHome[G.indexHome].board)
                {
                    G.listBoardRoom += board.name+"_";
                    G.listNameBoard.Add(board.name);
                    String typeBoard = board.type;
                    ImageButton lb = new ImageButton
                    {
                        Margin = new Thickness(10,0,0, 0),
                      Source= typeBoard,
                      HorizontalOptions = LayoutOptions.Start,
                      VerticalOptions = LayoutOptions.Center,
                      BackgroundColor= Color.FromHex("#f2f2f2"),
                  };

                  /*  Label lb = new Label
                    {

                        Text = "Thiết bị" + typeBoard,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 12,
                        FontAttributes = FontAttributes.None,
                        TextColor = Color.Black,
                        Margin=new Thickness(10,0,0,0)

                    };*/
                    Label lbBoard = new Label
                    {
                        Margin=5,
                        Text = board.name,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.Black,

                    };
                    listlbBoard.Add(lbBoard);
                    Label lbTimer = new Label
                    {
                        Margin = 5,
                        Text = board.lastTime.ToString("HH:mm"),
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment=TextAlignment.Center,
                        VerticalTextAlignment=TextAlignment.Center,
                        FontSize = 10,
                        FontAttributes = FontAttributes.Italic,
                        TextColor = Color.LightGray,

                    };
                    listlbLastTime.Add(lbTimer);
                    StackLayout stk = new StackLayout()
                    {

                        BackgroundColor = Color.FromHex("#f2f2f2"),
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Orientation = StackOrientation.Horizontal,
                       
                    };
                 
                    stk.Children.Add(lb);
                    stk.Children.Add(lbBoard);
                    stk.Children.Add(lbTimer);
                  
                    StackLayout stack = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Orientation = StackOrientation.Vertical,
                       
                    };
                  

                    // SwipeItems
                    SwipeItem settingSwipeItem = new SwipeItem
                    {

                     
                     
                        IconImageSource = "setting.png",

                        BackgroundColor = Color.FromHex("#6e6e6e"),
                       //Text="Cài đặt"
                        

                    };

                    SwipeItem deleteSwipeItem = new SwipeItem
                    {

                      
                        IconImageSource = "clear.png",
                        BackgroundColor = Color.OrangeRed,
                        //Text = "Xóa",
                    };
                    SwipeItem alarmSwipeItem = new SwipeItem
                    {
                     
                      IconImageSource = "alarm.png",
                        BackgroundColor = Color.Goldenrod,
                        
                         // Text = "Hẹn giờ"
                    };

                    deleteSwipeItem.Invoked += BtnDeleteBoard_Clicked;
                    settingSwipeItem.Invoked += BtnBoard_Clicked;
                    alarmSwipeItem.Invoked += AlarmSwipeItem_Invoked;
                    listBtnBoardDelete.Add(deleteSwipeItem);
                    listBtnBoard.Add(settingSwipeItem);
                    listBtnAlarm.Add(alarmSwipeItem);
                    List<SwipeItem> swipeItems = new List<SwipeItem>() { settingSwipeItem, deleteSwipeItem };
                    List<SwipeItem> swipeItems2 = new List<SwipeItem>() { alarmSwipeItem };
                    SwipeView swipeView = new SwipeView
                    {
                        
                        LeftItems = new SwipeItems(new SwipeItems(swipeItems2)
                        {

                            
                            SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.RemainOpen

                        }),
                        RightItems = new SwipeItems(new SwipeItems(swipeItems)
                        {


                            SwipeBehaviorOnInvoked = SwipeBehaviorOnInvoked.RemainOpen

                        }),
                        
                        Content = stk,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(0,5,0,-5),
                        HeightRequest=40

                    };

                    stack.Children.Add(swipeView);
                    stackBoard.Children.Add(stack);
                    listStackBoard.Add(stack);
                    Grid girdGPIO = new Grid()
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Margin = new Thickness(0, -5),
                    };
                  
                   
                    RowDefinition row = new RowDefinition();
                    row.Height = 80;
                    girdGPIO.RowDefinitions.Add(row);
                   
                    int col2 = 0;
                    List<GPIO> gpios = new List<GPIO>();
                    var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardStates&nameBoard=" + board.name + "&users=" + G.User);
                    responseString = await response.Content.ReadAsStringAsync();

                    responseString = responseString.Replace("\"", "");
                    responseString = responseString.Replace("{", "");
                    responseString = responseString.Replace("}", "");
                    String[] S4 = responseString.Split(',');

                    foreach (String s2 in S4)
                    {
                        if (s2 == "") continue;
                        String[] s3 = s2.Split(':');
                        if (s3.Count() < 2) continue;
                        gpios.Add(new GPIO(s3[0], Convert.ToInt32(s3[1])));
                    }
                    response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getTypeBoard&nameBoard=" + board.name + "&users=" + G.User);
                    responseString = await response.Content.ReadAsStringAsync();

                    responseString = responseString.Replace("\"", "");
                    responseString = responseString.Replace("{", "");
                    responseString = responseString.Replace("}", "");
                    String[] S5 = responseString.Split(',');
                    foreach (String s2 in S5)
                    {
                        if (s2 == "") continue;
                        String[] s3 = s2.Split(':');
                        if (s3.Count() < 2) continue;
                        string name = s3[0];
                        string type = s3[1];
                        gpios[gpios.FindIndex(a => a.name == name)].type = type;

                    }
                    board.GPIOs = gpios;
                  

                   
                    foreach (GPIO gpio in board.GPIOs)
                    {
                        String type = gpio.type;
                        Color cl = Color.FromRgb(220, 220, 220) ;
                        if (type.Contains("key"))
                        {
                            cl = Color.FromHex("#f9d667");
                            type += gpio.state;
                        }
                        else
                        {
                            if (gpio.state == 1)
                                cl = Color.FromHex("#f9d667");
                            else if (gpio.state == 2)
                                cl = Color.Cornsilk;
                        }
                        
                        if (gpio.type.Trim()== "none")continue;
                       
                            ImageButton imgBtn = new ImageButton
                        {
                            CornerRadius = 5,
                           
                            HeightRequest = 60,
                            Source = type,
                            
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            BackgroundColor = cl
                        };
                        imgBtn.Clicked += BtnGPIO_Clicked;
                        Label Label = new Label
                        {
                            
                            Text = gpio.name,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold,

                            TextColor = Color.Black,

                        };
                        StackLayout stk2 = new StackLayout()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            Orientation = StackOrientation.Vertical,

                        };
                        stk2.Children.Add(imgBtn);
                        stk2.Children.Add(Label);
                        listBtnGPIOs.Add(new ListBtnGPIO(board.name, gpio.name, imgBtn));

                        Grid.SetRow(stk2, 0);
                        Grid.SetColumn(stk2, col2);
                      
                        col2++;

                        girdGPIO.Children.Add(stk2);
                        
                    }
                    
                     CustomFrame frame = new CustomFrame()
                    {
                        CornerRadius = new CornerRadius(0,0,5,5),
                        HasShadow = true,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                      Margin=new Thickness(1,0),
                        Content = girdGPIO,
                      
                    };
                
                    listFrame.Add(frame);
                    stack.Children.Add(frame);
                }
                LoadOnLine();
            }
            catch(Exception ex) { }
        }
        Alarm Alarm;
        private async void AlarmSwipeItem_Invoked(object sender, EventArgs e)
        {
            G.IsInternet = CheckNet();
            if (!G.IsInternet) return;
            SwipeItem button = (SwipeItem)sender;
            int index = listBtnAlarm.FindIndex(a => a == button);
            G.boardSelect = G.listHome[G.indexHome].board[index];
            if (Alarm == null) Alarm = new Alarm();
            await PopupNavigation.PushAsync(Alarm);
        }

        private async void BtnDeleteBoard_Clicked(object sender, EventArgs e)
        {
            SwipeItem button = (SwipeItem)sender;
            int index = listBtnBoardDelete.FindIndex(a => a == button);
            G.boardSelect = G.listHome[G.indexHome].board[index];
           
            String listboard = "";
           
            var choice = await DisplayAlert("Xóa", "Bạn muốn xóa Thiết bị  " + G.boardSelect.name+ " ?", "YES", "NO");
            if (choice)
            {
                var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=deleteBoard&home=" + G.nameRoom + "&users=" + G.User+ "&nameBoard=" + G.boardSelect.name);

                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Length > 0)
                {
                    DisplayAlert("Delete", "Xóa thiết bị thành công", "OK");

                    G.history.Load();
                }
                else
                {
                    DisplayAlert("Delete", "Xóa  thiết bị thất bại", "OK");
                }
            }
        }

        Edits Edits;
        private async void BtnBoard_Clicked(object sender, EventArgs e)
        {
            G.IsInternet = CheckNet();
            if (!G.IsInternet) return;
            SwipeItem button = (SwipeItem)sender;
         int index=   listBtnBoard.FindIndex(a=>a==button);
            G.boardSelect = G.listHome[G.indexHome].board[index];
            if (Edits == null) Edits = new Edits();
            await PopupNavigation.PushAsync(Edits);
        }

        private void Btn_Clicked1(object sender, EventArgs e)
        {
            try
            {
                foreach (Button btn in btnsHome)
                {
                    btn.TextColor = Color.Gray;
                    // btn.BackgroundColor = Color.WhiteSmoke;
                }
                Button button = (Button)sender;
                G.nameRoom = button.Text.Trim();
               
                G.indexHome = G.listHome.FindIndex(a => a.name.Contains(button.Text.Trim()));
                //  button.TextColor = Color.Black;
                button.TextColor = Color.FromHex("#f9d667");
                if(G.indexHome>-1)
                LoadIO();
            }
            catch(Exception ex)
            {

            }
        }
        bool IsBlink = false;
        String sSendBlink; int indexGPIO; int indexBoard; ImageButton btnGPIO;
        bool IsStopDoor = false;
        GPIO gpioDoor;
        private async void PressGPIO(String nameBoard,GPIO gpio)
        {
            try
            {
                if (!G.listHome[G.indexHome].board[indexBoard].IsOnline) return;
                int state = gpio.state;
                string type = gpio.type;

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
                string s = "https://giacongpcb.vn/esp-outputs-action.php?action=output_status2&name=" + gpio.name + "&board=" + nameBoard + "&users=" + G.User + "&state=" + state;
                var response = await client.GetAsync(s);


                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString != "null")
                {
                    if (type.Trim().Contains("key"))
                    {
                        btnGPIO.Source = "key" + state;
                    }
                    else
                    {

                        if (state == 1)
                        {
                            btnGPIO.BackgroundColor = Color.FromHex("#f9d667");
                        }
                        else
                        {
                            btnGPIO.BackgroundColor = Color.FromRgb(220, 220, 220);
                        }
                    }

                    G.listHome[G.indexHome].board[indexBoard].GPIOs[indexGPIO].state = state;

                    if (type.Trim().Contains("door") || type.Trim().Contains("click"))
                    {
                        sSendBlink = "https://giacongpcb.vn/esp-outputs-action.php?action=output_status2&name=" + gpio.name + "&board=" + nameBoard + "&users=" + G.User + "&state=" + 0;

                        Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
                        {

                            if (IsSend)
                            {
                                Blink(); IsSend = false;
                                return false;
                            }

                            IsSend = true;
                            return true; // True = Repeat again, False = Stop the timer
                        });
                    }

                    else if (type.Trim().Contains("nc"))
                    {
                        sSendBlink = "https://giacongpcb.vn/esp-outputs-action.php?action=output_status2&name=" + gpio.name + "&board=" + nameBoard + "&users=" + G.User + "&state=" + 1;

                        Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
                        {

                            if (IsSend)
                            {
                                NC(); IsSend = false;
                                return false;
                            }

                            IsSend = true;



                            return true; // True = Repeat again, False = Stop the timer
                        });
                    }


                }
                if (IsStopDoor)
                    IsStopDoor = false;
            }
            catch(Exception ex)
            {
               
            }
        }
        String nameBoard; GPIO gpio;
        private async void BtnGPIO_Clicked(object sender, EventArgs e)
        {
           
            btnGPIO = (ImageButton)sender;
            int index = listBtnGPIOs.FindIndex(a => a.btnGPIO == btnGPIO);
            nameBoard = listBtnGPIOs[index].nameBoard;
            List<Board> boards = G.listHome[G.indexHome].board;
            indexBoard = boards.FindIndex(a => a.name.Contains(nameBoard));
            List<GPIO> gpios = G.listHome[G.indexHome].board[indexBoard].GPIOs;
            indexGPIO = gpios.FindIndex(a => a.name.Contains(listBtnGPIOs[index].GPIO));
            gpio = gpios[indexGPIO];
            G.IsInternet = CheckNet();
            if (!G.IsInternet)
            {

              Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    int n = 0;
                    Y: G.IsInternet = CheckNet();
                    if (G.IsInternet)
                    {
                        IsPressed = true; tmOut.Enabled = true;
                        PressGPIO(nameBoard, gpio);
                    }
                       
                    else 
                    {
                        DisplayAlert("Lỗi Kết nối mạng", "Kiểm tra dữ liệu mạng", "OK");
                    }
                    return false;
                });
               
                return;
            }
          
           else
            {
                IsPressed = true; tmOut.Enabled = true;
                PressGPIO(nameBoard, gpio);
            }    
           

        }
        List<String> listmacAddress = new List<string>();
        string macAddress = "";
        public async void LoadStatus()
        {
            if(IsPressed) return;
            try
            {
                listmacAddress = new List<string>();
                //var macAddress = NetworkInterface.GetAllNetworkInterfaces();
               
                foreach (Board board in G.listHome[G.indexHome].board)
                
                {
                    if (!board.IsOnline) continue;
              
                    var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardStates&nameBoard=" + board.name + "&users=" + G.User);
                    responseString = await response.Content.ReadAsStringAsync();

                    responseString = responseString.Replace("\"", "");
                    responseString = responseString.Replace("{", "");
                    responseString = responseString.Replace("}", "");
                    String[] S4 = responseString.Split(',');
                    foreach (String s2 in S4)
                    {
                        if (s2 == "") continue;
                        String[] s3 = s2.Split(':');
                        if (s3.Count() < 2) continue;
                   int indexGPIO= board.GPIOs.FindIndex(a=>a.name.Contains(s3[0]));
                    if(indexGPIO>-1)
                        {
                            int state = Convert.ToInt32(s3[1]);
                         if(board.GPIOs[indexGPIO].state!= state)
                            {
                                board.GPIOs[indexGPIO].state = state;
                                int index = listBtnGPIOs.FindIndex(a => a.nameBoard == board.name && a.GPIO == board.GPIOs[indexGPIO].name);
                                if (index > -1)
                                {

                                    if (board.GPIOs[indexGPIO].type.Contains("key"))
                                    {
                                        listBtnGPIOs[index].btnGPIO.Source = "key" + state;
                                    }
                                    else
                                    {
                                        switch (state)
                                        {

                                            case 0:
                                                listBtnGPIOs[index].btnGPIO.BackgroundColor = Color.FromRgb(220, 220, 220);
                                                break;
                                            case 1:
                                                listBtnGPIOs[index].btnGPIO.BackgroundColor = Color.FromHex("#f9d667");
                                                break;
                                        }
                                    }
                                }
                                   
                                }
                            
                        }    
                     
                    }
                   
                }
            }
            catch (Exception ex)
            {
             //   DisplayAlert("err2", ex.Message, "OK");
            }
        }

        DataTable dtPropety;
        List<Button> listBtnJog = new List<Button>();
        List<Button> listBtnSup = new List<Button>();
        List<Button> listBtnSiz = new List<Button>();


        String Jog, Sup, Siz;
      
        private void BtnAll_Clicked(object sender, EventArgs e)
        {
            //
        }



     

        DataTable dtQty;

        int indexWoker = 0;


        Timer tmLoad = new Timer();
        Pass Pass = new Pass();
    
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            G.IsInternet = CheckNet();
            if (G.IsInternet)
            {
              
                if (!IsLoad)
                {
                    G.User = Preferences.Get("User", "");
                    G.Pass = Preferences.Get("Pas", "");
                    if (G.User.Trim() == "")
                    {
                        layMain.IsVisible = false;
                        if (Pass == null) Pass = new Pass();
                        await PopupNavigation.PushAsync(Pass);
                    }
                    else
                    {
                        var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=login&name=" + G.User + "&pass=" + G.Pass);

                        var responseString = await response.Content.ReadAsStringAsync();
                        if (responseString != "null")
                        {

                            layMain.IsVisible = true;
                            lbName.Text = G.User;

                            G.IsLogin = true;
                            Load();
                            IsLoad = true;
                        }
                        else
                        {
                            layMain.IsVisible = false;
                            if (Pass == null) Pass = new Pass();
                            await PopupNavigation.PushAsync(Pass);
                        }
                    }
                }
            }
            else
            {
                DisplayAlert("Lỗi kết nối mạng ", "Vui lòng kiểm tra kết nối mạng", "OK");
                DependencyService.Get<INativeHelper>().CloseApp();

                INativeHelper nativeHelper = null;
                nativeHelper = DependencyService.Get<INativeHelper>();
                if (nativeHelper != null)
                {
                    nativeHelper.CloseApp();
                }


            }
        }
    
          
            
           
           
            

        
    
        List<ImageButton> listUser2 = new List<ImageButton>();
        String sWoker = "";
        bool IsChieu;
        int MaxChoose = 0,NumChoose=0;
   

     
     
        Timer tmStock =  new Timer();
        bool IsStock = false;
 
  
        private void Button_Clicked(object sender, EventArgs e)
        {

        }
        bool IsSendNow = true;
      
      
    
        int slLabel = 1;
   
  
    
       
        DateTime dateSelect=DateTime.Now;
        NewRoom NewRoom;
        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (NewRoom == null) NewRoom = new NewRoom();
            await PopupNavigation.PushAsync(NewRoom);
        }
        AddBoard AddBoard;
        Ping ping;
    
        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            /* try {
                 Ping ping = new Ping();
                 PingReply reply = ping.Send("192.168.1.12", 1000);

               //  return reply != null && reply.Status == IPStatus.Success;


                 DisplayAlert("Thiết bị mới", macAddress, "OK");
             }
             catch (Exception ex)
             {
                 DisplayAlert("err", ex.Message, "OK");
             }*/


            InativeWifi nativeHelper = null;
            nativeHelper = DependencyService.Get<InativeWifi>();
            if (nativeHelper != null)
            {

              bool IsConnect=  await nativeHelper.ConnectToWifi("Bee5G","beeau$beeau");
                if(IsConnect)
                DisplayAlert("Wifi", "Đã kết nối thành công!", "OK");

            }
            if (AddBoard == null)
                AddBoard = new AddBoard();
            await PopupNavigation.PushAsync(AddBoard);
            /* var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=getBoardNone");


             G.sBoardNone = await response.Content.ReadAsStringAsync();
             if (G.sBoardNone != "null")
             {

                 if (AddBoard == null) AddBoard = new AddBoard();
                 await PopupNavigation.PushAsync(AddBoard);
             }
             else
             {
                 DisplayAlert("Thiết bị mới", "Chưa có thiết bị mới vui lòng kết nối Wifi cho thiết bị", "OK");
             }   */

        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            G.IsInternet = CheckNet();
            if (!G.IsInternet) return;
            var choice = await DisplayAlert("LogOut", "Bạn muốn tất cả dữ liệu " + G.nameRoom + " ?", "YES", "NO");
            if (choice)
            {
                try
                {
                    var response = await client.GetAsync("https://giacongpcb.vn/esp-outputs-action.php?action=deleteRoom&home=" + G.nameRoom + "&users=" + G.User);

                    var responseString = await response.Content.ReadAsStringAsync();
                    if (responseString.Length > 0)
                    {
                        DisplayAlert("Delete", "Xóa nhà thành công", "OK");

                        G.history.LoadItem();
                    }
                    else
                    {
                        DisplayAlert("Delete", "Xóa nhà thất bại", "OK");
                    }

                }
                catch (Exception ex)
                {
                }

            }
        }

        private void btnAvatar_Clicked(object sender, EventArgs e)
        {

        }

    
    

        private async void resh1_Refreshing_1(object sender, EventArgs e)
        {
            //resh1.IsRefreshing = true;
           /// Connect(20000);
            //LoadProcess(10000);
        }

      
      

        private async void btnLogOut_Clicked(object sender, EventArgs e)
        {
            var choice = await DisplayAlert("LogOut", "Bạn muốn đăng xuất ?", "YES", "NO");
            if (choice)
            {
                G.IsLogin = false;
                Preferences.Set("User", "");
                Preferences.Set("Pas", "");
                layMain.IsVisible = false;
                if (Pass == null) Pass = new Pass();
                await PopupNavigation.PushAsync(Pass);
            }
        }

   

     
        
    
    }
    }