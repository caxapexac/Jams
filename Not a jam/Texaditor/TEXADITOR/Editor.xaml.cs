using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;
using OggVorbisEncoder;
using IntralismSharedEditor;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using TEXADITOR.elements;
//using Alvas.Audio;

namespace TEXADITOR
{
    //=========================================================================================================//
    //===                                              TEXADITOR                                            ===//
    //===  INFO:                                                                                            ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //===                                                                                                   ===//
    //=========================================================================================================//

    public partial class Window1 : Window
    {
        #region Fields
        #region System
        private VorbisWaveReader OggReader; //класс музыки
        public MapFileSystem MapFiles; // вспомогательный класс
        private DirectSoundOut AudioOut = new DirectSoundOut(); //класс проигрывателя
        private System.Windows.Threading.DispatcherTimer timer; //класс таймера
        private System.Windows.Threading.DispatcherTimer UItimer; //класс таймера
        private Rectangle SelRect = new Rectangle(); //Selecting rectangle
        public UndoClass UndoRedo = new UndoClass(); // вспомогательный класс
        public MapData EditorData = new MapData(); //основной класс эдитора

        private int FastSaveTick = 0; //счетчик
        private int TimingTick = 0; //счетчик
        private int UpdateConfigTick = 0; //счетчик
        private int InfoTick = 0; //счетчик
        #endregion
        #region Essentials
        List<Event> Events = new List<Event>(); //List of all the events
        List<double> Times = new List<double>(); // ONLY for setting timing

        private Event TimingEvent;
        private Event LastEventLeft;
        private Event LastEventRight;
        private Event Nearest;
        private Event MovedEvent;
        private Event EditedEvent;
        private Point StartPos;
        private Point Pos;
        private Point StartMovePosition;

