using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Windows.Documents;
using System.Windows.Input;

/// TODO: ExeptionHandling @ Line 116

namespace WpfApplication3
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            load_winPos(Screen.AllScreens[0].WorkingArea.Width - this.Width - 50, 50);

            DateTime t = DateTime.Now;
            update_tbTime(t);
            update_tbDate(t);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(updateTime);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();
        }

        private void updateTime(object sender, EventArgs e)
        {
            DateTime t = DateTime.Now;
            update_tbTime(t);
            update_tbDate(t);
        }

        private void update_tbTime(DateTime t)
        {
            string hour = t.Hour.ToString("00");
            string min = t.Minute.ToString("00");

            tbTime.Text = "";
            tbTime.Inlines.Add(new Run(hour) { FontWeight = FontWeights.Bold });
            tbTime.Inlines.Add(":");
            tbTime.Inlines.Add(new Run(min) { FontWeight = FontWeights.Light });
        }

        private void update_tbDate(DateTime t)
        {
            string wday = t.DayOfWeek.ToString();
            string mday = t.Day.ToString();
            int month = (int)t.Month;

            string[] strMonth = { "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            tbDate.Text = "";
            tbDate.Inlines.Add(wday + " ");
            tbDate.Inlines.Add(new Run(mday) { FontWeight = FontWeights.Bold });
            tbDate.Inlines.Add(" " + strMonth[month]);
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            save_winPos();
            this.Close();
        }

        private void save_winPos()
        {
            var json = new JavaScriptSerializer().Serialize(Screen.AllScreens);
            try {
                var file = new StreamWriter("winpos.json");
                file.WriteLine(json);
                file.Write(this.Left + ":" + this.Top);
                file.Close();
            }
            catch (Exception e)
            {
                ; // ingore all
            }
        }

        private void load_winPos(double stdLeft, double stdTop)
        {
            double left = stdLeft;
            double top = stdTop;
            try
            {
                var file = new StreamReader("winpos.json");
                if (file.ReadLine().Equals((new JavaScriptSerializer().Serialize(Screen.AllScreens)).ToString()))
                {
                    string[] pos = file.ReadLine().Trim().Split(':');
                    left = Convert.ToDouble(pos[0]);
                    top = Convert.ToDouble(pos[1]);
                }
                file.Close();
            }
            catch (Exception e)
            {
                ; // ignore all
            }
            finally
            {
                this.Left = left;
                this.Top = top;
            }
        }
    }
}