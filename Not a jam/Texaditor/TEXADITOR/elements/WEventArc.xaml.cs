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
    /// Логика взаимодействия для WEventArc.xaml
    /// </summary>
    public partial class WEventArc : Border
    {
        //public static DependencyProperty WBrushProperty;

        //static WEventArc()
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
                string d = data.GetData(1);
                if (d != null)
                {
                    if (d.Contains("Up")) UpImage.Visibility = Visibility.Visible; else UpImage.Visibility = Visibility.Hidden;
                    if (d.Contains("Left")) LeftImage.Visibility = Visibility.Visible; else LeftImage.Visibility = Visibility.Hidden;
                    if (d.Contains("Right")) RightImage.Visibility = Visibility.Visible; else RightImage.Visibility = Visibility.Hidden;
                    if (d.Contains("Down")) DownImage.Visibility = Visibility.Visible; else DownImage.Visibility = Visibility.Hidden;
                }
                else MessageBox.Show("ERROR 0x000013 INFO:" + data.GetData(1));
            }
            else MessageBox.Show("ERROR 0x000013 INFO:" + data);
        }

        public WEventArc()
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
