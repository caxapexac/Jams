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

namespace TEXADITOR.elements
{
    /// <summary>
    /// Логика взаимодействия для ColorChoose.xaml
    /// </summary>
    public partial class ColorChoose : UserControl
    {
        private double r = 1;
        private double g = 0;
        private double b = 0;
        private double dr = 1;
        private double dg = 0;
        private double db = 0;
        private bool ChangingGradient = false;
        private bool ChangingColor = false;

        public ColorChoose()
        {
            InitializeComponent();
            Rainbow.MouseDown += Rainbow_MouseDown;
            DiffColor.MouseDown += DiffColor_MouseDown;
            Rainbow.MouseUp += Rainbow_MouseUp;
            DiffColor.MouseUp += DiffColor_MouseUp;
            Rainbow.MouseMove += Rainbow_MouseMove;
            DiffColor.MouseMove += DiffColor_MouseMove;
            Diff2Color.MouseDown += DiffColor_MouseDown;
            Diff2Color.MouseUp += DiffColor_MouseUp;
            Diff2Color.MouseMove += DiffColor_MouseMove;
            RedSlider.ValueChanged += RedSlider_ValueChanged;
            GreenSlider.ValueChanged += GreenSlider_ValueChanged;
            BlueSlider.ValueChanged += BlueSlider_ValueChanged;
            RedBox.TextChanged += RedBox_TextChanged;
            GreenBox.TextChanged += GreenBox_TextChanged;
            BlueBox.TextChanged += BlueBox_TextChanged;
            MainGrid.MouseMove += DiffColor_MouseMove;
        }

