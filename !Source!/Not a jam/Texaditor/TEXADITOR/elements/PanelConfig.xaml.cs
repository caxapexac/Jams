using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntralismSharedEditor;

namespace TEXADITOR.elements
{
    /// <summary>
    /// Логика взаимодействия для PanelConfig.xaml
    /// </summary>
    public partial class PanelConfig : StackPanel
    {
        public Event ChoosedEvent;
        private List<MapResource> resources = new List<MapResource>();
        private List<Image> imgList = new List<Image>();
        private ColorChoose cc = new ColorChoose();
        private string mapPath;
        private bool isBlock = false;

        public PanelConfig(List<MapResource> mr, string resDir)
        {
            InitializeComponent();
            ColorBorder.Child = cc;
            ColorBorder.Visibility = Visibility.Collapsed;
            PanelStoryboard.Visibility = Visibility.Collapsed;
            PanelGameplay.Visibility = Visibility.Collapsed;
            PanelMapping.Visibility = Visibility.Collapsed;
            mapPath = resDir;
            foreach (MapResource res in mr) AddRes(res);
            //==========================================================================================================================
            //Main
            ButTime.Click += ButTime_Click;
            ButSel.Click += ButSel_Click;
            //"SpawnObj", // 0 арка 2

            //"Timing", // 1 тайминг (игнор игрой) 4
            ButTimingFrequency.Click += ButTimingFrequency_Click;
            ButTimingFirstTick.Click += ButTimingFirstTick_Click;
            ButTimingTickNum.Click += ButTimingTickNum_Click;
            //"Note", // 2 заметка (игнор картой) 1
            PropNoteText.TextChanged += PropNoteText_TextChanged;
            //"SetBGColor", // 3 2
            ButColor.Click += ButColor_Click;
            ButColorSpeedUp.Click += ButColorSpeedUp_Click;
            ButColorSpeedDown.Click += ButColorSpeedDown_Click;
            ButColorSpeed.Click += ButColorSpeed_Click;
            //"SetPlayerDistance", // 4 1
            PropDistanceDistance.TextChanged += PropDistanceDistance_TextChanged;
            //"ShowTitle", // 5 2
            PropTextText.TextChanged += PropTextText_TextChanged;
            ButTextLength.Click += ButTextLength_Click;
            ButTextLengthDown.Click += ButTextLengthDown_Click;
            ButTextLengthUp.Click += ButTextLengthUp_Click;
            //"ShowSprite", // 6 3
            PropPictureBackground.Checked += PropPictureBackground_Checked;
            PropPictureForeground.Checked += PropPictureForeground_Checked;
            ButPictureLength.Click += ButPictureLength_Click;
            ButPictureLengthDown.Click += ButPictureLengthDown_Click;
            ButPictureLengthUp.Click += ButPictureLengthUp_Click;
            PropPictureKeep.Checked += PropPictureKeep_Checked;
            PropPictureKeep.Unchecked += PropPictureKeep_Unchecked;
            PropPicturePicture.SelectionChanged += PropPicturePicture_SelectionChanged;
            //"MapEnd" // 7 0

            //Switch
            ButSwitchColor.Click += ButSwitchColor_Click;
            ButSwitchDistance.Click += ButSwitchDistance_Click;
            ButSwitchEnd.Click += ButSwitchEnd_Click;
            ButSwitchNote.Click += ButSwitchNote_Click;
            ButSwitchPicture.Click += ButSwitchPicture_Click;
            ButSwitchText.Click += ButSwitchText_Click;
            ButSwitchTiming.Click += ButSwitchTiming_Click;
            //==========================================================================================================================
        }

        //==============================================================================================================================
        //Main
        private void ButTime_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    if (ChoosedEvent.TrySetTime(PropTime.Text) == false)
                    {
                        PropTime.Text = ChoosedEvent.Time.ToString();
                        PropTime.Focusable = false;
                        PropTime.Focusable = true;
                    }
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //Time
        private void ButSel_Click(object sender, RoutedEventArgs e)
        {
            ChoosedEvent.ChangeSelect();
            ChoosedEvent.NeedUpdate = true;
            if (ChoosedEvent.IsSelected) ButSel.Content = "Unselect"; else ButSel.Content = "Select";
        } //Select/Unselect
        //==============================================================================================================================
        //==============================================================================================================================
        //"SpawnObj", // 0 арка 2