        private bool IsFocusEventWindow = false;
        private bool IsGamepalyBlocked = false;
        private bool IsStoryboadBlocked = false;
        private bool IsMappingBlocked = false;
        private bool IsLeftBlocked = false;//?
        private bool IsRightBlocked = false;//?
        private bool CreatingTiming = false;
        private bool ChangingTiming = false;
        private bool PreparingToCopy = false;
        private bool PreparingToPlay = false;
        private bool PreparingToUpdateConfig = false;
        private bool Selecting = false;
        private bool Scrolling = false;
        private bool IsChangedTiming = false;
        private bool IsEventMovIng = false;
        private bool IsEventMovEd = false;
        private bool MagnetOnSpawn = true;
        private bool MagnetOnTiming = true;
        private bool IsMuted = true;
        private double StartEventPosition;
        private double Dx = 0;
        private double? RepeatStart = null;
        private double? RepeatStop = null;
        private int Zoom = 1;
        private int CaretMargin = 1;
        #endregion
        #region Enums
        private Dictionary<string, SolidColorBrush> brushes = new Dictionary<string, SolidColorBrush>()
        {
            ["default"] = new SolidColorBrush(Color.FromArgb(150, 0, 150, 0)),
            ["left"] = new SolidColorBrush(Color.FromArgb(150, 200, 150, 0)),
            ["right"] = new SolidColorBrush(Color.FromArgb(150, 0, 150, 200)),
            ["selected"] = new SolidColorBrush(Color.FromArgb(150, 200, 0, 0)),
            ["leftselected"] = new SolidColorBrush(Color.FromArgb(200, 200, 250, 0)),
            ["rightselected"] = new SolidColorBrush(Color.FromArgb(200, 0, 250, 200)),
            ["bigtiming"] = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)),
            ["mediumtiming"] = new SolidColorBrush(Color.FromArgb(200, 70, 70, 0)),
            ["smalltiming"] = new SolidColorBrush(Color.FromArgb(200, 70, 0, 70)),
            ["error"] = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
        };
        #endregion
        #region Fuck my brain what is it?
        List<Event> Timings = new List<Event>(); //WIP
        private bool FreeKeyW = true; //К
        private bool FreeKeyS = true; //О
        private bool FreeKeyA = true; //С
        private bool FreeKeyD = true; //Т
        private bool FreeKeyQ = true; //Ы
        private bool FreeKeyE = true; //Л
        private bool SlowMoTick = false;//Ь

        private bool SelectedChangedLeft = false;
        private bool SelectedChangedRight = false;

        private int ModeEvent = 0; //режим редактирования ничего=3/арок=2/сториборда=1/всего=0
        #endregion
        #endregion
        #region Стартовые методы
        public Window1(string arg, string path)
        {
            InitializeComponent();
            Application.Current.Exit += ShutUp;
            Application.Current.SessionEnding += ComputerDown;
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            TexaditorWindow.Visibility = Visibility.Hidden;
            ChangeTheme();
            ChangeFullScreen();
            StartFileDialog(arg, path);
            LoadEvents();
            LoadResources();
            LoadMusic();
            LoadOnce();
            TexaditorWindow.Visibility = Visibility.Visible;
        } // Constructor =+= 1 CALL
        private void ChangeTheme()
        {
            var uri = new Uri(@"themes\" + ConfigReg.Theme + ".xaml", UriKind.Relative);
            ResourceDictionary resourseDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourseDict);
        } // Changes theme =+= 1 CALL
        private void ChangeFullScreen()
        {
            if (ConfigReg.FullScreen == 1)
            {
                TexaditorWindow.AllowsTransparency = true;
                TexaditorWindow.WindowStyle = WindowStyle.None;
                TexaditorWindow.WindowState = WindowState.Maximized;
                TexaditorWindow.Left = 0;
                TexaditorWindow.Top = 0;
                TexaditorWindow.Width = SystemParameters.VirtualScreenWidth;
                TexaditorWindow.Height = SystemParameters.VirtualScreenHeight;
                TexaditorWindow.ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                ExitButton.Visibility = Visibility.Collapsed;
                TexaditorWindow.AllowsTransparency = false;
                TexaditorWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
            }
        } // Changes fullscreen mode =+= 1 CALL
        private void StartFileDialog(string type, string path)
        {
            switch (type)
            {
                case "Create":
                    MapFiles = new MapFileSystem(path);
                    ConfigReg.DirCreate = path.Substring(0, path.LastIndexOf("\\"));
                    break;
                case "Load":
                    string text = File.ReadAllText(path);
                    MapData md = CustomEditor.GetMap(text);
                    MapFiles = new MapFileSystem(md.musicFile, path, md.levelResources, md.iconFile);
                    ConfigReg.DirLoad = MapFiles.MapDir;
                    ConfigReg.DirContinue = MapFiles.MapDir;
                    break;
                case "Continue":
                    string text2 = File.ReadAllText(path);
                    MapData md2 = CustomEditor.GetMap(text2);
                    MapFiles = new MapFileSystem(md2.musicFile, path, md2.levelResources, md2.iconFile);
                    break;
            }
        } // Choosing the map =+= 1 CALL
        private void LoadResources()
        {
            //Console.WriteLine("D");
            foreach (string name in Directory.GetFiles(MapFiles.ResDir))
            {
                string sname = name.Substring(name.LastIndexOf("\\") + 1); 
                List<MapResource> mr = EditorData.levelResources.FindAll(i => i.path == sname || i.path == "resources\\" + sname);
                if (mr != null && mr.Count != 0)
                {
                    if (mr.Count == 1)
                    {
                        if (mr[0].path.StartsWith("resources\\") == false)
                        {
                            mr[0].path = "resources\\" + sname;
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is more than one resource with the name " + name,"WARNING!",MessageBoxButton.OK,MessageBoxImage.Error,MessageBoxResult.OK);
                    }
                }
                else
                {
                    EditorData.levelResources.Add(new MapResource(sname.Substring(0,sname.IndexOf(".")), "Sprite", "resources\\" + sname, ""));
                }
            }
            List<MapResource> err = new List<MapResource>();
            foreach (MapResource name in EditorData.levelResources)
            {
                if (File.Exists(MapFiles.ResDir + "\\" + name.path))
                {
                    name.path = "resources\\" + name.path;
                    File.Copy(MapFiles.ResDir + "\\" + name.path, MapFiles.ResDir + "\\" + name.path, true);
                }
                else if (File.Exists(MapFiles.MapDir + "\\" + name.path) == false)
                {
                    err.Add(name);
                }
            }
            foreach (MapResource e in err) EditorData.levelResources.Remove(e);
        } // Loads the resources =+= 2 CALLS
        private void LoadMusic()
        {
            OggReader = new VorbisWaveReader(MapFiles.MusicPath);
            AudioOut = new DirectSoundOut();
            AudioOut.Init(OggReader);
            if (EditorData.musicTime != Math.Round(OggReader.TotalTime.TotalSeconds,6))
            {
                if (EditorData.musicTime != 0)
                {
                    MessageBox.Show("Wrong music file (lenghts dont equals):" + EditorData.musicTime + " != " + Math.Round(OggReader.TotalTime.TotalSeconds, 6), "WARNING", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                EditorData.musicTime = Math.Round(OggReader.TotalTime.TotalSeconds,6);
            }
            AllTime.Content = OggReader.TotalTime;
            PositionBar.Maximum = OggReader.TotalTime.TotalSeconds;
        } // Loads the music =+= 1 CALL
        private void LoadOnce()
        {
            AudioOut.Play();
            foreach (string tag in Helpers.tags)
            {
                TagsPanel.Children.Add(new CheckBox()
                {
                    Margin = new Thickness(10, 0, 0, 0),
                    Content = tag,
                    IsChecked = EditorData.tags.Contains(tag),
                    Focusable = false
                });
            } //Adds tag type from Helpers.tags
            StatusLabel.Content = "Map loaded in " + MapFiles.MapDir; //log
            LeftBorder.Child = new PanelConfig(EditorData.levelResources, MapFiles.MapDir);
            RightBorder.Child = new PanelConfig(EditorData.levelResources, MapFiles.MapDir);
            ThirdBorder.Child = new PanelConfig(EditorData.levelResources, MapFiles.MapDir) { HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top, Visibility = Visibility.Collapsed };
            ((PanelConfig)LeftBorder.Child).ButSetTiming.Click += ButSetTiming_Left;
            ((PanelConfig)RightBorder.Child).ButSetTiming.Click += ButSetTiming_Right;
            ((PanelConfig)ThirdBorder.Child).ButSetTiming.Click += ButSetTiming_Third;
            ((PanelConfig)LeftBorder.Child).ButRem.Click += ButRem_Left;
            ((PanelConfig)RightBorder.Child).ButRem.Click += ButRem_Right;
            ((PanelConfig)ThirdBorder.Child).ButRem.Click += ButRem_Third;
            for (int i = 0; i < ConfigReg.TimingTimes; i++)
            {
                OnlyPies.Children.Add(new Line()
                {
                    Height = 4000,
                    Width = 8,
                    X1 = 0,
                    X2 = 0,
                    Y1 = 0,
                    Y2 = 4000,
                    StrokeThickness = 2,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Fill = new SolidColorBrush(Colors.Black),
                    Stroke = new SolidColorBrush(Colors.Black)
                });
            }
            ChangeZoom();
            OggReader.CurrentTime = TimeSpan.FromSeconds(1);
            AudioOut.Pause();
            timer = new System.Windows.Threading.DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, ConfigReg.FPS) };
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
            UItimer = new System.Windows.Threading.DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 16) };
            UItimer.Tick += new EventHandler(UITimerTick);
            UItimer.Start();
            ChangeCaretMargin();
        } // System (launch once) =+= 1 CALL

       
        #endregion
        #region volume changing
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        private void Mute()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        private void VolDown()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }
        private void VolUp()
        {
            SendMessageW(new WindowInteropHelper(this).Handle, WM_APPCOMMAND, new WindowInteropHelper(this).Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }
        #endregion
        #region Visual Update
        private void UITimerTick(object sender, EventArgs e)
        {
            EventGrid.Margin = new Thickness(-OggReader.CurrentTime.TotalSeconds * Zoom,0,0,0);
        }
        private void Update()
        {
            UpdateRepeat();
            FastSave(1);
            UpdateBorders();
            UpdateUI();
            UpdateInfo(1);
        } // Calls every ~10ms
        private void UpdateRepeat()
        {
            if (RepeatStart != null && RepeatStop != null)
            {
                if (RepeatStart > RepeatStop)
                {
                    if (OggReader.CurrentTime.TotalSeconds > RepeatStart)
                    {
                        OggReader.CurrentTime = new TimeSpan(0, 0, (int)Math.Round((double)RepeatStop));
                    }
                }
                else
                {
                    if (OggReader.CurrentTime.TotalSeconds > RepeatStop)
                    {
                        OggReader.CurrentTime = new TimeSpan(0, 0, (int)Math.Round((double)RepeatStart));
                    }
                }
            }
        } // F5 feature =+= 1 CALL
        private void UpdateBorders()
        {
            double nowTime = OggReader.CurrentTime.TotalSeconds;
            bool changed = false;
            EventGrid.Width = OggReader.TotalTime.TotalSeconds * Zoom;
            if (LastEventLeft != null && (LastEventLeft.Time > nowTime + 0.001 || IsLeftBlocked))
            {
                ChangeColor(LastEventLeft, brushes["default"]);
                LastEventLeft = null;
                changed = true;
            }
            if (LastEventRight != null && (LastEventRight.Time <= nowTime - 0.001 || IsRightBlocked))
            {
                ChangeColor(LastEventRight, brushes["default"]);
                LastEventRight = null;
                changed = true;
            }
            foreach (Event one in Events.FindAll(i=>i.NeedRemove))
            {
                RemoveEvent(one);
            }
            foreach (Event oneEvent in Events)
            {
                if (CheckBorder(oneEvent))
                {
                    if (oneEvent.NeedUpdate) UpdateEvent(oneEvent);
                    if ((IsGamepalyBlocked == false && oneEvent.Type == "Gameplay") || (IsStoryboadBlocked == false && oneEvent.Type == "Storyboard") || (IsMappingBlocked == false && oneEvent.Type == "Mapping"))
                    {
                        if (oneEvent.Time > nowTime + 0.001)
                        {
                            if (IsRightBlocked == false && (LastEventRight == null || oneEvent.Time < LastEventRight.Time))
                            {
                                changed = true;
                                ChangeColor(LastEventRight, brushes["default"]);
                                LastEventRight = oneEvent;
                            }
                        }
                        else
                        {
                            if (IsLeftBlocked == false && (LastEventLeft == null || oneEvent.Time > LastEventLeft.Time))
                            {
                                changed = true;
                                ChangeColor(LastEventLeft, brushes["default"]);
                                LastEventLeft = oneEvent;
                            }
                        }
                    }
                }
            }
            if (changed)
            {
                if(IsMuted == false)
                {
                    DirectSoundOut chs = new DirectSoundOut();
                    chs.Init(new VorbisWaveReader(System.IO.Path.GetFullPath("Nvorbis.AudioOut.dll")));
                    chs.Play();
                }
                ChangeVisuals();
                ChangeColor(LastEventLeft, brushes["left"]);
                ChangeColor(LastEventRight, brushes["right"]);
            }
            List<Event> timing = Events.FindAll(i => i.mapEventList.Find(j => j.data[0] == "Timing") != null && i.GetData(1, "Timing") != "0" && i.Time < nowTime);
            if (timing != null && timing.Count != 0)
            {
                IsChangedTiming = true;
                int need = 0;
                foreach (Line l in OnlyPies.Children)
                {
                    if (l.Margin.Left < nowTime * Zoom) need += 1;
                }
                LabelDebug11.Content = "Timing detected:" + need + "<" + (ConfigReg.TimingTimes/4);
                if (need < ConfigReg.TimingTimes / 4 || need > ConfigReg.TimingTimes / 2)
                {
                    //MessageBox.Show("t");
                    Event near = null;
                    foreach (Event one in timing) if (near == null || one.Time > near.Time) near = one;
                    double freq = double.Parse(near.GetData(1, "Timing"));
                    double offset = double.Parse(near.GetData(2, "Timing")) + near.Time;
                    double count = int.Parse(near.GetData(4, "Timing"));
                    while (offset < nowTime - (freq * Math.Round((double)(ConfigReg.TimingTimes / 3 / count)))) offset += freq;
                    foreach (Line l in OnlyPies.Children)
                    {
                        // l.Margin = new Thickness(100,0,0,0);
                        l.Margin = new Thickness((offset * Zoom), 0, 0, 0);
                        offset += freq / count;
                    }
                }
            }
            else if (IsChangedTiming)
            {
                IsChangedTiming = false;
                foreach (Line l in OnlyPies.Children) l.Margin = new Thickness(0,0,0,0);
            }
        } // Finds nearest events =WORKS=
        private void UpdateMargin()
        {
            foreach (Event one in Events)
            {
                if (one.IsMoving == false) one.LeftMargin = (one.Time * Zoom - 30);
            }
        } // =+=
        private void UpdateUI()
        {
            if (Scrolling == false) PositionBar.Value = OggReader.CurrentTime.TotalSeconds;
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(PositionBar.Value);
                if (ts.TotalSeconds > 0 && ts.TotalSeconds < OggReader.TotalTime.TotalSeconds) OggReader.CurrentTime = ts;
            }
            CurTime.Content = OggReader.CurrentTime;
            ZoomLabel.Content = "Zoom = X" + ConfigReg.Zoom.ToString();
            EventsCount.Content = "Events count:" + Events.Count.ToString();
            LabelDebug1.Content = "Null events:" + Events.FindAll(i=>i==null).Count.ToString();
            LabelDebug2.Content = "Nearest:" + (Nearest?.Time.ToString() ?? "NULL");
            //LabelDebug3.Content = EventMouseDown();
            LabelDebug4.Content = "Fast save in:" + (30000 - FastSaveTick).ToString();
            LabelDebug5.Content = "Left:" + (LastEventLeft?.Time.ToString() ?? "NULL") + " Right:" + (LastEventRight?.Time.ToString() ?? "NULL" );
            LabelDebug6.Content = "Real zoom:" + Zoom + " min:" + (OggReader.CurrentTime.TotalSeconds - (((double)ConfigReg.CaretMargin) / (double)Zoom)) + "max: " + (OggReader.CurrentTime.TotalSeconds + ((EditorBackPic.ActualWidth - ConfigReg.CaretMargin) / (double)Zoom));
            LabelDebug7.Content = "X:" + Pos.X + " Y:" + Pos.Y + " Dx:" + Dx;
            LabelDebug8.Content = "CreateTim:" + CreatingTiming.ToString() + " ChangeTim:" + ChangingTiming.ToString() + " Times.Count:" + Times.Count + " Chld:" + OnlyPies.Children.Count;
            LabelDebug9.Content = "SMagnet:" + MagnetOnSpawn.ToString() + " TMagnet:" + MagnetOnTiming.ToString() + " Enter:" + IsFocusEventWindow;
            LabelDebug10.Content = "BlockLeft:" + IsLeftBlocked + " BlockRight:" + IsRightBlocked + " IsMoving:" + IsEventMovIng + " IsMoved:" + IsEventMovEd;
            //LabelDebug11.Content timing in updateborders 
            if (Events.Count > 1337) Spinner.Visibility = Visibility.Visible;
        } // Visual digits =+= 1 CALL

        private void UpdateEvent(Event oneEvent)
        {
            if(oneEvent.mapEventList.Count == 0)
            {
                RemoveEvent(oneEvent);
            }
            else
            {
                ChangeColor(oneEvent);
                DrawImage(oneEvent);
                oneEvent.NeedUpdate = false;
            }
        } // =+= 1 CALL
        private void UpdateColor() { foreach (Event one in Events) ChangeColor(one); } // ChangeColor() AOE NOT USED
        private void UpdateImages() { foreach (Event oneEvent in Events) { DrawImage(oneEvent); } } // DrawImage() AOE NOT USED
        private void UpdateInfo(int t)
        {
            InfoTick += t;
            if (InfoTick > 99)
            {
                double musicTime = OggReader.TotalTime.TotalSeconds;
                List<double> bal = new List<double>();
                List<double> mid = new List<double>();
                StatsPolygon.Points = new PointCollection();
                int arcsCount = 0;
                int gameplayCount = 0;
                int mappingCount = 0;
                int storyboardCount = 0;
                int infoCount = 0;
                int count = 0;
                int midCount;
                double balance = 0;
                if (infoCount == 0)
                {
                    if (EditorData.info != null) infoCount += (int)Math.Round(Math.Sqrt(EditorData.info.Count()));
                    if (EditorData.levelResources != null) infoCount += 10;
                    if (EditorData.lives != 15) infoCount += 10;
                    if (EditorData.moreInfoURL != null) infoCount += 30;
                    if (EditorData.name != null) infoCount += 10;
                    if (EditorData.tags != null) infoCount += 20;
                } //REWORK
                foreach (Event one in Events)
                {
                    switch (one.Type)
                    {
                        case "Gameplay":
                            if (one.GetData(1).Contains("Up")) arcsCount += 1;
                            if (one.GetData(1).Contains("Down")) arcsCount += 1;
                            if (one.GetData(1).Contains("Left")) arcsCount += 1;
                            if (one.GetData(1).Contains("Right")) arcsCount += 1;
                            bal.Add(one.Time);
                            gameplayCount += 1;
                            break;
                        case "Mapping":
                            mappingCount += one.EventCount;
                            break;
                        case "Storyboard":
                            storyboardCount += one.EventCount;
                            break;
                    }
                    count += one.EventCount;
                }
                if (bal.Count > 10)
                {
                    bal.Sort();
                    for (int i = 0; i < bal.Count - 1; i++) mid.Add(Math.Abs(bal[i] - bal[i + 1]));
                    mid.Sort();
                    midCount = mid.Count / 10;
                    if (mid.Count > 10)
                    {
                        mid.RemoveRange(0, midCount);
                        mid.RemoveRange(mid.Count - midCount - 1, midCount);
                        balance = Math.Abs(mid[mid.Count - 1] - mid[0]);
                    }
                }
                //===========================HARD MATH======================================
                int arg1 = 100 - (int)Math.Round(100 / (0.01 * arcsCount + 1));
                int arg2 = 100 - (int)Math.Round(100 / (0.3 * (EditorData.levelResources.Count) + 1));
                int arg3 = 100 - (int)Math.Round((double)(100 / (infoCount + 1)));
                int arg4 = 100 - (int)Math.Round(100 / (0.1 * storyboardCount + 1));
                int arg5 = 100 - (int)Math.Round(100 / (arcsCount / musicTime * 10 + 1));
                int arg6 = 100 - (int)Math.Round((double)(100 / (100 / (balance + 1))));
                StatsPolygon.Points.Add(new Point(180 - 1.8 * arg1, 180));
                StatsPolygon.Points.Add(new Point(180 - 0.9 * arg2, 180 - 1.55 * arg2));
                StatsPolygon.Points.Add(new Point(180 + 0.9 * arg3, 180 - 1.55 * arg3));
                StatsPolygon.Points.Add(new Point(180 + 1.8 * arg4, 180));
                StatsPolygon.Points.Add(new Point(180 + 0.9 * arg5, 180 + 1.55 * arg5));
                StatsPolygon.Points.Add(new Point(180 - 0.9 * arg6, 180 + 1.55 * arg6));
                //===========================================================================
                InfoName.Text = EditorData.name;
                InfoDescription.Text = EditorData.info;
                InfoCheckpoints.Content = EditorData.checkpoints.Count;
                InfoTime.Content = Math.Round(musicTime, 3);
                InfoLives.Text = EditorData.lives.ToString();
                InfoMaxLives.Text = EditorData.maxLives.ToString();
                InfoSpeed.Text = EditorData.speed.ToString();
                InfoTags.Text = EditorData.tags.Count + ": ";
                foreach (string tag in EditorData.tags) InfoTags.Text += tag + " ";
                InfoPath.Text = MapFiles.MapDir;
                InfoAnswer.Content = "42";
                InfoResources.Content = EditorData.levelResources.Count();
                InfoCount.Content = count;
                InfoBackup.Content = Directory.GetFiles(MapFiles.BackDir)?.Count().ToString() ?? "ERROR";
                InfoArcs.Content = "Events: " + gameplayCount + " Arcs: " + arcsCount;
                InfoStoryboard.Content = storyboardCount;
                InfoTimings.Content = mappingCount;
                InfoBPM.Content = Math.Round(arcsCount / musicTime, 3);
                InfoDiff.Content = Math.Round(arcsCount / musicTime * 2.5, 1) + "* of 10*";
                InfoTick = 0;
            }
        } // Updates info about map =+= 2 CALLS 1 BUTTON

        private bool CheckBorder(Event oneEvent)
        {
            if (oneEvent.Time > OggReader.CurrentTime.TotalSeconds - (((double)ConfigReg.CaretMargin)/(double)Zoom) && oneEvent.Time < OggReader.CurrentTime.TotalSeconds + (((double)EditorBackPic.ActualWidth - (double)ConfigReg.CaretMargin)/ (double)Zoom))
            {
                if (EventOnlyGrid.Children.Contains(oneEvent.GetBorder) == false)
                {
                    switch (oneEvent.Type)
                    {
                        case "Storyboard":
                            if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventStory();
                            EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                            Grid.SetRow(oneEvent.GetBorder, 0);
                            break;
                        case "Gameplay":
                            if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventArc();
                            EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                            Grid.SetRow(oneEvent.GetBorder, 1);
                            break;
                        case "Mapping":
                            if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventTiming();
                            EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                            Grid.SetRow(oneEvent.GetBorder, 2);
                            break;
                        default:
                            MessageBox.Show("ERROR 0x000004 INFO:" + oneEvent.Type);
                            break;
                    }
                }
                return true;
            }
            else
            {
                if (EventOnlyGrid.Children.Contains(oneEvent.GetBorder) && oneEvent.IsMoving == false)
                {
                    EventOnlyGrid.Children.Remove(oneEvent.GetBorder);
                }
                return false;
            }
        }
        private void ChangeVisuals()
        {
            ((PanelConfig)LeftBorder.Child).UpdateVisual(LastEventLeft);
            ((PanelConfig)RightBorder.Child).UpdateVisual(LastEventRight);
        }  // Updates config window
        private void ChangeEventTime(Event oneEvent)
        {
            double time = FindTime((oneEvent.LeftMargin + 30) / Zoom);
            if (MagnetOnSpawn)
            {
                IsNear(oneEvent.Type ,time);
                if (Nearest != null)
                {
                    if (Nearest.Time > 0 && Nearest.Time < OggReader.TotalTime.TotalSeconds) oneEvent.Time = Math.Round(Nearest.Time, 3); else MessageBox.Show("ERROR 0x000027 INFO:" + Nearest.Time);
                    foreach (MapEvent mevent in oneEvent.mapEventList)
                    {
                        Nearest.TryAddEvent(mevent);
                    }
                    RemoveEvent(oneEvent);
                    DrawImage(Nearest);
                }
                else
                {
                    if (time > 0 && time < OggReader.TotalTime.TotalSeconds) oneEvent.Time = Math.Round(time,3);
                    oneEvent.LeftMargin = oneEvent.Time * Zoom - 30;
                    oneEvent.IsMoving = false;
                }
            }
            else
            {
                if (time > 0 && time < OggReader.TotalTime.TotalSeconds) oneEvent.Time = Math.Round(time,3);
                oneEvent.LeftMargin = oneEvent.Time * Zoom - 30;
                oneEvent.IsMoving = false;
            }
            
        } // Changes time of selected moved events =+= 1 CALL
        private void ChangeSelect(Event one)
        {
            if (one != null)
            {
                one.ChangeSelect();
                ChangeColor(one);
            }
        } // Changes selecting of Event =+= 3 CALLS
        private void ChangeColor(Event one, SolidColorBrush col = null)
        {
            if (one != null)
            {
                if (col == null)
                {
                    if (one == LastEventLeft && IsLeftBlocked == false)
                    {
                        one.BorderColor = brushes["left"];
                    }
                    else if (one == LastEventRight && IsRightBlocked == false)
                    {
                        one.BorderColor = brushes["right"];
                    }
                    else
                    {
                        one.BorderColor = brushes["default"];
                    }
                }
                else
                {
                    one.BorderColor = col;
                }
                switch (one.Type)
                {
                    case "Storyboard":
                        if (one.IsSelected) ((WEventStory)one.GetBorder.Child).ColorPolygon.StrokeThickness = 4; else ((WEventStory)one.GetBorder.Child).ColorPolygon.StrokeThickness = 1;
                        break;
                    case "Gameplay":
                        if (one.IsSelected) ((WEventArc)one.GetBorder.Child).ColorPolygon.StrokeThickness = 4; else ((WEventArc)one.GetBorder.Child).ColorPolygon.StrokeThickness = 1;
                        break;
                    case "Mapping":
                        if (one.IsSelected) ((WEventTiming)one.GetBorder.Child).ColorPolygon.StrokeThickness = 4; else ((WEventTiming)one.GetBorder.Child).ColorPolygon.StrokeThickness = 1;
                        break;
                    default:
                        MessageBox.Show("ERROR 0x000014 INFO:" + one.Type);
                        break;
                }
            }
        } // Changes Event's Color =WORKS=
        private void ChangeZoom(double change = 0)
        {
            if (IsEventMovIng == false)
            {
                if (change != 0 && ((change > 0 && ConfigReg.Zoom + change < 50)||(change < 0 && ConfigReg.Zoom + change > 0)))
                {
                    ConfigReg.Zoom = (int)(ConfigReg.Zoom + change);
                }
                ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
                Zoom = (int)Math.Pow(ConfigReg.Zoom,2);
                Caret10Mill.Margin = new Thickness(Zoom * 0.01, 0, 0, 0);
                Caret100Mill.Margin = new Thickness(Zoom * 0.1, 0, 0, 0);
                CaretSec.Margin = new Thickness(Zoom, 0, 0, 0);
                Caret10Sec.Margin = new Thickness(Zoom * 10, 0, 0, 0);
                Caret100Sec.Margin = new Thickness(Zoom * 100, 0, 0, 0);
                if (Caret10Mill.Margin.Left < 5) Caret10Mill.Visibility = Visibility.Collapsed; else Caret10Mill.Visibility = Visibility.Visible;
                if (Caret100Mill.Margin.Left < 5) Caret100Mill.Visibility = Visibility.Collapsed; else Caret100Mill.Visibility = Visibility.Visible;
                if (CaretSec.Margin.Left < 5) CaretSec.Visibility = Visibility.Collapsed; else CaretSec.Visibility = Visibility.Visible;
                if (Caret10Sec.Margin.Left < 5) Caret10Sec.Visibility = Visibility.Collapsed; else Caret10Sec.Visibility = Visibility.Visible;
                if (Caret10Sec.Margin.Left < 5) Caret100Sec.Visibility = Visibility.Collapsed; else Caret100Sec.Visibility = Visibility.Visible;
                foreach (Event oneEvent in Events) oneEvent.LeftMargin = oneEvent.Time * Zoom - 30;
            }
        } // Changes ConfigReg.Zoom =+= 1 CALL 2 WHEEL
        private void ChangeCaretMargin(int change = 0)
        {
            if (ConfigReg.CaretMargin - change > 10 && ConfigReg.CaretMargin + change < TexaditorWindow.Width / 1.5)
            {
                ConfigReg.CaretMargin = ConfigReg.CaretMargin + change;
            }
            else
            {
                ConfigReg.CaretMargin = 400;
            }
            CaretMargin = ConfigReg.CaretMargin;
            Caret.Margin = new Thickness(ConfigReg.CaretMargin, 0, 0, 0);
        } // Changes margin of Caret =+= 1 CALL 2 WHEEL
        private void ChangeEventMode(bool? story = null, bool? game = null, bool? map = null)
        {
            if (ChangingTiming == false)
            {
                if (story == null && game == null && map == null)
                {
                    if (IsStoryboadBlocked == false && IsGamepalyBlocked == false && IsMappingBlocked == false)
                    {
                        IsStoryboadBlocked = true;
                        IsGamepalyBlocked = false;
                        IsMappingBlocked = false;
                    }
                    else if (IsStoryboadBlocked == true && IsGamepalyBlocked == false && IsMappingBlocked == false)
                    {
                        IsStoryboadBlocked = false;
                        IsGamepalyBlocked = true;
                        IsMappingBlocked = false;
                    }
                    else if (IsStoryboadBlocked == false && IsGamepalyBlocked == true && IsMappingBlocked == false)
                    {
                        IsStoryboadBlocked = false;
                        IsGamepalyBlocked = false;
                        IsMappingBlocked = true;
                    }
                    else if (IsStoryboadBlocked == false && IsGamepalyBlocked == false && IsMappingBlocked == true)
                    {
                        IsStoryboadBlocked = true;
                        IsGamepalyBlocked = false;
                        IsMappingBlocked = true;
                    }
                    else if (IsStoryboadBlocked == true && IsGamepalyBlocked == false && IsMappingBlocked == true)
                    {
                        IsStoryboadBlocked = true;
                        IsGamepalyBlocked = true;
                        IsMappingBlocked = false;
                    }
                    else if (IsStoryboadBlocked == true && IsGamepalyBlocked == true && IsMappingBlocked == false)
                    {
                        IsStoryboadBlocked = false;
                        IsGamepalyBlocked = true;
                        IsMappingBlocked = true;
                    }
                    else if (IsStoryboadBlocked == false && IsGamepalyBlocked == true && IsMappingBlocked == true)
                    {
                        IsStoryboadBlocked = true;
                        IsGamepalyBlocked = true;
                        IsMappingBlocked = true;
                    }
                    else 
                    {
                        IsStoryboadBlocked = false;
                        IsGamepalyBlocked = false;
                        IsMappingBlocked = false;
                    }
                }
                else
                {
                    IsStoryboadBlocked = story??IsStoryboadBlocked;
                    IsGamepalyBlocked = game??IsGamepalyBlocked;
                    IsMappingBlocked = map??IsMappingBlocked;
                }
                if (IsStoryboadBlocked) BlockStoryboard.Visibility = Visibility.Visible; else BlockStoryboard.Visibility = Visibility.Collapsed;
                if (IsGamepalyBlocked) BlockGameplay.Visibility = Visibility.Visible; else BlockGameplay.Visibility = Visibility.Collapsed;
                if (IsMappingBlocked) BlockMapping.Visibility = Visibility.Visible; else BlockMapping.Visibility = Visibility.Collapsed;
            }
        } // Changes editing mode =+= 1 BUTTON
        private void ChangeConfigWindowMode(bool? left = null, bool? right = null)
        {
            if (left == null && right == null)
            {
                if (IsLeftBlocked == true && IsRightBlocked == true)
                {
                    IsLeftBlocked = false;
                    IsRightBlocked = true;
                }
                else if (IsLeftBlocked == false && IsRightBlocked == true)
                {
                    IsLeftBlocked = true;
                    IsRightBlocked = false;
                }
                else if (IsLeftBlocked == true && IsRightBlocked == false)
                {
                    IsLeftBlocked = false;
                    IsRightBlocked = false;
                }
                else if (IsLeftBlocked == false && IsRightBlocked == false)
                {
                    IsLeftBlocked = true;
                    IsRightBlocked = true;
                }
            }
            else
            {
                IsLeftBlocked = left ?? IsLeftBlocked;
                IsRightBlocked = right ?? IsRightBlocked;
            }
            if (IsLeftBlocked == false) LeftBorder.Visibility = Visibility.Visible; else LeftBorder.Visibility = Visibility.Collapsed;
            if (IsRightBlocked == false) RightBorder.Visibility = Visibility.Visible; else RightBorder.Visibility = Visibility.Collapsed;
        } // Changes EventConfigWindows =+= 1 BUTTON
        private void ChangePlaybackState()
        {
            Uri u = new Uri("res\\" + "PicPauseButton.png", UriKind.Relative);
            switch (AudioOut.PlaybackState)
            {
                case PlaybackState.Stopped:
                    AudioOut.Play();
                    break;
                case PlaybackState.Paused:
                    AudioOut.Play();
                    break;
                case PlaybackState.Playing:
                    AudioOut.Pause();
                    u = new Uri("res\\" + "PicPlayButton.png", UriKind.Relative);
                    break;
                default:
                    break;
            }
            PlayPauseButton.Content = new Image() { Source = new BitmapImage(u), Stretch = Stretch.Uniform };
        } // Changes Play/Pause =+= 2 CALLS 1 BUTTON
        private void ScrollRight()
        {
            ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
            if (OggReader.CurrentTime.TotalSeconds < OggReader.TotalTime.TotalSeconds - (int)(ConfigReg.ScrollSeconds / ConfigReg.Zoom + 1))
            {
                OggReader.CurrentTime += new TimeSpan(0, 0, 0, (int)(ConfigReg.ScrollSeconds / ConfigReg.Zoom), ConfigReg.ScrollMills);
            }
            else
            {
                OggReader.CurrentTime = OggReader.TotalTime - new TimeSpan(0, 0, 0, 0, 10);
            }
        } // =+= 1 WHEEL
        private void ScrollLeft()
        {
            ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
            if (OggReader.CurrentTime.TotalSeconds > (ConfigReg.ScrollSeconds / ConfigReg.Zoom + 1))
            {
                OggReader.CurrentTime -= new TimeSpan(0, 0, 0, (int)(ConfigReg.ScrollSeconds / ConfigReg.Zoom), ConfigReg.ScrollMills);
            }
            else
            {
                OggReader.CurrentTime = new TimeSpan(0, 0, 0, 0, 1);
            }
        } // =+= 1 WHEEL
        #endregion
        #region События в xaml
        private void Button_Click(object sender, RoutedEventArgs e){ System.Diagnostics.Process.Start("https://discord.gg/JPChWnG"); }
        private void ComputerDown(object sender, SessionEndingCancelEventArgs e)
        {
            FastSave(30000);
            Application.Current.Shutdown();
        }
        private void ShutUp(object sender, ExitEventArgs e) { OpenMainWindow(); } // =WORKS=
        private void PositionBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ChangingTiming == false)
            {
                Scrolling = true;
                if (AudioOut.PlaybackState == PlaybackState.Playing) PreparingToPlay = true; else PreparingToPlay = false;
                AudioOut.Pause();
                OggReader.CurrentTime = new TimeSpan(0, 0, (int)Math.Round((double)PositionBar.Value));
            }
        } // =WORKS=
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e) { ChangePlaybackState(); } // =WORKS=
        private void ScrollLeftButton_Click(object sender, RoutedEventArgs e) { ScrollLeft(); }
        private void ScrollRightButton_Click(object sender, RoutedEventArgs e) { ScrollRight(); }
        private void TimerTick(object sender, EventArgs e){Update();} // =WORKS=
        private void Exit_Click(object sender, RoutedEventArgs e) { Cancel(); } // =WORKS= 

        private void MuteSounds_Unchecked(object sender, RoutedEventArgs e) { IsMuted = false; }
        private void MuteSounds_Checked(object sender, RoutedEventArgs e) { IsMuted = true; }

        private void Save_Click(object sender, RoutedEventArgs e) { SaveMap(); } // =WORKS=
        private void FastSave_Click(object sender, RoutedEventArgs e) { FastSave(30000); } // =WORKS=
        private void LoadBackup(object sender, RoutedEventArgs e) { LoadFromBackup(); } // =WORKS=
        private void AddResources_Click(object sender, RoutedEventArgs e) { AddRes(); } // =WORKS=
        private void ChangeIcon_Click(object sender, RoutedEventArgs e){ChangeIcon(); } // =WORKS=
        private void OpenFolder_Click(object sender, RoutedEventArgs e) { OpenExplorer(MapFiles.MapDir); } // =WORKS=
        private void UpdateInfo_Click(object sender, RoutedEventArgs e) { UpdateInfo(1000); } // =WORKS=

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            IsFocusEventWindow = true;
        }
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            IsFocusEventWindow = false;
        }

        private void SpawnMagnet_Checked(object sender, RoutedEventArgs e){MagnetOnSpawn = true; } // =WORKS=
        private void TimingMagnet_Checked(object sender, RoutedEventArgs e){MagnetOnTiming = true; } // =WORKS=
        private void SpawnMagnet_Unchecked(object sender, RoutedEventArgs e){MagnetOnSpawn = false; } // =WORKS=
        private void TimingMagnet_Unchecked(object sender, RoutedEventArgs e){MagnetOnTiming = false; } // =WORKS=
        private void ButRem_Right(object sender, RoutedEventArgs e) { ManualRemoveEvent(LastEventRight); } // =+=
        private void ButRem_Left(object sender, RoutedEventArgs e) { ManualRemoveEvent(LastEventLeft); } // =+=
        private void ButRem_Third(object sender, RoutedEventArgs e){ ManualRemoveEvent(EditedEvent); ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed; }
        private void ButSetTiming_Left(object sender, RoutedEventArgs e)
        {
            CreateTiming(LastEventLeft);
        }
        private void ButSetTiming_Right(object sender, RoutedEventArgs e)
        {
            CreateTiming(LastEventRight);
        }
        private void ButSetTiming_Third(object sender, RoutedEventArgs e)
        {
            CreateTiming(EditedEvent);
            ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
        }
        private void ChangeTags_Click(object sender, RoutedEventArgs e)
        {
            if(TagsPanel.Visibility == Visibility.Visible)
            {
                EditorData.tags.Clear();
                foreach (CheckBox box in TagsPanel.Children)
                {
                    if (box.IsChecked == true)
                    {
                        EditorData.tags.Add((string)box.Content);
                    }
                    else
                    {
                        EditorData.tags.Remove((string)box.Content);
                    }
                }
                ChangeTags.Content = "Change";
                TagsPanel.Visibility = Visibility.Collapsed;
                UpdateInfo(1000);
            }
            else
            {
                foreach (CheckBox box in TagsPanel.Children)
                {
                    box.IsChecked = EditorData.tags.Contains(box.Content);
                }
                ChangeTags.Content = "Apply";
                TagsPanel.Visibility = Visibility.Visible;
            }
        } // =WORKS=
        private void InfoName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != null && EditorData != null) EditorData.name = ((TextBox)sender).Text;
        } // =WORKS=
        private void InfoDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != null && EditorData != null) EditorData.info = ((TextBox)sender).Text;
        } // =WORKS=
        private void InfoLives_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int res))
            {
                if (res != 0 && EditorData != null)EditorData.lives = res;
            }
            else
            {
                MessageBox.Show("Only digits!!!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        } // =WORKS=
        private void InfoSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int res))
            {
                if (res != 0 && EditorData != null) EditorData.speed = res;
            }
            else
            {
                MessageBox.Show("Only digits!!!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        } // =WORKS=
        private void InfoMaxLives_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int res))
            {
                if (res != 0 && EditorData != null) EditorData.maxLives = res;
            }
            else
            {
                MessageBox.Show("Only digits!!!", "WARNING!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        } // =WORKS=

        private void DiscordGroup_Click(object sender, RoutedEventArgs e){System.Diagnostics.Process.Start("https://discord.gg/6cD4wC"); } // =WORKS=
        private void DiscordMe_Click(object sender, RoutedEventArgs e){MessageBox.Show("caxapexac#6442"); } // =WORKS=
        private void GmailMe_Click(object sender, RoutedEventArgs e){ MessageBox.Show("russianstudentmine@gmail.com"); } // =WORKS=
        private void SteamGroup_Click(object sender, RoutedEventArgs e){System.Diagnostics.Process.Start("http://steamcommunity.com/app/513510/discussions/"); } // =WORKS=
        private void SteamMe_Click(object sender, RoutedEventArgs e){System.Diagnostics.Process.Start("http://steamcommunity.com/id/caxp/"); } // =WORKS=
        #endregion
        #region работа с файлами
        private string FileDialog(string file)
        {
            Microsoft.Win32.OpenFileDialog ChooseFile = new Microsoft.Win32.OpenFileDialog();
            switch (file)
            {
                case "Icon":
                    ChooseFile.DefaultExt = ".jpg";
                    ChooseFile.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png";
                    ChooseFile.Multiselect = false;
                    ChooseFile.Title = "Choose a preview image";
                    ChooseFile.InitialDirectory = ConfigReg.DirResource;
                    if (ChooseFile.ShowDialog() == true)
                    {
                        if (ChooseFile.FileName.EndsWith(".jpg") | ChooseFile.FileName.EndsWith(".png"))
                        {
                            return ChooseFile.FileName;
                        }
                        else goto default;
                    }
                    else goto default;
                case "Backup":
                    ChooseFile.DefaultExt = ".data";
                    ChooseFile.Filter = "map.data files (*.data)|*.data";
                    ChooseFile.Multiselect = false;
                    ChooseFile.Title = "Load from backup (Fast save)";
                    ChooseFile.InitialDirectory = MapFiles.BackDir;
                    if (ChooseFile.ShowDialog() == true)
                    {
                        if (ChooseFile.FileName.EndsWith(".data"))
                        {
                            return ChooseFile.FileName;
                        }
                        else goto default;
                    }
                    else goto default;
                default:
                    return null;
            }

        } // Choosing Icon/Backup =+= 2 CALLS
        private string[] FileDialogRes()
        {
            Microsoft.Win32.OpenFileDialog ChooseRes = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png",
                Multiselect = true,
                Title = "Choose resourses",
                InitialDirectory = ConfigReg.DirResource
            };
            if (ChooseRes.ShowDialog() == true) return ChooseRes.FileNames; else return null;
        } // Choosing resources to add =+= 1 CALL
        private void Cancel()
        {
            if (Tab.SelectedIndex != 4)
            {
                Tab.SelectedIndex = 4;
            }
            else
            {
                ExitAndSave();
            }
        } // =+= 1 CALL 1 BUTTON
        private void ExitAndSave()
        {
            AudioOut.Stop();
            switch (MessageBox.Show("Do you really want to save the map and exit from the Texaditor?", "WARNING", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    SaveMap();
                    OpenExplorer(MapFiles.MapDir);
                    Close();
                    break;
                case MessageBoxResult.No:
                    FastSave(30000);
                    OpenMainWindow();
                    TexaditorWindow.Close();
                    break;
                case MessageBoxResult.Cancel:
                    DirectSoundOut nyas = new DirectSoundOut();
                    nyas.Init(new VorbisWaveReader(System.IO.Path.GetFullPath("NAudio.NVorbis.dll")));
                    nyas.Play();
                    MessageBox.Show("What a great choise!          P.S. Ika is watching you. Believe me.");
                    if(Events.Count > 1000) EasterLabel1.Visibility = Visibility.Visible;
                    break;
            }
        } // Save and exit || Exit || Dont exit =+= 1 CALL
        private void FastSave(int i)
        {
            FastSaveTick += i;
            if (FastSaveTick > 29999)
            {
                EditorData.events.Clear();
                Events.Sort(new CompareEvent().GetComparer());
                foreach (Event one in Events) foreach (MapEvent mevent in one.mapEventList) EditorData.events.Add(new MapEvent(mevent, true));
                string BackupName = MapFiles.BackDir + "\\Backup " + new Random().Next() + " " + EditorData.events.Count + " events.data";
                File.WriteAllText(BackupName, CustomEditor.GetConfig(EditorData));
                FastSaveTick = 0;
                StatusLabel.Content = "Fast save completed";
            }
        } // Fast save every 30k ticks =WORKS=
        private void SaveMap()
        {
            FastSave(30000);
            switch (MessageBox.Show("Do you really want to save your map?", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    File.Delete(MapFiles.ConfigPath);
                    File.WriteAllText(MapFiles.ConfigPath, CustomEditor.GetConfig(EditorData));
                    File.WriteAllText(MapFiles.MapDir + "\\config.txt", CustomEditor.GetConfig(EditorData));
                    ConfigReg.DirContinue = MapFiles.MapDir;
                    break;
            }
            
        } // Resave map.data =WORKS=
        private void OpenExplorer(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + path + "\"");
        } // Opens explorer.exe on user's map =+= 2 CALLS
        private void LoadFromBackup()
        {
            AudioOut.Pause();
            FastSave(30000);
            string BackupName = FileDialog("Backup");
            if (BackupName != null && BackupName.EndsWith(".data"))
            {
                string backup = File.ReadAllText(BackupName);
                if (backup != null && backup != "")
                {
                    MapData BufData = CustomEditor.GetMap(backup);
                    if (BufData != null && BufData.musicTime == EditorData.musicTime)
                    {
                        File.Copy(BackupName, MapFiles.ConfigPath, true);
                        LoadEvents();
                        LoadResources();
                        StatusLabel.Content = "Map loaded from backup " + BackupName; //log
                        return;
                    }
                }
                StatusLabel.Content = "ERROR 0x000016 " + BackupName; //log
            }
        } // Loads map from the backup =+= 1 BUTTON
        private void AddRes()
        {
            string[] resources = FileDialogRes();
            if (resources != null)
            {
                foreach (string res in resources)
                {
                    string name = res.Substring(res.LastIndexOf("\\") + 1);
                    string sname = name.Substring(0,name.IndexOf(char.Parse(".") ));
                    string type;
                    File.Copy(res, MapFiles.ResDir + "\\Res_" + name, true);
                    if (res.EndsWith(".jpg") | res.EndsWith(".png"))
                    {
                        type = "Sprite";
                    }
                    else
                    {
                        type = "Unknown";
                    }
                    MapResource mr = new MapResource(sname, type, "resources\\Res_" + name, "");
                    EditorData.levelResources.Add(mr);
                    ((PanelConfig)LeftBorder.Child).AddRes(mr);
                    ((PanelConfig)RightBorder.Child).AddRes(mr);
                }
            }
        } // Addes Resource(s) into the map =+= 1 BUTTON
        private void ChangeIcon()
        {
            string iconPath = FileDialog("Icon");
            if (iconPath != null)
            {
                if (iconPath.EndsWith(".jpg"))
                {
                    MapFiles.IconName = "icon.jpg";
                    EditorData.iconFile = "icon.jpg";
                    File.Copy(iconPath, MapFiles.IconPath);
                }
                else
                {
                    MapFiles.IconName = "icon.png";
                    EditorData.iconFile = "icon.png";
                    File.Copy(iconPath, MapFiles.IconPath);
                }
            }
        } // Changes Icon of map =WORKS=
        private void OpenMainWindow()
        {
            MainWindow win = new MainWindow();
            win.Show();
        } // Opens Create/Load/Continue window =WORKS=
        #endregion
        #region Работа с Эдитором
        private void LoadEvents()
        {
            RemoveEventAll();
            if (File.Exists(MapFiles.ConfigPath))
            {
                EditorData = CustomEditor.GetMap(File.ReadAllText(MapFiles.ConfigPath));
                foreach (MapEvent mevent in EditorData.events)
                {
                    List<string> ls = new List<string>() { mevent.data[0] };
                    ls.AddRange(mevent.data[1].Split(char.Parse(",")));
                    mevent.data = ls;
                    CreateEvent(mevent);
                }
            }
            else
            {
                EditorData = new MapData();
            }
        } // Load events from map.data AOE =+= 2 CALLS
        private void SelectAll()
        {
            foreach (Event one in Events)
            {
                string type = one.Type;
                if ((IsStoryboadBlocked == false && type == "Storyboard") ||
               (IsGamepalyBlocked == false && type == "Gameplay") ||
               (IsMappingBlocked == false && type == "Mapping"))
                {
                    one.IsSelected = true;
                    ChangeColor(one);
                }   
            }
        } // SelectedEvents.Add() AOE =+= 1 BUTTON
        private void InvertSelection()
        {
            foreach (Event oneEvent in Events)
            {
                ChangeSelect(oneEvent);
            }
        } // ChangeSelect() AOE =+= 1 BUTTON
        private void UnSelectAll()
        {
            foreach (Event one in Events)
            {
                one.IsSelected = false;
                ChangeColor(one);
            }
        } // SelectedEvents.Clear() AOE =+= 1 MOUSE
        private void RemoveEventAll()
        {
            UndoRedo.UndoStack.Clear();
            UndoRedo.RedoStack.Clear();
            EventOnlyGrid.Children.Clear();
            Events.Clear();
        } // Removes all the events AOE =+= 1 CALL 1 BUTTON
        private void RemoveEventAllSelected()
        {
            foreach (Event one in Events.FindAll(i=>i.IsSelected)) RemoveEvent(one);
        } // RemoveEvent() only selected AOE =+= 1 BUTTON 1 MOUSE

        private string Type (string data)
        {
            switch (data)
            {
                case "SpawnObj": return "Gameplay";
                case "Timing": return "Mapping";
                case "Note": return "Mapping";
                case "SetBGColor": return "Storyboard";
                case "SetPlayerDistance": return "Storyboard";
                case "ShowTitle": return "Storyboard"; 
                case "ShowSprite": return "Storyboard";
                case "MapEnd": return "Storyboard"; 
                default: MessageBox.Show("ERROR 0x000025 INFO:" + data); return null;
            }
        } //Extra
        private double FindTime(double startTime)
        {
            if (MagnetOnTiming && Events.Find(i => i.mapEventList.Find(j => j.data[0] == "Timing") != null && i.GetData(1, "Timing") != "0" && i.Time < OggReader.CurrentTime.TotalSeconds) != null)
            {
                Event near = null;
                foreach (Event one in Events.FindAll(i => i.Type == "Mapping" && i.GetData(1, "Timing") != "0" && i.Time < OggReader.CurrentTime.TotalSeconds))
                {
                    if (near == null || one.Time > near.Time) near = one;
                }
                double freq = double.Parse(near.GetData(1, "Timing"));
                double first = double.Parse(near.GetData(2, "Timing")) + near.Time;
                double count = (double)int.Parse(near.GetData(4, "Timing"));
                while (first < startTime) first += freq/count;
                if (first - (freq / count / 2) > startTime) first -= freq/count;
                return first;
            }
            return startTime;
        }
        private void CreateEvent(MapEvent mevent)
        {
            string type = Type(mevent.data[0]);
            if ((IsStoryboadBlocked == false && type == "Storyboard") || 
                (IsGamepalyBlocked == false && type == "Gameplay") ||
                (IsMappingBlocked == false && type == "Mapping"))
            {
                mevent.time = Math.Round(mevent.time, 3);
                bool added = false;
                foreach (Event one in Events)
                {
                    if (one.TryAddEvent(mevent))
                    {
                        added = true;
                        DrawImage(one);
                        if (one == LastEventLeft) ChangeVisuals();
                        if (one == LastEventRight) ChangeVisuals();
                        if (one == EditedEvent) ChangeVisuals();
                        break;
                    }
                }
                if (added == false)
                {
                    Event oneEvent = new Event(mevent);
                    CreateEvent(oneEvent);
                    StatusLabel.Content = "Event was created event in " + oneEvent.Time + " , " + oneEvent.Type;
                }
            }
        } // MAIN method =+= 2 CALLS 2 UndoRedo
        private void CreateEvent(Event oneEvent)
        {
            Events.Add(oneEvent);
            if (EventOnlyGrid.Children.Contains(oneEvent.GetBorder) == false)
            {
                switch (oneEvent.Type)
                {
                    case "Storyboard":
                        if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventStory();
                        EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                        Grid.SetRow(oneEvent.GetBorder, 0);
                        break;
                    case "Gameplay":
                        if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventArc();
                        EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                        Grid.SetRow(oneEvent.GetBorder, 1);
                        break;
                    case "Mapping":
                        if (oneEvent.GetBorder.Child == null) oneEvent.GetBorder.Child = new WEventTiming();
                        EventOnlyGrid.Children.Add(oneEvent.GetBorder);
                        Grid.SetRow(oneEvent.GetBorder, 2);
                        break;
                    default:
                        MessageBox.Show("ERROR 0x000004 INFO:" + oneEvent.Type);
                        break;
                }
                oneEvent.UI = true;
                oneEvent.GetBorder.PreviewMouseDown += EventMouseDown;
                DrawImage(oneEvent);
                ChangeColor(oneEvent);
                oneEvent.LeftMargin = oneEvent.Time * Zoom - 30;
            }
            else MessageBox.Show("ERROR 0x000029 INFO:" + oneEvent.Time);
        } // MAIN method =+= 3 CALLS
        private void RemoveEvent(MapEvent mevent)
        {
            if (mevent != null)
            {
                Event one = Events.Find(i=>i.mapEventList.Contains(mevent));
                if (one.TryRemoveEvent(mevent.data[0]) == false) RemoveEvent(one);
            }
            else MessageBox.Show("ERROR 0x000002");
            StatusLabel.Content = "MapEvent was removed from " + mevent.time + " , " + mevent.data[0];
        } // MAIN method =+= 2 UndoRedo
        private void RemoveEvent(Event oneEvent)
        {
            if (oneEvent != null)
            {
                if (oneEvent == LastEventLeft) LastEventLeft = null; ChangeVisuals();
                if (oneEvent == LastEventRight) LastEventRight = null; ChangeVisuals();
                if (oneEvent == EditedEvent) EditedEvent = null; ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
                if (EventOnlyGrid.Children.Contains(oneEvent.GetBorder))
                {
                    oneEvent.UI = false;
                    EventOnlyGrid.Children.Remove(oneEvent.GetBorder);
                    oneEvent.GetBorder.PreviewMouseDown -= EventMouseDown;
                }
                Events.Remove(oneEvent);
                StatusLabel.Content = "Event was removed from " + oneEvent.Time;
            }
            else MessageBox.Show("ERROR 0x000003");
        } // MAIN method =WORKS=
        private void IsNear(string type ,double time)
        {
            Nearest = null;
            double nearTime = ConfigReg.NearTime / 1000;
            foreach(Event one in Events)
            {
                if (one != null && one.Type == type && one.IsMoving == false)
                {
                    double nearer = Math.Abs(time - one.Time);
                    if (nearer < nearTime)
                    {
                        nearTime = nearer;
                        Nearest = one;
                    }
                }
                else if (one == null) MessageBox.Show("ERROR 0x000001");
            }
        } // Is any arc near? NO - Nearest = null | YES - Nearest = nearest event =WORKS=
        private void DrawImage(Event oneEvent)
        {
            if (oneEvent != null)
            {
                switch (oneEvent.Type)
                {
                    case "Storyboard":
                        ((WEventStory)oneEvent.GetBorder.Child).SetData(oneEvent);
                        break;
                    case "Gameplay":
                        ((WEventArc)oneEvent.GetBorder.Child).SetData(oneEvent);
                        break;
                    case "Mapping":
                        ((WEventTiming)oneEvent.GetBorder.Child).SetData(oneEvent);
                        break;
                    default:
                        MessageBox.Show("ERROR 0x000011 INFO:" + oneEvent.Type);
                        break;
                }
            }
            else MessageBox.Show("ERROR 0x000006");
        } // Changes preview pic of Event =+=

        private void UndoMethod()
        {
            Event UndoEvent = UndoRedo.Undo();
            if (UndoEvent != null && UndoEvent.Time > 0 && UndoEvent.Time < OggReader.TotalTime.TotalSeconds)
            {
                if (Events.Contains(UndoEvent))
                {
                    foreach(MapEvent mevent in UndoEvent.mapEventList) RemoveEvent(mevent);
                }
                else
                {
                    foreach (MapEvent mevent in UndoEvent.mapEventList) CreateEvent(mevent);
                }
                //UndoRedo.RedoStack.Push(UndoEvent);
            }
            else if(UndoEvent != null) MessageBox.Show("ERROR 0x000007 INFO:" + UndoEvent.Time);
        }// Ctrl + Z WORKS
        private void RedoMethod()
        {
            Event RedoEvent = UndoRedo.Redo();
            if (RedoEvent != null && RedoEvent.Time > 0 && RedoEvent.Time < OggReader.TotalTime.TotalSeconds)
            {
                if (Events.Contains(RedoEvent))
                {
                    foreach (MapEvent mevent in RedoEvent.mapEventList) RemoveEvent(mevent);
                }
                else
                {
                    foreach (MapEvent mevent in RedoEvent.mapEventList) CreateEvent(mevent);
                }
                //UndoRedo.UndoStack.Push(RedoEvent);
            }
            else if (RedoEvent != null) MessageBox.Show("ERROR 0x000008 INFO:" + RedoEvent.Time);
        }// Shift + Z || Ctrl + Y WORKS

        private void ManualCreateEvent(string[] data)
        {
            List<string> dataList = new List<string>(data);
            double time = FindTime(Math.Round(OggReader.CurrentTime.TotalSeconds, 3));
            if (MagnetOnSpawn)
            {
                IsNear(Type(dataList[0]) ,OggReader.CurrentTime.TotalSeconds);
                time = Nearest?.Time??time;
            }
            MapEvent mevent = new MapEvent(FindTime(time), dataList);
            CreateEvent(mevent);
        } // (Creates an Event || Edits the nearest Event) using W A S D etc =WORKS=
        private void ManualRemoveEvent(Event oneEvent)
        {
            RemoveEvent(oneEvent);
            UndoRedo.UndoStack.Push(oneEvent);
        } // Removes an Event using (mouse || X || Ctrl+X) =WORKS=
        private void ManualCopyEvent()
        {
            List<Event> buffer = new List<Event>();
            if (MovedEvent.IsSelected)
            {
                foreach (Event one in Events.FindAll(i=>i.IsSelected == true))
                {
                    one.IsSelected = false;
                    one.IsMoving = false;
                    Event two = new Event(one);
                    two.IsSelected = true;
                    two.IsMoving = true;
                    CreateEvent(two);
                    ChangeColor(one);
                }
            }
            else
            {
                MovedEvent.IsSelected = false;
                MovedEvent.IsMoving = false;
                Event two = new Event(MovedEvent);
                two.IsSelected = true;
                two.IsMoving = true;
                CreateEvent(two);
                ChangeColor(MovedEvent);
            }
        } // Copies Event(s) using Ctrl+Mouse =WORKS=
        
        #region Движение эвентов 
        private void EventMouseDown(object sender, MouseEventArgs e)
        {
            Tab.SelectedIndex = 4;
            Event one = Events.Find(i => i.GetBorder == (Border)sender);
            LabelDebug3.Content = "MUST BE 1:" + (Events.FindAll(i => i.GetBorder == (Border)sender).Count + " TIME:" + Events.Find(i => i.GetBorder == (Border)sender).Time);
            if (one != null && IsEventMovIng == false)
            {
                if (Mouse.RightButton == MouseButtonState.Pressed)
                {
                    if (one.IsSelected)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                        {
                            RemoveEventAllSelected();
                        }
                        else
                        {
                            RemoveEvent(one);
                        }
                    }
                    else
                    {
                        RemoveEvent(one);
                    }
                    IsEventMovIng = false;
                } // +
                else if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    StartMovePosition = Mouse.GetPosition(TexaditorWindow);
                    StartEventPosition = OggReader.CurrentTime.TotalSeconds;
                    MovedEvent = one;
                    if (one.IsSelected)
                    {
                        foreach (Event two in Events.FindAll(i => i.IsSelected == true)) two.IsMoving = true;
                    }
                    else
                    {
                        one.IsMoving = true;
                    }
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) PreparingToCopy = true;
                    IsEventMovIng = true;
                } // +
                else if (Mouse.MiddleButton == MouseButtonState.Pressed)
                {
                    EditedEvent = one;
                    Point pos = Mouse.GetPosition(ThirdBorder);
                    ((PanelConfig)ThirdBorder.Child).Margin = new Thickness(pos.X,pos.Y,0,0);
                    ((PanelConfig)ThirdBorder.Child).UpdateVisual(EditedEvent);
                    ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Visible;
                }
            }
            else MessageBox.Show("ERROR 0x000009");
        } // =WORKS=
        private void RectGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Tab.SelectedIndex = 4;
            if (Mouse.LeftButton == MouseButtonState.Pressed && Mouse.RightButton == MouseButtonState.Released)
            {
                ((PanelConfig)ThirdBorder.Child).Visibility = Visibility.Collapsed;
                UnSelectAll();
                Selecting = true;
                StartPos = Mouse.GetPosition(RectGrid);
                SelRect = new Rectangle()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(100, 0, 70, 170)),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 0,
                    Width = 0,
                };
                RectGrid.Content = SelRect;
            } // +
        } // =WORKS=
        private void TexaditorWindow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Selecting)
            {
                if (SelRect.Height + SelRect.Width > 10)
                {
                    double min = OggReader.CurrentTime.TotalSeconds - (ConfigReg.CaretMargin / (double)Zoom);
                    foreach (Event oneEvent in Events)
                    {
                        double fin = (oneEvent.Time - min) * Zoom;
                        if ((fin >= StartPos.X && fin <= Pos.X) || (fin <= StartPos.X && fin >= Pos.X))
                        {
                            if (oneEvent.Type == "Storyboard")
                            {
                                if (IsStoryboadBlocked == false && ((Pos.Y < 2 * (RectGrid.ActualHeight / 3)) || (StartPos.Y < RectGrid.ActualHeight / 3)))
                                {
                                    oneEvent.IsSelected = true;
                                }
                            }
                            else if (oneEvent.Type == "Mapping")
                            {
                                if (IsMappingBlocked == false && ((Pos.Y > RectGrid.ActualHeight / (4/3)) || (StartPos.Y > RectGrid.ActualHeight / (4/3))))
                                {
                                    oneEvent.IsSelected = true;
                                }
                            }
                            else
                            {
                                if (IsGamepalyBlocked == false && ((Pos.Y > RectGrid.ActualHeight / 1.5) || (StartPos.Y > RectGrid.ActualHeight / 1.5)))
                                {
                                    oneEvent.IsSelected = true; 
                                }
                            }
                        }
                        ChangeColor(oneEvent);
                    }
                }
                RectGrid.Content = null;
                Selecting = false;
            } // +
            else if (IsEventMovIng)
            {
                if (IsEventMovEd)
                {
                    List<Event> buf = new List<Event>();
                    foreach (Event one in Events.FindAll(i => i.IsMoving))
                    {
                        ChangeEventTime(one);
                        buf.Add(one);
                    }
                    foreach (Event two in buf.FindAll(i=>i.IsMoving))
                    {
                        RemoveEvent(two);
                    }
                    UpdateBorders();
                }
                else
                {
                    foreach (Event one in Events.FindAll(i => i.IsMoving))
                    {
                        one.IsMoving = false;
                    }
                    ChangeSelect(MovedEvent);
                }
                PreparingToCopy = false;
                IsEventMovIng = false;
                IsEventMovEd = false;
            } // +
        } // =WORKS=
        private void TexaditorWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (Selecting)
            {
                Pos = Mouse.GetPosition(RectGrid);
                if (Mouse.LeftButton == MouseButtonState.Released || Mouse.RightButton == MouseButtonState.Pressed)
                {
                    TexaditorWindow_MouseUp(sender, null);
                }
                else
                {
                    if (Pos.X >= StartPos.X)
                    {
                        SelRect.HorizontalAlignment = HorizontalAlignment.Left;
                        SelRect.Margin = new Thickness(StartPos.X, SelRect.Margin.Top, 0, SelRect.Margin.Bottom);
                    }
                    else
                    {
                        SelRect.HorizontalAlignment = HorizontalAlignment.Right;
                        SelRect.Margin = new Thickness(0, SelRect.Margin.Top, RectGrid.ActualWidth - StartPos.X, SelRect.Margin.Bottom);
                    }
                    if (Pos.Y >= StartPos.Y)
                    {
                        SelRect.VerticalAlignment = VerticalAlignment.Top;
                        SelRect.Margin = new Thickness(SelRect.Margin.Left, StartPos.Y, SelRect.Margin.Right, 0);
                    }
                    else
                    {
                        SelRect.VerticalAlignment = VerticalAlignment.Bottom;
                        SelRect.Margin = new Thickness(SelRect.Margin.Left, 0, SelRect.Margin.Right, RectGrid.ActualHeight - StartPos.Y);
                    }
                    SelRect.Width = Math.Abs(Pos.X - StartPos.X);
                    SelRect.Height = Math.Abs(Pos.Y - StartPos.Y);
                }
            } // +
            else if (IsEventMovIng)
            {
                Pos = Mouse.GetPosition(TexaditorWindow);
                if (Mouse.LeftButton == MouseButtonState.Released || Mouse.RightButton == MouseButtonState.Pressed)
                {
                    TexaditorWindow_MouseUp(sender, null);
                }
                else
                {
                    Dx = (Pos.X - StartMovePosition.X) + (Zoom * (OggReader.CurrentTime.TotalSeconds - StartEventPosition));
                    IsEventMovEd = true;
                    if (PreparingToCopy)
                    {
                        ManualCopyEvent();
                        PreparingToCopy = false;
                    }
                    else
                    {
                        foreach (Event oneEvent in Events.FindAll(i=>i.IsMoving == true))
                        {
                            oneEvent.LeftMargin = (oneEvent.Time) * Zoom - 30 + Dx;
                        }
                    }
                }
            } // +
            else if (Scrolling && Mouse.LeftButton == MouseButtonState.Released)
            {
                Scrolling = false;
                if (PreparingToPlay) AudioOut.Play();
            } // +
        } // =WORKS=
        #endregion
        #region Timing event
        private void CreateTiming(Event oneEvent)
        {
            TimingEvent = oneEvent;
            CreatingTiming = true;
            Times = new List<double>();
            HintTimingLabel.Visibility = Visibility.Visible;
            AudioOut.Play();
        } // Starting creating timing =+= 2 CALLS
        private void TimingClicks()
        {
            if (Times.Count > 5 && TimingEvent != null)
            {
                List<double> freqs = new List<double>();
                for (int i = 0; i < Times.Count - 1; i++) freqs.Add(Times[i + 1] - Times[i]);
                int range = (int)Math.Floor((double)(freqs.Count / 4));
                double all = 0;
                double FirstClick = Times[0];
                freqs.Sort();
                freqs.RemoveRange(0, range);
                freqs.RemoveRange(freqs.Count - 1 - range, range);
                foreach (double d in freqs) all += d;
                double Frequency = all / freqs.Count;
                if (FirstClick > TimingEvent.Time)
                {
                    while (FirstClick - Frequency >= TimingEvent.Time) FirstClick -= Frequency;
                }
                else
                {
                    while (FirstClick < TimingEvent.Time) FirstClick += Frequency;
                }
                TimingEvent.SetData(1, Math.Round(Frequency,3).ToString(), "Timing");
                TimingEvent.SetData(2, Math.Round(FirstClick - TimingEvent.Time,3).ToString(), "Timing");
                ((WEventTiming)TimingEvent.GetBorder.Child).SetData(TimingEvent);
            }
            else MessageBox.Show("ERROR 0x000010");
            HintTimingLabel.Visibility = Visibility.Collapsed;
            AudioOut.Pause();
            CreatingTiming = false;
            ChangingTiming = false;
            ChangeVisuals();
        } // Math the timing =+= 1 BUTTON
        private void AddTimingClick()
        {
            if (IsMuted == false)
            {
                DirectSoundOut chs = new DirectSoundOut();
                chs.Init(new VorbisWaveReader(System.IO.Path.GetFullPath("Nvorbis.AudioOut.dll")));
                chs.Play();
            }
            ChangingTiming = true;
            Times.Add(OggReader.CurrentTime.TotalSeconds);
        } // adds one timing tick =+= 1 BUTTON
        #endregion
        #endregion 
        #region Нажатия клавиатуры / мыши
        private void Editor_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ChangingTiming == false)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl)) 
                {
                    if (e.Delta < 0) { ChangeZoom(-1); }//колесо вниз
                    else if (e.Delta > 0) { ChangeZoom(1); }//колесо вверх
                }//приблизить отдалить
                else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift)) 
                {
                    if (e.Delta < 0) { ChangeCaretMargin(10); }//колесо вниз
                    else if (e.Delta > 0) { ChangeCaretMargin(-10); }//колесо вверх
                } // двигать каретку 
                else if (Keyboard.IsKeyDown(Key.LeftAlt) | Keyboard.IsKeyDown(Key.RightAlt)) 
                {
                    if (e.Delta < 0) { VolDown(); }//колесо вниз
                    else if (e.Delta > 0) { VolUp(); }//колесо вверх
                }//громкость
                else
                {
                    if (e.Delta < 0) { ScrollRight(); }//колесо вниз
                    else if (e.Delta > 0) { ScrollLeft(); }//колесо вверх
                }//прокрутка песни колесом
            }
        } // Scrolling using mouse wheel =WORKS=
        private void TexaditorWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Tab.SelectedIndex != 1)
            {
                if (CreatingTiming == false)
                {
                    switch (e.Key)
                    {
                        case Key.F1:
                            FastSave(30000);
                            MessageBox.Show("FAST SAVE COMPLETED");
                            break;
                        case Key.F2:
                            string data = "RESOURCES LIST: \r";
                            foreach (MapResource mr in EditorData.levelResources) { data += mr.name + " " + mr.path + " " + mr.type + "\r"; };
                            MessageBox.Show(data);
                            break;
                        case Key.F3:
                            InvertSelection();
                            break;
                        case Key.F4:
                            if (MessageBox.Show("Alt+F4 makes me CRAZY", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.No)
                            {
                                if (MessageBox.Show("YES", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.No)
                                {
                                    if (MessageBox.Show("YES!!!!!", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.No)
                                    {
                                        if (MessageBox.Show("Y  E  S  !  !  !  !", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.No)
                                        {
                                            if (MessageBox.Show("okay...", "WARNING", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No) == MessageBoxResult.No)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case Key.F5:
                            if (RepeatStart == null)
                            {
                                RepeatStart = OggReader.CurrentTime.TotalSeconds;
                            }
                            else if (RepeatStop == null)
                            {
                                RepeatStop = OggReader.CurrentTime.TotalSeconds;
                            }
                            else
                            {
                                RepeatStart = null;
                                RepeatStop = null;
                            }
                            break;
                        case Key.F6:
                            RepeatStart = null;
                            RepeatStop = null;
                            break;
                        case Key.F7:
                            SelectAll();
                            break;
                        case Key.F8:
                            if (timer.IsEnabled)
                            {
                                timer.Stop();
                            }
                            else
                            {
                                timer.Start();
                            }
                            break;
                        case Key.F9:
                            System.Diagnostics.Process.Start("https://unicode-table.com/en/2012/");
                            break;
                        case Key.F10:
                            Mute();
                            break;
                        case Key.F11:
                            OpenExplorer(ConfigReg.DirDefault);
                            break;
                        case Key.F12:
                            OpenMainWindow();
                            break;
                        case Key.Escape:
                            Cancel();
                            break;
                        case Key.Delete:
                            if (Keyboard.IsKeyDown(Key.LeftCtrl)) RemoveEventAll();
                            else RemoveEventAllSelected();
                            break;
                        default:
                            if (IsFocusEventWindow)
                            {
                                
                            }
                            else
                            {
                                switch (e.Key)
                                {
                                    case Key.W:
                                        if (FreeKeyW)
                                        {
                                            FreeKeyW = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[5] });
                                            } // Arc
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[10] });
                                            }// Arc
                                            else
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[0] });
                                            }// Arc
                                        }
                                        break;
                                    case Key.A:
                                        if (FreeKeyA)
                                        {
                                            FreeKeyA = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[6] });
                                            }// Arc
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[11] });
                                            }// Arc
                                            else
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[1] });
                                            }// Arc
                                        }
                                        break;
                                    case Key.S:
                                        if (FreeKeyS)
                                        {
                                            FreeKeyS = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[7] });

                                            }// Arc
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[12] });
                                            }// Arc
                                            else
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[2] });
                                            }// Arc
                                        }
                                        break;
                                    case Key.D:
                                        if (FreeKeyD)
                                        {
                                            FreeKeyD = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[8] });
                                            }// Arc
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[13] });
                                            }// Arc
                                            else
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[3] });
                                            }// Arc
                                        }
                                        break;
                                    case Key.Q:
                                        if (FreeKeyQ)
                                        {
                                            FreeKeyQ = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[9] });
                                            }// Arc
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[14] });
                                            }// Arc
                                            else
                                            {
                                                ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[4] });
                                            }// Arc
                                        }
                                        break;
                                    case Key.Z:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            UndoMethod();
                                        }//отмена действия
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {
                                            RedoMethod();
                                        } //отмена отмененного действия
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[1] });
                                        }//TIMING CREATE EVENT
                                        break;
                                    case Key.X:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            ManualRemoveEvent(LastEventLeft);
                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {
                                        }
                                        else
                                        {
                                            ManualRemoveEvent(LastEventRight);
                                        }
                                        break;
                                    case Key.Space:
                                        ChangePlaybackState();
                                        break;
                                    case Key.Tab:
                                        ChangeSelect(LastEventRight);
                                        break;
                                    case Key.F:
                                        ChangePlaybackState();
                                        break;
                                    case Key.E:
                                        if (FreeKeyE)
                                        {
                                            FreeKeyE = false;
                                            if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                            {
                                                ChangeConfigWindowMode();
                                            }//event config toggle
                                            else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                            {
                                                ChangeEventMode();
                                                //UnSelectAll();
                                            }
                                            else
                                            {
                                                ManualCreateEvent(new string[] { ConfigReg.LastEvent });
                                            }
                                        }
                                        break;
                                    case Key.T:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            RedoMethod();
                                        }//Redo
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[2] });
                                        } //Event
                                        break;
                                    case Key.Y:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            RedoMethod();
                                        }//Redo
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[3] });
                                        } //Event
                                        break;
                                    case Key.U:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {

                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[4] });
                                        } //Event
                                        break;
                                    case Key.G:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            
                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[5] });
                                        } //Event
                                        break;
                                    case Key.H:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            
                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[6] });
                                        } //Event
                                        break;
                                    case Key.J:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {
                                            
                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[7] });
                                        } //Event
                                        break;
                                    case Key.C:
                                        if (Keyboard.IsKeyDown(Key.LeftCtrl) | Keyboard.IsKeyDown(Key.RightCtrl))
                                        {

                                        }
                                        else if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightShift))
                                        {

                                        }
                                        else
                                        {
                                            ManualCreateEvent(new string[] { Helpers.eventTypes[0], Helpers.patternsMap[15] });
                                        } //Arc
                                        break;

                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.Z:
                            TimingClicks();
                            break;
                        case Key.X:
                            AddTimingClick();
                            break;
                    }
                }
            } 
        } // Key pressing
        private void TexaditorWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    FreeKeyW = true;
                    break;
                case Key.S:
                    FreeKeyS = true;
                    break;
                case Key.A:
                    FreeKeyA = true;
                    break;
                case Key.D:
                    FreeKeyD = true;
                    break;
                case Key.Q:
                    FreeKeyQ = true;
                    break;
                case Key.E:
                    FreeKeyE = true;
                    break;
            }
            
        } // Key releasing =WORKS=
        #endregion



        
    }
}   