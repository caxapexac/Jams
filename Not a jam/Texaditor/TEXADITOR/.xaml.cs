using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IntralismSharedEditor;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
//using System.IO;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using NAudio;
//using NAudio.Wave;
//using NAudio.Vorbis;
//using OggVorbisEncoder;
//using System.ComponentModel;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using Microsoft.Win32;
//using Microsoft.DirectX.AudioVideoPlayback;
//using System.Configuration;
//using System.IO;

namespace TEXADITOR
{
    public partial class MainWindow : Window
    {
        List<string> Hints = new List<string>()
        {
            "Unstablest!",
            "Don't use commas (,) !",
            "Special for Senasmrt!",
            "I see...",
            "You never know..........",
            "Glad to see you!",
            "U R WELCOME",
            "OWO",
            "BAKABAKABAKABAKABAKA",
            "RLY?",

            "it is flying text.",
            "dat boi comes here",
            "oh shit waddup!",
            "What about some shitmapping?",
            "Fuck the police",
            "Texas was here~~~",
            "Nya",
            "Editor. Just an Editor.",
            "I LOVE IMAGINE DRAGONS",
            "Its my job to notice asses (c)",

            "Now I'm believer...",
            "HELP ME",
            "Sagiri is cute, isn't it?",
            "Everybody watched SAO))",
            "So....",
            "It's easter egg too :)",
            "RUSH B",
            "Dont press Alt+F4 please",
            "DAMN!",
            "Nah",

            "Got rekt((",
            "Okay, guy",
            "Better than OSU!",
            "Oxy, don't make me crazy!",
            "DELET THIS",
            "CHEATER",
            "ERROR 0x000000 It is bug too ;(",
            "Try to find all the easter eggs",
            "Double click on 'version'",
            "Global elite in mapping",

            "8k MMR in mapping. Got it. Finally.",
            "My elginsh bad is",
            "LOL YOU GONNA USE THIS APP",
            //43
            "Discord mystery server invite in this app!",
            "Everything will freeze! (F8)",
        };
        