        //==============================================================================================================================
        //==============================================================================================================================
        //"Timing", // 1 тайминг (игнор игрой) 4
        private void ButTimingFrequency_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    if(ChoosedEvent.GetData(1, "Timing") == PropTimingFrequency.Text)
                    {
                        if (double.TryParse(PropTimingBPM.Text, out double bpm))
                        {
                            bpm = Math.Round(60 / bpm,3);
                            ChoosedEvent.SetData(1, bpm.ToString() , "Timing");
                        }
                    }
                    else
                    {
                        if (double.TryParse(PropTimingFrequency.Text, out double freq))
                        {
                            ChoosedEvent.SetData(1, freq.ToString() , "Timing");
                        }
                    }
                    PropTimingFrequency.Text = ChoosedEvent.GetData(1, "Timing");
                    PropTimingBPM.Text = (60 / double.Parse(ChoosedEvent.GetData(1, "Timing"))).ToString();
                    PropTimingFrequency.Focusable = false;
                    PropTimingFrequency.Focusable = true;
                    PropTimingBPM.Focusable = false;
                    PropTimingBPM.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        private void ButTimingFirstTick_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    ChoosedEvent.SetData(2, PropTimingFirstTick.Text, "Timing");
                    PropTimingFirstTick.Text = ChoosedEvent.GetData(2, "Timing");
                    PropTimingFirstTick.Focusable = false;
                    PropTimingFirstTick.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //2
        private void ButTimingTickNum_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    ChoosedEvent.SetData(4, PropTimingTickNum.Text, "Timing");
                    PropTimingTickNum.Text = ChoosedEvent.GetData(4, "Timing");
                    PropTimingTickNum.Focusable = false;
                    PropTimingTickNum.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //4
        //==============================================================================================================================
        //==============================================================================================================================
        //"Note", // 2 заметка (игнор картой) 1
        private void PropNoteText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(1, ((TextBox)sender).Text, "Note");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        //==============================================================================================================================
        //==============================================================================================================================
        //"SetBGColor", // 3 2
        private void ButColor_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    if(ColorBorder.Visibility == Visibility.Collapsed)
                    {
                        ColorBorder.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ColorBorder.Visibility = Visibility.Collapsed;
                        ChoosedEvent.SetData(1, cc.R.ToString(), "SetBGColor");
                        ChoosedEvent.SetData(2, cc.G.ToString(), "SetBGColor");
                        ChoosedEvent.SetData(3, cc.B.ToString(), "SetBGColor");
                        PropColorColor.Fill = new SolidColorBrush(Color.FromRgb((byte)(cc.R * 255), (byte)(cc.G * 255), (byte)(cc.B * 255)));
                    }
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        private void ButColorSpeedUp_Click(object sender, RoutedEventArgs e)
        {
            ButColorSpeed_Click(null, null);
            PropColorSpeed.Text = ((double.Parse(PropColorSpeed.Text)) + 1).ToString();
            ButColorSpeed_Click(null, null);
        } //4
        private void ButColorSpeedDown_Click(object sender, RoutedEventArgs e)
        {
            ButColorSpeed_Click(null, null);
            PropColorSpeed.Text = ((double.Parse(PropColorSpeed.Text)) - 1).ToString();
            ButColorSpeed_Click(null, null);
        } //4
        private void ButColorSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    ChoosedEvent.SetData(4, PropColorSpeed.Text, "SetBGColor");
                    PropColorSpeed.Text = ChoosedEvent.GetData(4, "SetBGColor");
                    PropColorSpeed.Focusable = false;
                    PropColorSpeed.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //4
        //==============================================================================================================================
        //==============================================================================================================================
        //"SetPlayerDistance", // 4 1
        private void PropDistanceDistance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(1, ((TextBox)sender).Text, "SetPlayerDistance");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        //==============================================================================================================================
        //==============================================================================================================================
        //"ShowTitle", // 5 2
        private void PropTextText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(1, ((TextBox)sender).Text, "ShowTitle");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        private void ButTextLengthUp_Click(object sender, RoutedEventArgs e)
        {
            ButTextLength_Click(null, null);
            PropTextLenght.Text = ((double.Parse(PropTextLenght.Text)) + 1).ToString();
            ButTextLength_Click(null, null);
        } //2
        private void ButTextLengthDown_Click(object sender, RoutedEventArgs e)
        {
            ButTextLength_Click(null, null);
            PropTextLenght.Text = ((double.Parse(PropTextLenght.Text)) - 1).ToString();
            ButTextLength_Click(null, null);
        } //2
        private void ButTextLength_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    ChoosedEvent.SetData(2, PropTextLenght.Text, "ShowTitle");
                    PropTextLenght.Text = ChoosedEvent.GetData(2, "ShowTitle");
                    PropTextLenght.Focusable = false;
                    PropTextLenght.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //2
        //==============================================================================================================================
        //==============================================================================================================================
        //"ShowSprite", // 6 3
        private void PropPicturePicture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(1, ((Image)PropPicturePicture.SelectedItem).Name.Substring(1), "ShowSprite");
                    PropPictureName.Content = ChoosedEvent.GetData(1, "ShowSprite");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //1
        private void PropPictureForeground_Checked(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(2, "2", "ShowSprite");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //2
        private void PropPictureBackground_Checked(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(2, "1", "ShowSprite");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //2
        private void PropPictureKeep_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(3, "false", "ShowSprite");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //3
        private void PropPictureKeep_Checked(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    ChoosedEvent.SetData(3, "true", "ShowSprite");
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //3
        private void ButPictureLengthUp_Click(object sender, RoutedEventArgs e)
        {
            ButPictureLength_Click(null, null);
            PropPictureLength.Text = ((double.Parse(PropPictureLength.Text)) + 1).ToString();
            ButPictureLength_Click(null, null);
        } //4
        private void ButPictureLengthDown_Click(object sender, RoutedEventArgs e)
        {
            ButPictureLength_Click(null, null);
            PropPictureLength.Text = ((double.Parse(PropPictureLength.Text)) - 1).ToString();
            ButPictureLength_Click(null, null);
        } //4
        private void ButPictureLength_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosedEvent != null)
            {
                if (isBlock == false)
                {
                    isBlock = true;
                    ChoosedEvent.SetData(4, PropPictureLength.Text, "ShowSprite");
                    PropPictureLength.Text = ChoosedEvent.GetData(4, "ShowSprite");
                    PropPictureLength.Focusable = false;
                    PropPictureLength.Focusable = true;
                    isBlock = false;
                }
            }
            else MessageBox.Show("ERROR 0x000024");
        } //4
        //==============================================================================================================================
        //==============================================================================================================================
        //"MapEnd" // 7 0

        //==============================================================================================================================
        //==============================================================================================================================
        //Switch
        private void ButSwitchTiming_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("Timing");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchTiming.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("Timing"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchTiming.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchText_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("ShowTitle");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchText.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("ShowTitle"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchText.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchPicture_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("ShowSprite");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchPicture.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("ShowSprite"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchPicture.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchNote_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("Note");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchNote.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("Note"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchNote.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchEnd_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("MapEnd");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchEnd.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("MapEnd"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchEnd.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchDistance_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("SetPlayerDistance");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchDistance.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("SetPlayerDistance"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchDistance.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        private void ButSwitchColor_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "ADD EVENT")
            {
                ChoosedEvent.AddEvent("SetBGColor");
                ((Button)sender).Content = "REMOVE EVENT";
                SwitchColor.Visibility = Visibility.Visible;
            }
            else
            {
                if (ChoosedEvent.TryRemoveEvent("SetBGColor"))
                {
                    ((Button)sender).Content = "ADD EVENT";
                    SwitchColor.Visibility = Visibility.Collapsed;
                }
                else MessageBox.Show("ERROR 0x000028 INFO:" + ((Button)sender).Content);
            }
            ChoosedEvent.NeedUpdate = true;
        }
        //==============================================================================================================================
        public void AddRes(MapResource res)
        {
            if (resources.Find(i=>i.name == res.name) == null)
            {
                resources.Add(res);
                Uri uri = new Uri(mapPath + "\\" + res.path);
                try
                {
                    Image img = new Image() { Name = "A" + res.name, Source = new BitmapImage(uri), Height = 128, Width = 128, ToolTip = res.name };
                    imgList.Add(img);
                    PropPicturePicture.Items.Add(img);
                }
                catch
                {
                    MessageBox.Show("ERROR 0x000031 INFO:" + res.name + ". Because resoures must contain ONLY english letters & numbers. NOT different letters or spaces!!! Also it must be .png or .jpg pics!");
                }
            }
            else MessageBox.Show("ERROR 0x000020 INFO:" + res.name);
        }
        public void RemRes(MapResource res)
        {
            if (resources.Find(i => i.name == res.name) == null)
            {
                Image img = imgList.Find(i=>i.Name == "A" + res.name);
                resources.Remove(res);
                PropPicturePicture.Items.Remove(img);
            }
            else MessageBox.Show("ERROR 0x000021 INFO:" + res.name);
        }
        //==============================================================================================================================
        public void UpdateVisual(Event oneEvent)
        {
            isBlock = true;
            ColorBorder.Visibility = Visibility.Collapsed;
            if (oneEvent != null)
            {
                if (oneEvent.IsSelected) ButSel.Content = "Unselect"; else ButSel.Content = "Select";
                ChoosedEvent = oneEvent;
                PanelStoryboard.Visibility = Visibility.Collapsed;
                PanelGameplay.Visibility = Visibility.Collapsed;
                PanelMapping.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Visible;
                PropTime.Text = oneEvent.Time.ToString();
                PropType.Content = oneEvent.Type;
                switch (oneEvent.Type)
                {
                    case "Storyboard":
                        PanelStoryboard.Visibility = Visibility.Visible;
                        SwitchColor.Visibility = Visibility.Collapsed;
                        SwitchDistance.Visibility = Visibility.Collapsed;
                        SwitchPicture.Visibility = Visibility.Collapsed;
                        SwitchText.Visibility = Visibility.Collapsed;
                        SwitchEnd.Visibility = Visibility.Collapsed;
                        ButSwitchColor.Content = "ADD EVENT";
                        ButSwitchDistance.Content = "ADD EVENT";
                        ButSwitchPicture.Content = "ADD EVENT";
                        ButSwitchText.Content = "ADD EVENT";
                        ButSwitchEnd.Content = "ADD EVENT";
                        break;
                    case "Gameplay":
                        PanelGameplay.Visibility = Visibility.Visible;
                        break;
                    case "Mapping":
                        PanelMapping.Visibility = Visibility.Visible;
                        SwitchTiming.Visibility = Visibility.Collapsed;
                        SwitchNote.Visibility = Visibility.Collapsed;
                        ButSwitchTiming.Content = "ADD EVENT";
                        ButSwitchNote.Content = "ADD EVENT";
                        break;
                    default:
                        MessageBox.Show("ERROR 0x000018 INFO:" + oneEvent.Type);
                        break;
                }
                foreach (MapEvent mevent in oneEvent.mapEventList)
                {
                    switch (mevent.data[0])
                    {
                        case "SpawnObj":
                            PropArcType.Content = mevent.data[1];
                            PropArcHand.Content = mevent.data[2];
                            break;
                        case "Timing":
                            PropTimingFrequency.Text = mevent.data[1];
                            PropTimingBPM.Text = (60 / double.Parse(mevent.data[1])).ToString();
                            PropTimingFirstTick.Text = mevent.data[2];
                            PropTimingWidth.Content = mevent.data[3];
                            PropTimingTickNum.Text = mevent.data[4];
                            SwitchTiming.Visibility = Visibility.Visible;
                            ButSwitchTiming.Content = "REMOVE EVENT";
                            break;
                        case "Note":
                            PropNoteText.Text = mevent.data[1];
                            SwitchNote.Visibility = Visibility.Visible;
                            ButSwitchNote.Content = "REMOVE EVENT";
                            break;
                        case "SetBGColor":
                            PropColorColor.Fill = new SolidColorBrush(Color.FromRgb((byte)(double.Parse(mevent.data[1])*255), (byte)(double.Parse(mevent.data[2]) * 255), (byte)(double.Parse(mevent.data[3]) * 255)));
                            PropColorSpeed.Text = mevent.data[4];
                            SwitchColor.Visibility = Visibility.Visible;
                            ButSwitchColor.Content = "REMOVE EVENT";
                            break;
                        case "SetPlayerDistance":
                            PropDistanceDistance.Text = mevent.data[1];
                            SwitchDistance.Visibility = Visibility.Visible;
                            ButSwitchDistance.Content = "REMOVE EVENT";
                            break;
                        case "ShowTitle":
                            PropTextText.Text = mevent.data[1];
                            PropTextLenght.Text = mevent.data[2];
                            SwitchText.Visibility = Visibility.Visible;
                            ButSwitchText.Content = "REMOVE EVENT";
                            break;
                        case "ShowSprite":
                            PropPicturePicture.SelectedIndex = resources.FindIndex(i=>i.name == mevent.data[1]);
                            PropPictureName.Content = mevent.data[1];
                            PropPicturePath.Content = resources.Find(i => i.name == mevent.data[1])?.path??"NULL";
                            if (mevent.data[2] == "1")
                            {
                                PropPictureBackground.IsChecked = true;
                                PropPictureForeground.IsChecked = false;
                            }
                            else
                            {
                                PropPictureBackground.IsChecked = false;
                                PropPictureForeground.IsChecked = true;
                            }
                            PropPictureKeep.IsChecked = bool.Parse(mevent.data[3]);
                            PropPictureLength.Text = mevent.data[4];
                            SwitchPicture.Visibility = Visibility.Visible;
                            ButSwitchPicture.Content = "REMOVE EVENT";
                            break;
                        case "MapEnd":
                            SwitchEnd.Visibility = Visibility.Visible;
                            ButSwitchEnd.Content = "REMOVE EVENT";
                            break;
                        default:
                            MessageBox.Show("ERROR 0x000019 INFO:" + mevent.data[0]);
                            break;
                    }
                }
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
            isBlock = false;
        }
    }
}
