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
    /// Логика взаимодействия для Wevent.xaml
    /// </summary>
    public partial class WEventStory : Border
    {
        //public static DependencyProperty WBrushProperty;

        //static WEventStory()
        //{
        //    WBrushProperty = DependencyProperty.Register(
        //        "WBrush",
        //        typeof(SolidColorBrush),
        //        typeof(WEventTiming));
        //    //new FrameworkPropertyMetadata(Colors.WhiteSmoke, new PropertyChangedCallback(OnColorChanged)));
        //}

        public void SetData(Event data)
        {
            if (data != null)
            {
                CountLabel.Content = data.mapEventList.Count;
                if (data.mapEventList.Find(i => i.data[0] == "SetBGColor") != null) SetBGColorImage.Visibility = Visibility.Visible; else SetBGColorImage.Visibility = Visibility.Collapsed;
                if (data.mapEventList.Find(i => i.data[0] == "SetPlayerDistance") != null) SetPlayerDistanceImage.Visibility = Visibility.Visible; else SetPlayerDistanceImage.Visibility = Visibility.Collapsed;
                if (data.mapEventList.Find(i => i.data[0] == "ShowTitle") != null) ShowTitleImage.Visibility = Visibility.Visible; else ShowTitleImage.Visibility = Visibility.Collapsed;
                if (data.mapEventList.Find(i => i.data[0] == "ShowSprite") != null) ShowSpriteImage.Visibility = Visibility.Visible; else ShowSpriteImage.Visibility = Visibility.Collapsed;
                if (data.mapEventList.Find(i => i.data[0] == "MapEnd") != null) MapEndImage.Visibility = Visibility.Visible; else MapEndImage.Visibility = Visibility.Collapsed;
            }
            else MessageBox.Show("ERROR 0x000012 INFO:" + data);
        }

        public WEventStory()
        {
            InitializeComponent();
        }

        //public SolidColorBrush WBrush
        //{
        //    get { return (SolidColorBrush)GetValue(WBrushProperty); }
        //    set { SetValue(WBrushProperty, value); ColorPolygon.Fill = WBrush; }
        //}
    }
}