        #region Запуск Основного Окна
        Window1 EditorWindow;
        public MainWindow()
        {
            //===================================================
            //Visuals
            InitializeComponent();
            if (ConfigReg.Version != Version.Content.ToString()) ConfigReg.DeleteAll();
            RegistryUpdate();
            Focus();
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Hint.BeginAnimation(FontSizeProperty, new DoubleAnimation()
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                From = 12,
                To = 16,
                Duration = TimeSpan.FromSeconds(2)
            });
            Hint.Content = Hints[new Random().Next(45)];
            //===================================================
            //Theme
            Themer.SelectedIndex = 0;
        }
        //private string ChoosedThemeList(int index)
        //{
        //    switch (index)
        //    {
        //        case 0:
        //            return "default";
        //        case 1:
        //            return "black";
        //        case 2:
        //            PudgeModeBox.IsChecked = true;
        //            return "caxa";
        //        case 3:
        //            return "sharp";
        //        case 4:
        //            return "oxy";
        //        default:
        //            return "default";
        //    }
        //}
        private void RegistryUpdate()
        {
            ConfigReg.Version = Version.Content.ToString();
            ConfigReg.Theme = "Default";
            if (ConfigReg.FullScreen == 1) FullScreenBox.IsChecked = true; else FullScreenBox.IsChecked = false;
            if (ConfigReg.Mode == 1) KawaiiBox.IsChecked = true; else KawaiiBox.IsChecked = false;
            TimesBox.Text = ConfigReg.TimingTimes.ToString();
            if ((bool)KawaiiBox.IsChecked) KawaiiBox.Content = "KAWAII ON";
            if (ConfigReg.DirContinue == null) ContinueButton.Visibility = Visibility.Collapsed;
            ConfigReg.FPS = 4;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //THEME
        }
        private void StartEditor(string type)
        {
            while (ConfigReg.DirDefault == null || Directory.Exists(ConfigReg.DirDefault) == false) SaveMapFolder();
            Microsoft.Win32.OpenFileDialog ChooseMusic = new Microsoft.Win32.OpenFileDialog();
            switch (type)
            {
                case "Create":
                    ChooseMusic.DefaultExt = ".ogg";
                    ChooseMusic.Filter = "OGG music (*.ogg)|*.ogg|MP3 music (NOT SUPPORTED YET) (*.mp3)|*.mp3|FLAC music (NOT SUPPORTED YET) (*.flac)|*.flac|WAV music (NOT SUPPORTED YET) (*.wav)|*.wav";
                    ChooseMusic.Multiselect = false;
                    ChooseMusic.Title = "Choose a music in OGG format";
                    if (ConfigReg.DirCreate != null && Directory.Exists(ConfigReg.DirCreate))
                    {
                        ChooseMusic.InitialDirectory = ConfigReg.DirCreate;
                    }
                    if (ChooseMusic.ShowDialog() == true)
                    {
                        if (ChooseMusic.FileName.EndsWith(".ogg"))
                        {
                            EditorWindow = new Window1(type, ChooseMusic.FileName);
                            Close();
                        }
                        else
                        {
                            System.Diagnostics.Process.Start("http://online-audio-converter.com/");
                        }
                    }
                    break;
                case "Load":
                    ChooseMusic.DefaultExt = ".txt";
                    ChooseMusic.Filter = "Old config files (*.txt)|*.txt|data files (map.data)|map.data";
                    ChooseMusic.Multiselect = false;
                    ChooseMusic.Title = "Choose a map";
                    if (ConfigReg.DirLoad != null && Directory.Exists(ConfigReg.DirLoad))
                    {
                        ChooseMusic.InitialDirectory = ConfigReg.DirLoad;
                    }
                    if (ChooseMusic.ShowDialog() == true)
                    {
                        if (ChooseMusic.FileName.EndsWith(".txt") || ChooseMusic.FileName.EndsWith(".data"))
                        {
                            string text = File.ReadAllText(ChooseMusic.FileName) ?? "";
                            if (text.Length > 0)
                            {
                                MapData md = CustomEditor.GetMap(text);
                                if (md != null)
                                {
                                    EditorWindow = new Window1(type, ChooseMusic.FileName);
                                    Close();
                                }
                                else MessageBox.Show("THIS DATA IS BROKEN IM SO SORRY");
                            }
                            else MessageBox.Show("YOU ARE BAKA BAKA BAKA BAKA BAKA BAKA BAKA BAKA BAKA BAKA BAKA (YOU GAVE ME BLANK TEXT FILE)");
                        }
                    }
                    break;
                case "Continue":
                    string name = ConfigReg.DirContinue + "\\map.data";
                    if (File.Exists(name))
                    {
                        string text = File.ReadAllText(name);
                        if (text != null && text != "")
                        {
                            MapData md = CustomEditor.GetMap(text);
                            if (md != null)
                            {
                                EditorWindow = new Window1(type, name);
                                Close();
                                break;
                            }
                        }
                    }
                    ConfigReg.DirContinue = "";
                    ContinueButton.Visibility = Visibility.Collapsed;
                    MessageBox.Show("SORRY, BUT I CANT FIND YOU LAST EDITED MAP..... \r I'M REALLY SORRY BUT THERE IS NO MAP IN THE " + ConfigReg.DirContinue??"NULL");
                    break;
            } // Choosing the map =WORKS=
        }
        private void SaveMapFolder()
        {
            System.Windows.Forms.FolderBrowserDialog Folder = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Choose directory for your maps:",
                ShowNewFolderButton = true
            };
            if (Folder.ShowDialog() == System.Windows.Forms.DialogResult.OK) ConfigReg.DirDefault = Folder.SelectedPath;
            if (ConfigReg.DirDefault == null) SaveMapFolder();
        }
        #endregion
        #region События
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.F1:
                    SaveMapFolder();
                    break;
                case Key.Space:
                    StartEditor("Continue");
                    break;
                case Key.C:
                    StartEditor("Create");
                    break;
                case Key.L:
                    StartEditor("Load");
                    break;
                default:

                    break;
            }
        }
        private void ChooseMapButton(object sender, RoutedEventArgs e) { StartEditor("Create"); }
        private void LoadButton_Click(object sender, RoutedEventArgs e) { StartEditor("Load"); }
        private void Continue_Click(object sender, RoutedEventArgs e) { StartEditor("Continue"); }
        private void ExitButton_Click(object sender, RoutedEventArgs e) { Close(); }
        private void MapFolder_Click(object sender, RoutedEventArgs e) { SaveMapFolder(); }
        private void FullScreenBox_Checked(object sender, RoutedEventArgs e) { ConfigReg.FullScreen = 1; }
        private void FullScreenBox_Unchecked(object sender, RoutedEventArgs e) { ConfigReg.FullScreen = 0; }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) { DragMove(); } }
        //Secret panel V
        private void Info_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://steamcommunity.com/id/caxp/");
        }
        private void Info_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SecretPanel.Visibility == Visibility.Visible) SecretPanel.Visibility = Visibility.Collapsed; else SecretPanel.Visibility = Visibility.Visible;
        }
        private void PassBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (((PasswordBox)sender).Password == "12345") GetBut.Visibility = Visibility.Visible;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int i))
            {
                ConfigReg.TimingTimes = i;
            }
            else MessageBox.Show("ONII CHAN SHOULD TYPE ONLY DIGITS DESU!");
        }
        private void TickBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(((TextBox)sender).Text, out int i))
            {
                ConfigReg.FPS = i;
            }
            else MessageBox.Show("ONII CHAN SHOULD TYPE ONLY DIGITS DESU!");
        }
        private void GetBut_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/PFzSpC");
            MessageBox.Show("https://discord.gg/PFzSpC");
        }
        private void KawaiiBox_Checked(object sender, RoutedEventArgs e) { ConfigReg.Mode = 1; KawaiiBox.Content = "KAWAII ON"; }
        private void KawaiiBox_Unchecked(object sender, RoutedEventArgs e) { ConfigReg.Mode = 0; KawaiiBox.Content = "KAWAII OFF"; }
        private void ReturnDefault_Click(object sender, RoutedEventArgs e) { ConfigReg.DeleteAll(); RegistryUpdate(); }
    }
    #endregion
}