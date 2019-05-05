using Microsoft.Win32;
using System.Windows;

namespace IntralismSharedEditor
{
    /// <summary>
    /// Работа с данными реестра
    /// </summary>
    public static class ConfigReg
    {
        /// <summary>
        /// Create key
        /// </summary>
        public static RegistryKey SubDir = Registry.CurrentUser.CreateSubKey("Texaditor");
        /// <summary>
        /// Returns default settings
        /// </summary>
        public static void DeleteAll()
        {
            if (MessageBox.Show("REMOVING ALL SETTINGS. ARE YOU SURE?", "WARNING!",MessageBoxButton.YesNo,MessageBoxImage.Exclamation,MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                //Registry.CurrentUser.DeleteSubKey("Texaditor");
                DirDefault = "";
                DirCreate = "";
                DirLoad = "";
                DirContinue = "";
                DirResource = "";
                Theme = "";
                LastEvent = "";
                Version = "";
                FPS = 0;
                CaretMargin = 0;
                Zoom = 0;
                ScrollSeconds = 0;
                ScrollMills = 0;
                TimingLength = 0;
                TimingTimes = 0;
                NearTime = 0;
                Mode = 0;
                FullScreen = 0;
                IsKawaii = 0;
                Q = "";
                SubDir.DeleteValue("DirDefault");
                SubDir.DeleteValue("DirCreate");
                SubDir.DeleteValue("DirLoad");
                SubDir.DeleteValue("DirContinue");
                SubDir.DeleteValue("DirResource");
                SubDir.DeleteValue("Theme");
                SubDir.DeleteValue("LastEvent");
                SubDir.DeleteValue("Version");
                SubDir.DeleteValue("FPS");
                SubDir.DeleteValue("CaretMargin");
                SubDir.DeleteValue("Zoom");
                SubDir.DeleteValue("ScrollSeconds");
                SubDir.DeleteValue("ScrollMills");
                SubDir.DeleteValue("TimingLength");
                SubDir.DeleteValue("TimingTimes");
                SubDir.DeleteValue("NearTime");
                SubDir.DeleteValue("Mode");
                SubDir.DeleteValue("FullScreen");
                SubDir.DeleteValue("IsKawaii");
                SubDir.DeleteValue("Q");
            }
        }
        //==============================================================================================================================
        //STRING
        /// <summary>
        /// Save map folder
        /// </summary>
        public static string DirDefault
        {
            get { return (string)SubDir.GetValue("DirDefault", null); }
            set { SubDir.SetValue("DirDefault", value); }
        }
        /// <summary>
        /// Initial directory when creating new map
        /// </summary>
        public static string DirCreate
        {
            get { return (string)SubDir.GetValue("DirCreate", null); }
            set { SubDir.SetValue("DirCreate", value); }
        }
        /// <summary>
        /// Initial directory when loading new map
        /// </summary>
        public static string DirLoad
        {
            get { return (string)SubDir.GetValue("DirLoad", null); }
            set { SubDir.SetValue("DirLoad", value); }
        }
        /// <summary>
        /// Initial directory when continue last map
        /// </summary>
        public static string DirContinue
        {
            get { return (string)SubDir.GetValue("DirContinue", null); }
            set { SubDir.SetValue("DirContinue", value); }
        }
        /// <summary>
        /// Initial directory when adding the resources
        /// </summary>
        public static string DirResource
        {
            get { return (string)SubDir.GetValue("DirResource", null); }
            set { SubDir.SetValue("DirResource", value); }
        }
        /// <summary>
        /// Choosed theme
        /// </summary>
        public static string Theme
        {
            get
            {
                return (string)SubDir.GetValue("Theme", "Default");
            }
            set
            {
                SubDir.SetValue("Theme", value);
            }
        }
        /// <summary>
        /// Last spawned event
        /// </summary>
        public static string LastEvent
        {
            get { return (string)SubDir.GetValue("LastEvent", "SetBGColor"); }
            set { SubDir.SetValue("LastEvent", value); }
        }
        /// <summary>
        /// Version of application
        /// </summary>
        public static string Version
        {
            get { return (string)SubDir.GetValue("Version", "0"); }
            set { SubDir.SetValue("Version", value); }
        }
        //==============================================================================================================================
        //INT
        /// <summary>
        /// 1 / FPS
        /// </summary>
        public static int FPS
        {
            get { return (int)SubDir.GetValue("FPS", 10); }
            set { SubDir.SetValue("FPS", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int CaretMargin
        {
            get { return (int)SubDir.GetValue("CaretMargin", 400); }
            set { SubDir.SetValue("CaretMargin", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int Zoom
        {
            get { return (int)SubDir.GetValue("Zoom", 20); }
            set { SubDir.SetValue("Zoom", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int ScrollSeconds
        {
            get { return (int)SubDir.GetValue("ScrollSeconds", 9); }
            set { SubDir.SetValue("ScrollSeconds", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int ScrollMills
        {
            get { return (int)SubDir.GetValue("ScrollMills", 300); }
            set { SubDir.SetValue("ScrollMills", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int TimingLength
        {
            get { return (int)SubDir.GetValue("TimingLength", 4); }
            set { SubDir.SetValue("TimingLength", value); }
        }
        /// <summary>
        /// Extra for interface
        /// </summary>
        public static int TimingTimes
        {
            get { return (int)SubDir.GetValue("TimingTimes", 20); }
            set { SubDir.SetValue("TimingTimes", value); }
        }
        //==============================================================================================================================
        //DOUBLE
        /// <summary>
        /// (ms) when events are really near
        /// </summary>
        public static double NearTime
        {
            get { return (double)SubDir.GetValue("NearTime", 50.0); }
            set { SubDir.SetValue("NearTime", value); }
        }
        //==============================================================================================================================
        //BOOL
        /// <summary>
        /// Debug mode
        /// </summary>
        public static int Mode
        {
            get { return (int)SubDir.GetValue("Mode", 0); }
            set { SubDir.SetValue("Mode", value); }
        }
        /// <summary>
        /// Full screen mode
        /// </summary>
        public static int FullScreen
        {
            get { return (int)SubDir.GetValue("FullScreen", 0); }
            set { SubDir.SetValue("FullScreen", value); }
        }
        /// <summary>
        /// Idk what is it
        /// </summary>
        public static int IsKawaii
        {
            get { return (int)SubDir.GetValue("IsKawaii", 0); }
            set { SubDir.SetValue("IsKawaii", value); }
        }
        //==============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        public static string Q
        {
            get { return (string)SubDir.GetValue("Q", "default"); }
            set { SubDir.SetValue("Q", value); }
        }
    }
}