        public void SetColor(double red, double green, double blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        public void UpdateColor()
        {
            RedBox.Text = R.ToString();
            GreenBox.Text = G.ToString();
            BlueBox.Text = B.ToString();
            RedSlider.Value = R;
            GreenSlider.Value = G;
            BlueSlider.Value = B;
            Choosed.Fill = new SolidColorBrush(Color.FromRgb((byte)(255 * R), (byte)(255 * G), (byte)(255 * B)));
        }

        private void RedBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(double.TryParse(RedBox.Text,out double res) && res >= 0 && res <= 1)
            {
                R = res;
            }
            else
            {
                if (RedBox.Text != "" && RedBox.Text.IndexOf(char.Parse(".")) == RedBox.Text.LastIndexOf(char.Parse(".")))
                {
                    MessageBox.Show("YOU ARE INVALID. THIS IS FOR NUMBERS ONLY 0<=n<=1");
                    RedBox.Text = R.ToString();
                }
            }
        }
        private void GreenBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(GreenBox.Text, out double res) && res >= 0 && res <= 1)
            {
                G = res;
            }
            else
            {
                if (GreenBox.Text != "" && GreenBox.Text.IndexOf(char.Parse(".")) == GreenBox.Text.LastIndexOf(char.Parse(".")))
                {
                    MessageBox.Show("YOU ARE INVALID. THIS IS FOR NUMBERS ONLY 0<=n<=1");
                    GreenBox.Text = G.ToString();
                }
            }
        }
        private void BlueBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(BlueBox.Text, out double res) && res >= 0 && res <= 1)
            {
                B = res;
            }
            else
            {
                if (BlueBox.Text != "" && BlueBox.Text.IndexOf(char.Parse(".")) == BlueBox.Text.LastIndexOf(char.Parse(".")))
                {
                    MessageBox.Show("YOU ARE INVALID. THIS IS FOR NUMBERS ONLY 0<=n<=1");
                    BlueBox.Text = B.ToString();
                }
            }
        }

        private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R = RedSlider.Value;
        }
        private void GreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            G = GreenSlider.Value;
        }
        private void BlueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            B = BlueSlider.Value;
        }
        
        private void DiffColor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangingColor = true;
            Point pos = Mouse.GetPosition(DiffColor);
            double vertInf = 0;
            double horInf = 0;
            if (pos.Y < 0) vertInf = 1;
            else if (pos.Y > 100) vertInf = 0;
            else vertInf = pos.Y / -100 + 1; //влияет на влияние
            if (pos.X < 0) horInf = 1;
            else if (pos.X > 100) horInf = 0;
            else horInf = pos.X / -100 + 1; //влияет на общий цвет
            if (dr == 1) R = dr * vertInf; else R = (dr + (horInf * (1 - dr))) * vertInf;
            if (dg == 1) G = dg * vertInf; else G = (dg + (horInf * (1 - dg))) * vertInf;
            if (db == 1) B = db * vertInf; else B = (db + (horInf * (1 - db))) * vertInf;
        }
        private void DiffColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangingColor = false;
        }
        private void DiffColor_MouseMove(object sender, MouseEventArgs e)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (ChangingColor)
                {
                    Point pos = Mouse.GetPosition(DiffColor);
                    double vertInf = 0;
                    double horInf = 0;
                    if (pos.Y < 0) vertInf = 1;
                    else if (pos.Y > 100) vertInf = 0;
                    else vertInf = pos.Y / -100 + 1; //влияет на влияние
                    if (pos.X < 0) horInf = 1;
                    else if (pos.X > 100) horInf = 0;
                    else horInf = pos.X / -100 + 1; //влияет на общий цвет
                    if (dr == 1) R = dr * vertInf; else R = (dr + (horInf * (1 - dr))) * vertInf;
                    if (dg == 1) G = dg * vertInf; else G = (dg + (horInf * (1 - dg))) * vertInf;
                    if (db == 1) B = db * vertInf; else B = (db + (horInf * (1 - db))) * vertInf;
                }
            }
            else
            {
                ChangingColor = false;
            }
        }

        private void ChangeGradient()
        {
            Point pos = Mouse.GetPosition(Rainbow);
            if (pos.Y >= 0 && pos.Y < 16.6)
            {
                dr = 1;
                dg = pos.Y / 16.6; //g 0-1
                db = 0;
            }
            else if (pos.Y >= 16.6 && pos.Y < 33.3)
            {
                dr = (pos.Y - 16.6) / -16.6 + 1; //r 1-0
                dg = 1;
                db = 0;
            }
            else if(pos.Y >= 33.3 && pos.Y < 50)
            {
                dr = 0;
                dg = 1;
                db = (pos.Y - 33.3) / 16.6; //b 0-1
            }
            else if (pos.Y >= 50 && pos.Y <= 66.6)
            {
                dr = 0;
                dg = (pos.Y - 50) / -16.6 + 1; //g 1-0
                db = 1;
            }
            else if (pos.Y >= 66.6 && pos.Y <= 83.3)
            {
                dr = (pos.Y - 66.6) / 16.6;//r 0-1
                dg = 0;
                db = 1;
            }
            else if (pos.Y >= 83.3 && pos.Y <= 99)
            {
                double diff = pos.Y - 83.3;
                dr = 1;
                dg = 0;
                db = (pos.Y - 83.3) / -16.6 + 1; //b 1-0
            }
            else
            {
                dr = 1;
                dg = 0;
                db = 0;
            }
            RedLabel.Content = dr;
            GreenLabel.Content = dg;
            BlueLabel.Content = db;
            ((GradientBrush)DiffColor.Fill).GradientStops[0] = new GradientStop(Color.FromRgb((byte)(dr * 255), (byte)(dg * 255), (byte)(db * 255)), 1);
        }

        private void Rainbow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //0 - 16.6 - 33.3 - 50 - 66.6 - 83.3 - 100
            ChangingGradient = true;
            ChangeGradient();
        }
        private void Rainbow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangingGradient = false;
            ChangeGradient();
        }
        private void Rainbow_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (ChangingGradient) ChangeGradient();
            }
            else
            {
                ChangingGradient = false;
            }
        }

        public double R
        {
            get
            {
                return r;
            }
            set
            {
                r = Math.Round(value,3);
                UpdateColor();

            }
        }
        public double G
        {
            get
            {
                return g;
            }
            set
            {
                g = Math.Round(value, 3); ;
                UpdateColor();
            }
        }
        public double B
        {
            get
            {
                return b;
            }
            set
            {
                b = Math.Round(value, 3); ;
                UpdateColor();
            }
        }

    }
}
