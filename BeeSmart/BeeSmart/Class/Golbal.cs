

using BeeSmart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CodeShare.Library.Models.MetaData;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Data;
using BeeSmart.Views;
using BeeFactory;
using SFS_HPT.Class;

namespace BeeSmart
{
    enum Level
    {
        Admin,
        User,
        Leader
    }
    public struct G
    {
        public static string sBoardNone;
        public static List<string> listNameBoard = new List<string>();
        public static string nameRoom="",listBoardRoom="";
       public static int indexHome=0;
        public static Board boardSelect;
        public static Board BoardNone;
        public static List< Board> listBoard=new List<Board>();
        public static List<Home> listHome = new List<Home>();
        public static bool IsLogin = false,IsConnectAgain;
        public static String User, Pass, Level;
      public static List<Processing> listProcess = new List<Processing>();
        public static History history=new History();
        public static List<String> listID = new List<string>();
        public static List<String> listName = new List<String>();
        public static List<String> listNameAll = new List<String>();
        public static DateTime dtNow=DateTime.Now;
    
        public static String pathCon,ID="";
        public static double SumBank, Timer, Money;
       public static DataTable dtFund, dt, dtHis;
        public static int numClickLock = 0,numDoorUp=0;
       public static Timer tmUpdate ;
       
        public static bool isLoadAgain, isClient = false, IsLock = false, IsUnLockScreen, IsDeviceLocked, IsDisConnectWifi=false, IsConnectWifi, IsClose, IsClose2,IsSend=false;
        public static bool IsBackground = false;
        public static String txtOld = "",txtOffLine;
        public static ImageButton imgLink;
        public static BackgroundWorker worker = new BackgroundWorker();
        public static List<MetaInformation> metaInformations = new List<MetaInformation>();
        public static List<ImageSource> listLight = new List<ImageSource> { "LightOn.gif", "LightOff.png" };
        public static bool IsInternet = true,IsSleep;
        public static HomeModel HomeModel=new HomeModel();
        public static double lati = 10.91170405163229, longi = 106.765421366079;
        public static String id;
        public static double distances;
    public static String sLocaion= "10.91170405163229,106.765421366079", ImageSouse= "LightOff.png";
        public static String nameCus, namePJ, nameFile;
        public static bool IsDowload = false,IsStatus=false;
        public static String pathView;
        public static string pathPro,Ip;
        public static bool IsClick = false;
        public static Label lbLat, lbLon,lbDis;
         public static Image btnLock, btnLockDown , btnPauseUp, btnPauseDown;
        public static bool IsUpDoor = false, IsDownDoor=false;
        public static System.Timers.Timer tmStatus = new System.Timers.Timer();
        public static System.Timers.Timer tmRun = new System.Timers.Timer(),tmRefresh=new System.Timers.Timer();
        public static int St1 = 0, St2 = 0, St3 = 0, St4 = 0,
             St11 = 0, St21 = 0, St31 = 0, St41 = 0, numError = 0;
    }
    class Golbal
    {
    }
}