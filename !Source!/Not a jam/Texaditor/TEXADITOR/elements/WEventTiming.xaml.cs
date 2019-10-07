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
    /// Логика взаимодействия для WEventTiming.xaml
    /// </summary>
    public partial class WEventTiming : Border
    {
        //public static DependencyProperty WBrushProperty;

        //static WEventTiming()
        //{
        //    WBrushProperty = DependencyProperty.Register(
        //        "WBrush",
        //        typeof(SolidColorBrush),
        //        typeof(WEventTiming));
        //        //new FrameworkPropertyMetadata(Colors.WhiteSmoke, new PropertyChangedCallback(OnColorChanged)));
        //}

        public void SetData(Event data)
        {
            if (data != null)
            {
                CountLabel.Content = data.mapEventList.Count;
                if (data.mapEventList.Find(i => i.data[0] == "Timing") != null)
                {
                    TimingImage.Visibility = Visibility.Visible;
                    FreqLabel.Visibility = Visibility.Visible;
                    FirstLabel.Visibility = Visibility.Visible;
                    FreqLabel.Content = data.GetData(1, "Timing");
                    FirstLabel.Content = data.GetData(1, "Timing");
                }
                else
                {
                    TimingImage.Visibility = Visibility.Collapsed;
                    FreqLabel.Visibility = Visibility.Collapsed;
                    FirstLabel.Visibility = Visibility.Collapsed;
                }
                if (data.mapEventList.Find(i => i.data[0] == "Note") != null)
                {
                    NoteImage.Visibility = Visibility.Visible;
                    NoteBlock.Visibility = Visibility.Visible;
                    NoteBlock.Text = data.GetData(1, "Note");
                }
                else
                {
                    NoteImage.Visibility = Visibility.Collapsed;
                    NoteBlock.Visibility = Visibility.Collapsed;
                }
            }
            else MessageBox.Show("ERROR 0x000014 INFO:" + data);
        }

        public WEventTiming()
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
